namespace FoodDeliveryBackend.DTOs
{
    public class CreateDishDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public bool? IsVegetarian { get; set; }
    }
}
