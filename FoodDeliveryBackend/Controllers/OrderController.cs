using Microsoft.AspNetCore.Mvc;
using FoodDeliveryBackend.DTOs.OrderDtos;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FoodDeliveryBackend.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Get a specific order by ID.
        /// </summary>
        [HttpGet("{orderId}")]
        [Authorize]
        [ProducesResponseType(typeof(OrderDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return Unauthorized();

                var order = await _orderService.GetOrderByIdAsync(userId, orderId);
                if (order == null) return NotFound();

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Get a list of all user orders.
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(List<OrderDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return Unauthorized();

                var orders = await _orderService.GetOrdersAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Create a new order from cart.
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(OrderDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateOrder()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return Unauthorized();

                var order = await _orderService.CreateOrderAsync(userId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "Error", Message = ex.Message });
            }
        }

        /// <summary>
        /// Confirm order delivery.
        /// </summary>
        [HttpPost("{orderId}/status")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ConfirmOrderDelivery(int orderId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return Unauthorized();

                var result = await _orderService.ConfirmOrderDeliveryAsync(userId, orderId);
                return result ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "Error", Message = ex.Message });
            }
        }
    }
}
