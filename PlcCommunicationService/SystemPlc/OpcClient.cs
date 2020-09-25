using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlcCommunicationService.Models;
using Workstation.ServiceModel.Ua;

namespace PlcCommunicationService.SystemPlc
{
    public class OpcClient : PlcCommunicationService.OpcClient
    {
        private static readonly Type Type = typeof(Declarations);

        public OpcClient(ILoggerFactory loggerFactory, IConfiguration configuration) :
            base(loggerFactory, loggerFactory.CreateLogger<PlcConnection>(), Type, CreateMonitoredItems(),
                configuration)
        {
        }

        protected override IConfigurationSection GetClientConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection("PlcSystemConnectionSettings");
        }

        private static List<MonitoredItemDefinition> CreateMonitoredItems()
        {
            return new List<MonitoredItemDefinition>()
            {
                new MonitoredItemDefinition
                {
                    propertyName = "ReadMoveRequestConf",
                    nodeId = NodeId.Parse(Declarations.NodePaths.ReadMoveRequestConf),
                    clientHandle = 1,
                    samplingInterval = 10
                },
                new MonitoredItemDefinition
                {
                    propertyName = "MoveRequestRead",
                    nodeId = NodeId.Parse(Declarations.NodePaths.MoveRequestRead),
                    clientHandle = 2,
                    samplingInterval = 10
                },
                new MonitoredItemDefinition
                {
                    propertyName = "ReadScanNotification",
                    nodeId = NodeId.Parse(Declarations.NodePaths.ReadScanNotification),
                    clientHandle = 3,
                    samplingInterval = 10
                },
                new MonitoredItemDefinition
                {
                    propertyName = "CA_P_Idle",
                    nodeId = NodeId.Parse(Declarations.NodePaths.CA_P_Idle),
                    clientHandle = 4,
                    samplingInterval = 10
                },
                new MonitoredItemDefinition
                {
                    propertyName = "CB_P_Idle",
                    nodeId = NodeId.Parse(Declarations.NodePaths.CB_P_Idle),
                    clientHandle = 5,
                    samplingInterval = 10
                },
            };
        }

        protected override void CreateIO()
        {
            OutSignals = new OutSignals(Channel, LoggerFactory);
            InSignals = new InSignals(Channel, LoggerFactory);

            OutSignals.SetMoveRequestConfRead(false).Wait();
            OutSignals.SetReadMoveRequest(false).Wait();
            ((OutSignals) OutSignals).SetScanNotificationRead(false).Wait();
        }

        protected override string ClientId()
        {
            return "System";
        }
    }
}