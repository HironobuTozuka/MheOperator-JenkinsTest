using Common.Models.Location;
using Common.Models.Tote;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;

namespace RcsLogic.RcsController.ToteCommand
{
    public class NotifyRobotCommand : IToteCommand
    {
        private readonly ILogger<NotifyRobotCommand> _logger;
        private readonly DeviceRegistry _deviceRegistry;
        private readonly Location _scanLocation;
        private readonly Tote _tote;
        private readonly ToteRotation _toteRotation;

        public NotifyRobotCommand(ILoggerFactory loggerFactory, DeviceRegistry deviceRegistry, Location scanLocation, Tote tote, ToteRotation toteRotation)
        {
            _deviceRegistry = deviceRegistry;
            this._scanLocation = scanLocation;
            this._tote = tote;
            this._toteRotation = toteRotation;
            _logger = loggerFactory.CreateLogger<NotifyRobotCommand>();
        }

        public void Execute()
        {
            _logger.LogInformation("Tote on robot location {1}", _tote.toteBarcode);
            var toteReady = new PrepareForPicking()
            {
                Tote = _tote,
                Location = _scanLocation,
                ToteRotation = _toteRotation
            };
            
            var device = _deviceRegistry.ChooseDeviceForPickingPreparation(toteReady);

            device.ToteReady(toteReady);
        }
    }
}