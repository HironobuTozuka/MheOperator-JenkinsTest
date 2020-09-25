using System;
using System.Collections.Generic;
using System.Text;
using Workstation.ServiceModel.Ua;

namespace PlcCommunicationService.Models
{
    public class MonitoredItemDefinition
    {
        public string propertyName { get; set; }
        public NodeId nodeId { get; set; }
        public MonitoringMode monitoringMode { get; set; } = MonitoringMode.Reporting;
        public uint clientHandle { get; set; }
        public uint queueSize { get; set; } = 2;
        public bool discardOldest { get; set; } = true;
        public double samplingInterval { get; set; } = 50.0;

    }
}
