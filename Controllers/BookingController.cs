using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Data;
using RoomBookingApp.Models;

namespace RoomBookingApp.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? filterDate, bool myBookingsOnly = false)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var query = _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .AsQueryable();

            if (myBookingsOnly)
            {
                query = query.Where(b => b.UserId == currentUserId);
                ViewBag.MyBookingsOnly = true;
            }

            if (filterDate.HasValue)
            {
                query = query.Where(b => b.BookingDate.Date == filterDate.Value.Date);
                ViewBag.FilterDate = filterDate.Value.ToString("yyyy-MM-dd");
            }

            ViewBag.CurrentUserId = currentUserId;

            var bookings = await query.OrderByDescending(b => b.BookingDate).ToListAsync();
            return View(bookings);
        }
        
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms.Where(r => r.IsActive), "RoomId", "RoomName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            booking.UserId = userId;

            booking.BookingDate = DateTime.SpecifyKind(booking.BookingDate.Date, DateTimeKind.Utc);

            if (booking.BookingDate.Date < DateTime.UtcNow.Date)
            {
                ModelState.AddModelError("BookingDate", "Tanggal booking tidak boleh di masa lalu.");
            }

            bool isBooked = await _context.Bookings.AnyAsync(b =>
                b.RoomId == booking.RoomId &&
                b.BookingDate.Date == booking.BookingDate.Date);

            if (isBooked)
            {
                ModelState.AddModelError("", "Gagal! Ruangan ini sudah dipesan untuk tanggal tersebut.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Pemesanan ruangan berhasil disimpan!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomId"] = new SelectList(_context.Rooms.Where(r => r.IsActive), "RoomId", "RoomName", booking.RoomId);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Booking berhasil dibatalkan/dihapus.";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewData["RoomId"] = new SelectList(_context.Rooms.Where(r => r.IsActive), "RoomId", "RoomName", booking.RoomId);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingId) return NotFound();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            booking.UserId = userId;

            if (booking.BookingDate.Date < DateTime.Today)
            {
                ModelState.AddModelError("BookingDate", "Tanggal booking tidak boleh di masa lalu.");
            }

            bool isBooked = await _context.Bookings.AnyAsync(b =>
                b.RoomId == booking.RoomId &&
                b.BookingDate.Date == booking.BookingDate.Date &&
                b.BookingId != booking.BookingId);

            if (isBooked)
            {
                ModelState.AddModelError("", "Gagal! Ruangan ini sudah dipesan oleh orang lain pada tanggal tersebut.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Data booking berhasil diperbarui!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomId"] = new SelectList(_context.Rooms.Where(r => r.IsActive), "RoomId", "RoomName", booking.RoomId);
            return View(booking);
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}