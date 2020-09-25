using System;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Logging;
using Workstation.ServiceModel.Ua;

namespace PlcCommunicationService
{
    public class Heartbeat
    {
        private byte _heartbeatValue;
        private readonly Timer _heartBeatTimer;
        private readonly OpcClient _opcClient;
        private const int HeartbeatTimespan = 1000;
        private readonly ILogger<Heartbeat> _logger;

        public Heartbeat(ILoggerFactory loggerFactory, OpcClient opcClient)
        {
            _opcClient = opcClient;
            _logger = loggerFactory.CreateLogger<Heartbeat>();
            _heartBeatTimer = new Timer(HeartbeatTimespan) {AutoReset = false};
            _StartHeartbeat();
        }

        private async void _heartBeatTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!_opcClient.AllCommunicationOK)
            {
                _logger.LogWarning("Communication not ok in heartbeat");
                return;
            }

            try
            {
                _heartbeatValue += 1;
            }
            catch
            {
                _heartbeatValue = 0;
            }

            try
            {
                var startTime = DateTime.Now;
                await _opcClient.OutSignals.SetHeartBeat(_heartbeatValue);
                var writeTime = DateTime.Now - startTime;
                if (writeTime > TimeSpan.FromMilliseconds(100))
                    _logger.LogWarning("Long heartbeat write time {0:g} !!!!", writeTime);
                ResetHeartbeatTimer();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to set heartbeat!!!!!!!!!!!! in {0}", _opcClient.GetType());
                _opcClient.ChannelFaulted();
            }
        }


        private void _StartHeartbeat()
        {
            _heartBeatTimer.Elapsed += _heartBeatTimer_Elapsed;
            _heartBeatTimer.Enabled = true;
            ResetHeartbeatTimer();
            _heartBeatTimer.Start();
        }


        public void ResetHeartbeatTimer()
        {
            _heartBeatTimer.Interval = HeartbeatTimespan;
        }
    }
}