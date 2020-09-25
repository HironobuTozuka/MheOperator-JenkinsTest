using Common.Models;

namespace Common
{
    public interface IScanNotificationListener
    {
        public void ProcessScanNotification(ScanNotificationModel scanNotification);
    }
}
