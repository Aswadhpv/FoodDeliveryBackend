namespace FoodDeliveryFrontend.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
