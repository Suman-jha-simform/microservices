using Consumer.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using Consumer.Service.Orders;

namespace Consumer.Service.RabbitMQ;

public class RabbitMqService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ConnectionFactory _factory;
    private IChannel channel;
    private IConnection connection;

    public RabbitMqService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            Port = 5672
        };

    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        
        // Establish connection and channel
          connection = await _factory.CreateConnectionAsync();
          channel = await connection.CreateChannelAsync();

        // Declare queue
        await channel.QueueDeclareAsync(queue: "orderQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var order = JsonConvert.DeserializeObject<Order>(message);
                Console.WriteLine($"Processing order: {order}");

                using var scope = _scopeFactory.CreateScope();
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                await orderService.SaveOrderAsync(order);

                await channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        };

        await channel.BasicConsumeAsync(queue: "orderQueue", autoAck: false, consumer: consumer);

        // 🔹 Keep service alive - this is needed!
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        // Clean up resources on service stop
        channel?.CloseAsync();
        connection?.CloseAsync();
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        channel?.Dispose();
        connection?.Dispose();
        base.Dispose();
    }
}
