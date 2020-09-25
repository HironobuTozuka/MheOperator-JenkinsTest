using RcsLogic.Services;

namespace RcsLogic.Models.Device
{

    public abstract class Device : IDevice
    {
        protected readonly DeviceId _deviceId;
        protected readonly TaskBundleService _taskBundles;
        public DeviceId DeviceId => _deviceId;

        public Device(TaskBundleService tasks, DeviceId deviceId)
        {
            _deviceId = deviceId;
            _taskBundles = tasks;
        }

        
    }
}