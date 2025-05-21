using ServerStatisticsCollectionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerStatisticsCollectionService.Interfaces
{
    public interface IMessageQueue
    {
        public Task PublishAsync(string topic, ServerStatistics statistics);
    }
}
