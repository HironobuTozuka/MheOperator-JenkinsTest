using RcsLogic.Models;
using RcsLogic.Models.Device;
using RcsLogic.Robot;

namespace Tests
{
    public class MockPrepareForPickingDevice : IPrepareForPickingDevice
    {
        private readonly IPrepareForPickingDevice _robot;
        private readonly IPrepareForPickingDevice _deviceToPropagate;

        public MockPrepareForPickingDevice(IPrepareForPickingDevice robot, IPrepareForPickingDevice deviceToPropagate)
        {
            _robot = robot;
            _deviceToPropagate = deviceToPropagate;
        }


        public void ToteReady(PrepareForPicking prepareForPicking)
        {
            _deviceToPropagate.ToteReady(prepareForPicking);
            _robot.ToteReady(prepareForPicking);
        }

        public DeviceId DeviceId => new DeviceId("MockPrepareForPickingDevice");
    }
}