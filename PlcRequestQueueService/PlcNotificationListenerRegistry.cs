using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Models;
using Common.Models.Plc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PlcRequestQueueService
{
    public class PlcNotificationListenerRegistry
    {
        private readonly Dictionary<IPlcNotificationListener, PlcNotificationType> _subscribedListeners;
        private readonly ILogger<PlcNotificationListenerRegistry> _logger;

        public PlcNotificationListenerRegistry(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _subscribedListeners = new Dictionary<IPlcNotificationListener, PlcNotificationType>();
            _logger = loggerFactory.CreateLogger<PlcNotificationListenerRegistry>();
        }

        public async Task NotifyListeners(PlcNotification plcNotification)
        {
            _logger.LogDebug("Notifying subscribers plc notification {0}", plcNotification);
            foreach (var listener in _subscribedListeners.Where(listener => listener.Value == plcNotification.NotificationType))
            {
                await listener.Key.NotifyListener(plcNotification);
            }
        }

        public void RegisterListener(IPlcNotificationListener plcNotificationListener, PlcNotificationType notificationType)
        {
            _logger.LogTrace("{0} subscribed to pick requests", plcNotificationListener.GetType());
            _subscribedListeners.Add(plcNotificationListener, notificationType);
        }
    }
}