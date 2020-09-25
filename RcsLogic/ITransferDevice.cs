using System.Collections.Generic;
using RcsLogic.Models;
using RcsLogic.Models.Device;

namespace RcsLogic
{
    public interface ITransferDevice : IDevice
    {
        public void Execute(Transfer request);

        void Execute(List<Transfer> transfers);
    }
}