using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingAndAnomalyDetectionService.Models
{
    public class ServerStatistics
    {
        public String ServerIdentifier { get; set; }
        public double MemoryUsage { get; set; }
        public double AvailableMemory { get; set; } 
        public double CpuUsage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
