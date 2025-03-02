using HR.BuildingRabbitMQ.Lib;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Smee.IO.Client;

namespace HR.WatchMySmee;

public class WatchMySmee(ILogger<WatchMySmee> logger, RabbitMqPublisher rabbitMqPublisher) : BackgroundService
{
    private readonly ILogger<WatchMySmee> _logger = logger;
    private readonly RabbitMqPublisher _rabbitMqPublisher = rabbitMqPublisher;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var smeeURI = new Uri("https://smee.io/nBO1Vmzr1WPBXkW");
        var smeeClient = new SmeeClient(smeeURI);

        smeeClient.OnConnect += (sender, args) =>
        {
            _logger.LogInformation($"Connected to Smee {smeeURI}");
        };

        smeeClient.OnDisconnect += (sender, args) =>
        {
            _logger.LogInformation($"Disconnected from Smee {smeeURI}");
        };

        smeeClient.OnMessage += async (sender, args) =>
        {
            Console.WriteLine($"Received message from Smee {args}");
            await _rabbitMqPublisher.Publish(args);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(JsonConvert.SerializeObject(args));
            Console.ResetColor();
            _logger.LogInformation($"Received message from Smee {args}");
        };

        smeeClient.OnPing += (sender, args) =>
        {
            _logger.LogInformation($"Ping from Smee {args}");
        };

        smeeClient.OnError += (sender, args) =>
        {
            _logger.LogError($"Error from Smee {args}");
        };

        Console.CancelKeyPress += (sender, args) =>
        {
            _logger.LogInformation("Stopping Smee client");
            smeeClient.Stop();
        };

        await smeeClient.StartAsync(stoppingToken);
    }
}