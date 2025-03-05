using Consumer.Entities;

namespace Consumer.Service.Orders;

public interface IOrderService
{
    Task SaveOrderAsync(Order order);

    Task<List<Order>> GetOrdersAsync();
}
