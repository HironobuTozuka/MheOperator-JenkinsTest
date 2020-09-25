using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Logging;
using PlcCommunicationService.Models;
using Workstation.ServiceModel.Ua;
using Timer = System.Timers.Timer;

namespace PlcCommunicationService
{
    public class MoveRequestSender : IMoveRequestRedStateChangeListener
    {
        private readonly ILogger<MoveRequestSender> _logger;
        private bool _moveRequestRead = false;
        private readonly OpcClient _opcClient;

        public MoveRequestSender(ILoggerFactory loggerFactory, OpcClient opcClient)
        {
            _opcClient = opcClient;
            _logger = loggerFactory.CreateLogger<MoveRequestSender>();
        }

        private async Task<bool> MoveRequestSent(MoveRequest request)
        {
            try
            {
                //Plc hasn't yet read the request or is not connected
                if (_opcClient?.OutSignals == null || !_opcClient.AllCommunicationOK ||
                    _opcClient.OutSignals.GetReadMoveRequest())
                {
                    await Task.Delay(20);
                    return false;
                }

                //Plc is not ready to accept move requests - PLC not running
                if (IsSystemPlc() && !IsPlcAcceptingRequests())
                {
                    await Task.Delay(200);
                    return false;
                }

                //Skip if MoveRequestRead is not 0;
                var actualMoveRequestRead = _opcClient.InSignals.GetMoveRequestRead();
                if (_moveRequestRead || actualMoveRequestRead)
                {
                    await Task.Delay(20);
                    return false;
                }

                _logger.LogInformation("{1}: Begin to send first MoveRequest request from queue to PLC in kafka {2}",
                    GetPlcConnectionId(), request);
                await _opcClient.OutSignals.SetMoveRequest(request);
                _logger.LogInformation("{0}: MoveRequest request sent from queue to PLC in kafka: {1}",
                    GetPlcConnectionId(), request.ToString());
                await _opcClient.OutSignals.SetReadMoveRequest(true);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception while sending move request from queue in {GetPlcConnectionId()}");
                Thread.Sleep(500);
                return false;
            }
        }

        private bool IsPlcAcceptingRequests()
        {
            var packMlState = ((SystemPlc.InSignals) _opcClient.InSignals).Status.PackMLState();
            var packMlMode = ((SystemPlc.InSignals) _opcClient.InSignals).Status.PackMLMode();
            packMlState.Wait();
            packMlMode.Wait();
            return packMlState.Result == 6 && packMlMode.Result == 1;
        }

        private string GetPlcConnectionId()
        {
            if (IsSystemPlc()) return "SystemPlc";
            return "RobotPlc";
        }

        private bool IsSystemPlc()
        {
            return _opcClient.GetType() == typeof(SystemPlc.OpcClient);
        }

        public async Task Send(MoveRequest moveRequest)
        {
            while (await MoveRequestSent(moveRequest) == false)
            {
            }
        }


        public void NotifyListener(bool moveRequestRed)
        {
            _moveRequestRead = moveRequestRed;
        }
    }
}