namespace FoodDeliveryBackend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // Ensure this matches your authentication system
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }  // ✅ Add this property
        public string Status { get; set; }  // Example: Pending, Completed, Cancelled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // ✅ Add this property // Default status

        // Navigation Properties
        public User User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public List<Dish> Dishes { get; set; } = new();
    }

}
