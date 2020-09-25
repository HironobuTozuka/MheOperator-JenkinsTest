using System.Threading.Tasks;
using Common.Models;
using Common.Models.Plc;

namespace PlcCommunicationService.Models
{
    public interface IPlcReadNotificationListener
    {
        public Task NotifyListener(PlcReadNotification plcAction, OpcClient opcClient);
    }
}
