using System.ComponentModel.DataAnnotations;

namespace RoomBookingApp.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        [Required(ErrorMessage = "Nama ruangan wajib diisi")]
        public string RoomName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Kapasitas wajib diisi")]
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}