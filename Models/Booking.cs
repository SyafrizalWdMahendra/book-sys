using System.ComponentModel.DataAnnotations;

namespace RoomBookingApp.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Pilih ruangan terlebih dahulu")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Tanggal booking wajib diisi")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User? User { get; set; }
        public Room? Room { get; set; }
    }
}