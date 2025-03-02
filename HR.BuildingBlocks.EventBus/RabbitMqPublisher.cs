using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace HR.BuildingRabbitMQ.Lib
{
    public class RabbitMqPublisher
    {
        private readonly RabbitMqSettings _settings;
        private readonly ConnectionFactory? factory;
        public RabbitMqPublisher(RabbitMqSettings settings)
        {
            _settings = settings;
            factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password
            };
        }

        public async Task Publish<T>(T message)
        {
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: _settings.QueueName, durable: true,
                                          exclusive: false,
                                          autoDelete: false,
                                          arguments: null);

            string jsonMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            await channel.BasicPublishAsync(exchange: "",
                                      routingKey: _settings.QueueName,
                                      body: body);
        }
    }
}
