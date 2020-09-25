using System.Collections.Generic;
using RcsLogic.Models.Device;

namespace RcsLogic
{
    public interface IDeviceInitializer
    {
        public List<IDevice> GetDevices();
    }
}