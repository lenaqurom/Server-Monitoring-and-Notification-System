using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerStatisticsCollectionService.Models;
using ServerStatisticsCollectionService.Interfaces;
using ServerStatisticsCollectionService.Services;

namespace ServerStatisticsCollectionService
{
    public class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var rabbitMqHost = context.Configuration.GetValue<string>("RabbitMQConfig:HostName");
                    var rabbitMqPort = context.Configuration.GetValue<int>("RabbitMQConfig:Port");
                    var rabbitMqUser = context.Configuration.GetValue<string>("RabbitMQConfig:UserName");
                    var rabbitMqPassword = context.Configuration.GetValue<string>("RabbitMQConfig:Password");

                    if (string.IsNullOrEmpty(rabbitMqHost) || string.IsNullOrEmpty(rabbitMqUser) || string.IsNullOrEmpty(rabbitMqPassword))
                    {
                        throw new ArgumentNullException("One of the RabbitMQ configuration values is null or empty.");
                    }
                    services.AddSingleton<IMessageQueue>(prvider => new RabbitMqMessageQueue(rabbitMqHost, rabbitMqPort, rabbitMqUser, rabbitMqPassword));
                    services.AddHostedService<StatisticsCollectorService>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .Build();
            host.Run();
        }
    }
}
