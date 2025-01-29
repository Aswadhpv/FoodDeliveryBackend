namespace FoodDeliveryBackend.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsVegetarian { get; set; }

        // ⭐ Rating System
        public int TotalRating { get; set; }
        public int RatingsCount { get; set; }
        public double AverageRating => RatingsCount == 0 ? 0 : (double)TotalRating / RatingsCount;

        // Category (Use string for flexibility OR enum for strict types)
        public DishCategory Category { get; set; }

    }

    public enum DishCategory
    {
        Wok,
        Pizza,
        Soup,
        Dessert,
        Drink
    }

}
