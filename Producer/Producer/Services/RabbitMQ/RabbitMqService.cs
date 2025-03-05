using Producer.Entities;
using RabbitMQ.Client;
using Newtonsoft.Json; // Import Newtonsoft.Json
using System.Text;
using System.Threading.Tasks; // For async/await

namespace Producer.Services.RabbitMQ;

public class RabbitMqService : IRabbitMqService
{
    private readonly ConnectionFactory _factory;

    public RabbitMqService()
    {
        _factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            Port = 5672
        };
    }

    public async Task PublishOrderAsync(Order order) { 
        // Await the asynchronous connection creation
        using var connection = await _factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "orderQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var orderJson = JsonConvert.SerializeObject(order); // Serialize the order object
        var body = Encoding.UTF8.GetBytes(orderJson);

        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "orderQueue", body: body);
        
    }
}
