namespace FoodDeliveryBackend.DTOs
{
    public class AddToCartDto
    {
        public int UserId { get; set; }
        public int DishId { get; set; }
        public int Quantity { get; set; }
    }
}
