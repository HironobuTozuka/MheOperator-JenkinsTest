using Common;
using Common.Models;
using Microsoft.Extensions.Logging;
using PlcRequestQueueService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RcsLogic
{
    public class ScanNotificationListenerRegistry : IScanNotificationListenerRegistry
    {
        ILogger<ScanNotificationListenerRegistry> _logger;
        private List<IScanNotificationListener> _scanNotificationListeners = new List<IScanNotificationListener>();

        public ScanNotificationListenerRegistry(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ScanNotificationListenerRegistry>();
        }

        public async Task NotifyListeners(ScanNotificationModel scanNotification)
        {
            _logger.LogDebug("Notifying subscribers about scan notification in PlcCommunicationService on location {0}",
                scanNotification.LocationId);
            _scanNotificationListeners.ForEach(listener => listener.ProcessScanNotification(scanNotification));
        }

        public void RegisterListener(IScanNotificationListener scanNotificationListener)
        {
            _logger.LogInformation("{0} subscribed to scan notifications", scanNotificationListener.GetType());
            _scanNotificationListeners.Add(scanNotificationListener);
        }
    }
}