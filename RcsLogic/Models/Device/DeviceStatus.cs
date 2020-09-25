namespace RcsLogic.Models.Device
{
    public class DeviceStatus
    {
        public DeviceStatus(DeviceId deviceId, Status status)
        {
            DeviceId = deviceId;
            Status = status;
        }

        public DeviceId DeviceId { get; }
        public Status Status { get; }
    }
}