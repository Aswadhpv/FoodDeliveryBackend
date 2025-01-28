using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryBackend.DTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } // Change 'Name' to 'FullName'

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
    }
}
