using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryBackend.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public int UserId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "OrderDetails must contain at least one item.")]
        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}
