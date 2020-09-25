using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Workstation.ServiceModel.Ua;
using Workstation.ServiceModel.Ua.Channels;

namespace PlcCommunicationService.SystemPlc.Models
{
    public class Status : SignalReader
    {
        public async Task<int> FirstFaultNo()
        {
            return (await ReadVariable($"{Declarations.NodePaths.Status}.FirstFaultNo")).GetValueOrDefault<ushort>();
        }
        public async Task<int> PackMLMode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.Status}.PackMLMode")).GetValueOrDefault<ushort>();
        }
        public async Task<int> PackMLState()
        {
            return (await ReadVariable($"{Declarations.NodePaths.Status}.PackMLState")).GetValueOrDefault<ushort>();
        }
        public async Task<bool> LoadingGateOpen()
        {
            return (await ReadVariable($"{Declarations.NodePaths.Status}.LoadingGateOpen")).GetValueOrDefault<bool>();
        }
        public async Task<bool> OrderGate1_1Open()
        {
            return (await ReadVariable($"{Declarations.NodePaths.Status}.OrderGate1_1Open")).GetValueOrDefault<bool>();
        }
        public async Task<bool> OrderGate1_2Open()
        {
            return (await ReadVariable($"{Declarations.NodePaths.Status}.OrderGate1_2Open")).GetValueOrDefault<bool>();
        }
        public async Task<bool> OrderGate2_1Open()
        {
            return (await ReadVariable($"{Declarations.NodePaths.Status}.OrderGate2_1Open")).GetValueOrDefault<bool>();
        }
        public async Task<bool> OrderGate2_2Open()
        {
            return (await ReadVariable($"{Declarations.NodePaths.Status}.OrderGate2_2Open")).GetValueOrDefault<bool>();
        }


        public Status(ILoggerFactory loggerFactory, UaTcpSessionChannel channel) : base(channel, loggerFactory.CreateLogger<Status>())
        {
        }
        
    }
}
