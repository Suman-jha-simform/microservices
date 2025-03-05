using Consumer.Service.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Consumer.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController(IOrderService _orderService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }
    }
}
