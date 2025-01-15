using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryBackend.DTOs
{
    public class UpdateUserProfileDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name must be at most 100 characters.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
