using MessageProcessingAndAnomalyDetectionService.Interfaces;
using MessageProcessingAndAnomalyDetectionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MessageProcessingAndAnomalyDetectionService.Services
{
    public class MongoDbService : IMongoDbService
    {
        private readonly IMongoCollection<ServerStatistics> _collection;

        public MongoDbService(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<ServerStatistics>(collectionName);
        }

        public async Task InsertStatisticsAsync(ServerStatistics statistics)
        {
            await _collection.InsertOneAsync(statistics);
        }
    }
}
