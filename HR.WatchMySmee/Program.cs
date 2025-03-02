using HR.BuildingRabbitMQ.Lib;
using HR.WatchMySmee;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var settings = new RabbitMqSettings();
        services.AddSingleton(settings);
        services.AddSingleton<RabbitMqConsumer>();
        services.AddHostedService<WatchMySmee>();
        services.AddSingleton<RabbitMqPublisher>();
    }).Build();
host.Run();