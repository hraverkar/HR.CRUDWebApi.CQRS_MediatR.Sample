
using HR.BuildingRabbitMQ.Lib;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Events
{
    public class RabbitMqConsumerAudit(RabbitMqConsumer rabbitMqConsumer, ILogger<RabbitMqConsumerAudit> logger) : BackgroundService
    {
        private readonly ILogger<RabbitMqConsumerAudit> _logger = logger;
        private readonly RabbitMqConsumer _consumer = rabbitMqConsumer;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting RabbitMQ consumer");
            try
            {
                await _consumer.StartConsuming(stoppingToken);
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (_consumer.TryDequeue(out string message))
                    {
                        _logger.LogInformation($"Received message: {message}");
                        // process the message
                    }
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming RabbitMQ messages");
            }
        }
    }
}
