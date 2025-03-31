using Microsoft.AspNetCore.Mvc;
using FoodDeliveryFrontend.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DeliveryWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;

        public CartController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/cart");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Unable to load cart items.";
                return View(new List<CartItem>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var cartItems = JsonSerializer.Deserialize<List<CartItem>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(cartItems);
        }

        public async Task<IActionResult> Remove(int dishId)
        {
            var response = await _httpClient.DeleteAsync($"api/cart/dish/{dishId}");

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Failed to remove item.";

            return RedirectToAction("Index");
        }
    }
}
