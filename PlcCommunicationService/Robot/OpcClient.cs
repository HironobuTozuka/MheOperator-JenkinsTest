using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlcCommunicationService.Models;
using Workstation.ServiceModel.Ua;

namespace PlcCommunicationService.Robot
{
    public class OpcClient : PlcCommunicationService.OpcClient
    {
        private static readonly Type Type = typeof(Declarations);
        private IConfiguration _configuration;

        public OpcClient(ILoggerFactory loggerFactory, IConfiguration configuration) :
            base(loggerFactory, loggerFactory.CreateLogger<PlcConnection>(), Type, CreateMonitoredItems(),
                configuration)
        {
            _configuration = configuration;
        }

        protected override IConfigurationSection GetClientConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection("PlcRobotConnectionSettings");
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
                }
            };
        }

        protected override void CreateIO()
        {
            OutSignals = new OutSignals(Channel, LoggerFactory);
            InSignals = new InSignals(Channel, LoggerFactory);

            OutSignals.SetMoveRequestConfRead(false).Wait();
            OutSignals.SetReadMoveRequest(false).Wait();
        }


        protected override string ClientId()
        {
            return "Robot";
        }
    }
}