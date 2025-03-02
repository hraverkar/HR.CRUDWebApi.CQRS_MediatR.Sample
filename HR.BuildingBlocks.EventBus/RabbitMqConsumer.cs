using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace HR.BuildingRabbitMQ.Lib
{
    public class RabbitMqConsumer
    {
        private readonly ILogger<RabbitMqConsumer> _logger;
        private readonly RabbitMqSettings _settings;
        private readonly ConnectionFactory? factory;
        private readonly ConcurrentQueue<string> _messages = new ConcurrentQueue<string>();

        public RabbitMqConsumer(RabbitMqSettings settings, ILogger<RabbitMqConsumer> logger)
        {
            _settings = settings;
            _logger = logger;
            factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password
            };
        }
        public async Task StartConsuming(CancellationToken cancellationToken)
        {
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: _settings.QueueName, durable: true,
                                          exclusive: false,
                                          autoDelete: false,
                                          arguments: null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($" [✔] Message received: {message}");
                _messages.Enqueue(message);
                await channel.BasicAckAsync(ea.DeliveryTag, false);
            };
            await channel.BasicConsumeAsync(queue: _settings.QueueName, autoAck: true, consumer: consumer);
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(500, cancellationToken); // Prevents busy waiting
            }
        }
        public bool TryDequeue(out string message)
        {
            return _messages.TryDequeue(out message);
        }
    }
}
