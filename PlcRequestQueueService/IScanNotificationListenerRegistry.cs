using System.Threading.Tasks;
using Common.Models;

namespace PlcRequestQueueService
{
    public interface IScanNotificationListenerRegistry
    {
        public Task NotifyListeners(ScanNotificationModel scanNotification);
    }
}