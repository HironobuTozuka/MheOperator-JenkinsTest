using System.Collections.Generic;
using Common.Models.Task;
using RcsLogic.Models;
using RcsLogic.Models.Device;

namespace RcsLogic
{
    public interface ITransferPlanningDevice : IDevice
    {
        public void RemovePlannedTransfers(TaskBase task);
        
    }
}