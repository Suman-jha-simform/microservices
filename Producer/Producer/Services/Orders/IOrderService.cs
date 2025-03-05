using Producer.Entities;
namespace Producer.Services.Orders;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(Order order);

    Task<List<Order>> GetOrdersAsync();
}
