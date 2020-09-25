using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models.Gate;
using Microsoft.Extensions.Logging;
using Workstation.ServiceModel.Ua;
using Workstation.ServiceModel.Ua.Channels;

namespace PlcCommunicationService.SystemPlc.Models
{
    public class ExternalIO : SignalWriter
    {
        public async Task Led1(bool value)
        {
            await WriteVar(GetNodeId("Led1"), value);
        }

        public async Task Led2(bool value)
        {
            await WriteVar(GetNodeId("Led2"), value);
        }

        public async Task Led3(bool value)
        {
            await WriteVar(GetNodeId("Led3"), value);
        }

        public async Task OpenGate(GateId gateId, bool value, int? slot = null)
        {
            if (slot == null)
            {
                await WriteVar(GetNodeId($"Open{gateId}"), value);
            }
            else
            {
                await WriteVar(GetNodeId($"Open{gateId}_{slot+1}"), value);
            }
        }

        public ExternalIO(UaTcpSessionChannel channel, ILoggerFactory loggerFactory) : base(channel,
            loggerFactory.CreateLogger<SignalWriter>())
        {
        }

        private string GetNodeId(string signalName)
        {
            return $"{Declarations.NodePaths.ExternalIO}.{signalName}";
        }
    }
}