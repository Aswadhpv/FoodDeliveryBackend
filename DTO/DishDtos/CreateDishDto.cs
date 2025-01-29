using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryBackend.DTOs
{
    public class CreateDishDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public bool IsVegetarian { get; set; }
        public string Category { get; set; }
    }
}
