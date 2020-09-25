using System.Threading.Tasks;
using Common.Models.Plc;
using Microsoft.Extensions.Logging;
using PlcRequestQueueService;
using RcsLogic.Crane;
using RcsLogic.Models;
using RcsLogic.Models.Device;

namespace RcsLogic.RcsController
{
    public class CraneIdleListener : IPlcNotificationListener
    {
        private readonly ILogger<CraneIdleListener> _logger;
        private readonly RcsController _rcsController;
        private readonly DeviceRegistry _deviceRegistry;

        public CraneIdleListener(ILoggerFactory loggerFactory, RcsController rcsController, DeviceRegistry deviceRegistry)
        {
            _logger = loggerFactory.CreateLogger<CraneIdleListener>();
            _rcsController = rcsController;
            _deviceRegistry = deviceRegistry;
        }

        public async Task NotifyListener(PlcNotification plcNotification)
        {
            if (_deviceRegistry.GetDeviceByDeviceId(new DeviceId(plcNotification.Parameters.CraneId)) is CraneDevice crane)
            {
                var timedOutRequests = crane.GetTimedOutRequests();
                if(timedOutRequests.Count>0)_logger.LogError("{DeviceId} Requests timed out: {requests}", crane.DeviceId, string.Join(",", timedOutRequests));
                timedOutRequests.ForEach(request => _rcsController.HandleRequestTimeout( 
                    new TimedOutTransfer()
                    { 
                        device = crane,
                        transfer = request 
                    }));
                crane.CraneIdle();
            }
        }
    }
}