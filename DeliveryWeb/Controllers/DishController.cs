using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using FoodDeliveryFrontend.Models;

namespace DeliveryWeb.Controllers
{
    public class DishController : Controller
    {
        private readonly HttpClient _httpClient;

        public DishController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/dish");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not retrieve dishes.";
                return View(new List<Dish>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var dishes = JsonSerializer.Deserialize<List<Dish>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(dishes);
        }
    }
}
