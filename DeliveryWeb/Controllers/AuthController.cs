using Microsoft.AspNetCore.Mvc;
using FoodDeliveryFrontend.Models;
using System.Text;
using System.Text.Json;

namespace DeliveryWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var response = await _httpClient.PostAsync("api/auth/login", new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid login attempt.";
                return View();
            }

            var tokenResponse = await response.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<Dictionary<string, string>>(tokenResponse);
            HttpContext.Session.SetString("JWToken", tokenData["token"]);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var response = await _httpClient.PostAsync("api/auth/register", new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Registration failed.";
                return View();
            }

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction("Login");
        }
    }
}
