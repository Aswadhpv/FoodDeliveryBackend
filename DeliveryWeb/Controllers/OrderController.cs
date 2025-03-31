using Microsoft.AspNetCore.Mvc;
using FoodDeliveryFrontend.Models;
using System.Text.Json;

namespace DeliveryWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;

        public OrderController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/order");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Unable to load orders.";
                return View(new List<Order>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<Order>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(orders);
        }

        public async Task<IActionResult> Create()
        {
            var response = await _httpClient.PostAsync("api/order", null);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to place order.";
                return RedirectToAction("Index", "Cart");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ConfirmDelivery(int orderId)
        {
            var response = await _httpClient.PostAsync($"api/order/{orderId}/status", null);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to confirm delivery.";
            }

            return RedirectToAction("Index");
        }
    }
}
