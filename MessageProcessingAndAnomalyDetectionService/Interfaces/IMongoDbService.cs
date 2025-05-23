using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageProcessingAndAnomalyDetectionService.Models;

namespace MessageProcessingAndAnomalyDetectionService.Interfaces
{
    public interface IMongoDbService
    {
        Task InsertStatisticsAsync(ServerStatistics statistics);
    }
}
