namespace FoodDeliveryBackend.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int DishId { get; set; }
        public int Quantity { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Dish Dish { get; set; }
    }
}
