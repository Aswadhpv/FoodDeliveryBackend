namespace FoodDeliveryBackend.DTOs.OrderDtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
