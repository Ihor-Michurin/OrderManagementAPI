using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace OrderManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderStore _orderStore;

        public OrdersController(OrderStore orderStore)
        {
            _orderStore = orderStore;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(Order order)
        {
            await _orderStore.AddOrderAsync(order);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var orders = await _orderStore.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderAsync(Guid orderId)
        {
            var order = await _orderStore.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateOrderStatusAsync(Guid orderId, [FromBody] string descriptionUpdate)
        {
            var updatedOrder = await _orderStore.UpdateOrderStatusAsync(orderId, descriptionUpdate);

            if (updatedOrder == null)
            {
                return NotFound();
            }

            return Ok(updatedOrder);
        }
    }
}
