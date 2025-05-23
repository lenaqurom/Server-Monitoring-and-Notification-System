using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MessageProcessingAndAnomalyDetectionService.Models;
using MessageProcessingAndAnomalyDetectionService.Interfaces;
using MessageProcessingAndAnomalyDetectionService.Services;

namespace MessageProcessingAndAnomalyDetectionService
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;

                    services.AddSingleton<IMessageQueue, RabbitMqMessageQueue>(provider =>
                    {
                        var hostName = configuration.GetValue<string>("RabbitMQConfig:HostName");
                        var port = configuration.GetValue<int>("RabbitMQConfig:Port");
                        var userName = configuration.GetValue<string>("RabbitMQConfig:UserName");
                        var password = configuration.GetValue<string>("RabbitMQConfig:Password");
                        return new RabbitMqMessageQueue(hostName, port, userName, password);
                    });
                    services.AddSingleton<IMongoDbService, MongoDbService>(provider =>
                    {
                        var connectionString = configuration.GetValue<string>("MongoDbConfig:ConnectionString");
                        var databaseName = configuration.GetValue<string>("MongoDbConfig:DatabaseName");
                        var collectionName = configuration.GetValue<string>("MongoDbConfig:CollectionName");
                        return new MongoDbService(connectionString, databaseName, collectionName);
                    });
                })
                    .Build();

            var messageQueue = host.Services.GetRequiredService<IMessageQueue>();
            var mongoDbService = host.Services.GetRequiredService<IMongoDbService>();

            messageQueue.Subscribe(stats => HandleServerStatistics(stats, mongoDbService));

            Console.WriteLine("Subscribed to RabbitMQ messages. Press Enter to exit.");
            Console.ReadLine();

            if (messageQueue is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
        private static void HandleServerStatistics(ServerStatistics stats, IMongoDbService mongoDbService)
        {
            Console.WriteLine("Received Server Statistics:");
            Console.WriteLine($"  Server ID      : {stats.ServerIdentifier}");
            Console.WriteLine($"  CPU Usage      : {stats.CpuUsage}%");
            Console.WriteLine($"  Memory Usage   : {stats.MemoryUsage} MB");
            Console.WriteLine($"  Available Mem  : {stats.AvailableMemory} MB");
            Console.WriteLine($"  Timestamp      : {stats.Timestamp}");
            Console.WriteLine(new string('-', 40));

            mongoDbService.InsertStatisticsAsync(stats).Wait();
        }
    }
}

