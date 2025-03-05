using Microsoft.AspNetCore.Mvc;
using Producer.Entities;
using Producer.Services.Orders;

namespace Producer.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController(IOrderService _orderService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order == null)
                return BadRequest("Invalid order data.");

            var createdOrder = await _orderService.CreateOrderAsync(order);
            return Ok(createdOrder);
        }


        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }
    }
}
