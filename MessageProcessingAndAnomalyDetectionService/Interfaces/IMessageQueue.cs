using MessageProcessingAndAnomalyDetectionService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingAndAnomalyDetectionService.Interfaces
{
    public interface IMessageQueue
    {
        public void Subscribe(Action<ServerStatistics> messageHandler);
    }
}
