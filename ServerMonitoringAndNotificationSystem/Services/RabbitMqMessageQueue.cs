using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using ServerStatisticsCollectionService.Models;
using ServerStatisticsCollectionService.Interfaces;
using System.Text.Json;


namespace ServerStatisticsCollectionService.Services
{
    public class RabbitMqMessageQueue : IMessageQueue, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public RabbitMqMessageQueue(string hostName, int port, string userName, string password)
        {
            var factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password
            };
            factory.ClientProvidedName = "Server StatisticsSent Collector Service";

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public async Task PublishAsync(string topic, ServerStatistics statistics)
        {
            _channel.ExchangeDeclare("ServerStatisticsExchange", ExchangeType.Topic);
            _channel.QueueDeclare("Queue", durable: false, exclusive: false, autoDelete: false);
            _channel.QueueBind("Queue", "ServerStatisticsExchange", topic);

            string jsonMessage = JsonSerializer.Serialize(statistics);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            await Task.Run(() => _channel.BasicPublish(
                exchange: "ServerStatisticsExchange",
                routingKey: topic,
                basicProperties: null,
                body: body
            ));
        }
        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
        public Task Publish(string topic, ServerStatistics statistics)
        {
            throw new NotImplementedException();
        }
    }
}
