using Producer.Entities;
namespace Producer.Services.RabbitMQ;

public interface IRabbitMqService
{
    Task PublishOrderAsync(Order order);
}
