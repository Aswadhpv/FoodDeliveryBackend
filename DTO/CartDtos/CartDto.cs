namespace FoodDeliveryBackend.DTOs.CartDtos
{
    public class CartDto
    {
        public int DishId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
}
