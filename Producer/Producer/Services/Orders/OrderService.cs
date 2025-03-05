using Microsoft.EntityFrameworkCore;
using Producer.Database;
using Producer.Entities;
using Producer.Services.Orders;
using Producer.Services.RabbitMQ;

namespace Producer.Services.Entities;

public class OrderService(ApplicationDbContext _context, IRabbitMqService _rabbitMqService) : IOrderService
{
    public async Task<Order> CreateOrderAsync(Order order)
    {
        _context.ProducerOrders.Add(order);
        await _context.SaveChangesAsync();

        await _rabbitMqService.PublishOrderAsync(order);

        return order;
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _context.ProducerOrders.ToListAsync();
    }

}
