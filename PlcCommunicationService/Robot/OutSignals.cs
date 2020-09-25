using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Workstation.ServiceModel.Ua;
using System.ComponentModel;
using Workstation.ServiceModel.Ua.Channels;
using Microsoft.Extensions.Logging;
using PlcCommunicationService.Models;

namespace PlcCommunicationService.Robot
{
    public class OutSignals : SignalWriter, IOutSignals
    {
        public OutSignals(UaTcpSessionChannel channel, ILoggerFactory loggerFactory) : base(channel,
            loggerFactory.CreateLogger<SignalWriter>())
        {
            SignalReader reader = new SignalReader(channel, loggerFactory.CreateLogger<SignalReader>());
            InitSignals(reader);
        }

        /// <summary>
        /// Sets the value of MoveRequest -> description about which parts to pick.
        /// </summary>
        public async Task SetMoveRequest(IMoveRequest value)
        {
            await WriteVar(Declarations.NodePaths.MoveRequest, value);
        }

        /// <summary>
        /// Sets the value of ReadMoveRequest -> trigger to read MoveRequest.
        /// </summary>
        public async Task SetReadMoveRequest(bool value)
        {
            await WriteVar(Declarations.NodePaths.ReadMoveRequest, value);
            _readMoveRequest = value;
        }

        public bool GetReadMoveRequest()
        {
            return _readMoveRequest;
        }

        private bool _readMoveRequest;

        /// <summary>
        /// Sets the value of HeartBeat -> confirmation, for PLC, that the system is alive.
        /// </summary>
        public async Task SetHeartBeat(byte value)
        {
            await WriteVar(Declarations.NodePaths.HeartBeat, value);
        }

        /// <summary>
        /// Sets the value of MoveRequestConfRead -> confirmation, that the move request conf was read.
        /// </summary>
        public async Task SetMoveRequestConfRead(bool value)
        {
            await WriteVar(Declarations.NodePaths.MoveRequestConfRead, value);
        }

        async void InitSignals(SignalReader reader)
        {
            try
            {
                _readMoveRequest = (await reader.ReadVariable(Declarations.NodePaths.ReadMoveRequest))
                    .GetValueOrDefault<bool>();
            }
            catch
            {
                _logger.LogError("Unable to read signal values while initialising the Out signals for System PLC");
            }
        }
    }
}