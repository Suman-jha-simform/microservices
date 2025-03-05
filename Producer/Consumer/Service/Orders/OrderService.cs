using Consumer.Database;
using Consumer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Consumer.Service.Orders;

public class OrderService(ApplicationDbContext _context) : IOrderService
{
    public async Task SaveOrderAsync(Order order)
    {
        order.Id = 0;
        _context.ConsumerOrders.Add(order);
        await _context.SaveChangesAsync();
    }


    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _context.ConsumerOrders.ToListAsync();
    }
}
