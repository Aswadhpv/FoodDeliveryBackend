namespace FoodDeliveryBackend.DTOs
{
    public class RatingDto
    {
        public int DishId { get; set; }
        public int Stars { get; set; } // 1-5
        public string Review { get; set; }
    }
}
