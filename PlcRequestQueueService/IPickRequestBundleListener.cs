using System.Threading.Tasks;
using Common.Models;

namespace PlcRequestQueueService
{
    public interface IPickRequestBundleListener
    {
        public Task NotifyListener(RobotPickRequestBundleModel pickRequestBundle);
    }
}