using FoodDeliveryBackend.Models;

namespace FoodDeliveryBackend.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public int UserId { get; set; }
        public int Stars { get; set; } // 1-5 stars
        public string Review { get; set; }

        public Dish Dish { get; set; }
        public User User { get; set; }
    }
}
