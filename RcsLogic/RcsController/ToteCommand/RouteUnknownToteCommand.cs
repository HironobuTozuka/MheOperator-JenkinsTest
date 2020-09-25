using Common.Models.Location;
using Common.Models.Plc;
using Common.Models.Tote;
using Microsoft.Extensions.Logging;

namespace RcsLogic.RcsController.ToteCommand
{
    public class RouteUnknownToteCommand : IToteCommand
    {
        private readonly ILogger<RouteUnknownToteCommand> _logger;
        private readonly UnknownToteRouter _unknownToteRouter;
        private readonly DeviceRegistry _deviceRegistry;
        private readonly Location _scanLocation;
        private readonly Tote _tote;
        private readonly ToteRotation _toteRotation;

        public RouteUnknownToteCommand(ILoggerFactory loggerFactory, UnknownToteRouter unknownToteRouter, DeviceRegistry deviceRegistry, Location scanLocation, Tote tote, ToteRotation toteRotation)
        {
            _unknownToteRouter = unknownToteRouter;
            _deviceRegistry = deviceRegistry;
            this._scanLocation = scanLocation;
            this._tote = tote;
            this._toteRotation = toteRotation;
            _logger = loggerFactory.CreateLogger<RouteUnknownToteCommand>();
        }

        public void Execute()
        {
            _logger.LogDebug("Route Unknown Tote, handling tote {0} on scan notification: {1}", _tote, _scanLocation);
            if (_tote.toteBarcode == Barcode.NoTote)
            {
                _logger.LogDebug("Skipping NOTOTE handling tote {0} on scan notification: {1}", _tote, _scanLocation);
                return;
            }
            //Best option would be to have method: RequestTransferFromLocation(Location source). Can we hardcode tote type and barcode here?
            var transfer = _unknownToteRouter.RequestTransfer(_scanLocation.plcId,
                _tote.toteBarcode, new RequestToteType(_tote.type));
            var device = _deviceRegistry.ChooseDeviceForTransfer(transfer);

            device.Execute(transfer);
        }
    }
}