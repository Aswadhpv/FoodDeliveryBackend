using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryBackend.DTOs
{
    public class OrderDetailDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "DishId must be greater than 0.")]
        public int DishId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
    }
}
