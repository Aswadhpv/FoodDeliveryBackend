using Microsoft.AspNetCore.Identity;

namespace FoodDeliveryBackend.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Role { get; set; } = "User"; // Default role: User or Admin

        // Relationships
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }

}
