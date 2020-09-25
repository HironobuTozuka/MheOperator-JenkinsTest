using System.Threading.Tasks;
using Common.Models;
using Common.Models.Plc;

namespace PlcRequestQueueService
{
    public interface IPlcNotificationListener
    {
        public Task NotifyListener(PlcNotification plcNotification);
    }
}