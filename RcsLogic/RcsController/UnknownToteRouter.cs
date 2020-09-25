using Common.Models.Location;
using Common.Models.Plc;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Services;

namespace RcsLogic.RcsController
{
    public class UnknownToteRouter
    {
        private readonly LocationRepository _locationRepository;
        private readonly RoutingService _routingService;
        private readonly LocationService _locationService;
        private readonly ILogger _logger;
        private readonly TaskBundleService _taskBundleService;

        public UnknownToteRouter(ILoggerFactory loggerFactory, 
            LocationRepository locationRepository,
            RoutingService routingService,
            LocationService locationService, 
            TaskBundleService taskBundleService)
        {
            _logger = loggerFactory.CreateLogger<UnknownToteRouter>();
            _locationRepository = locationRepository;
            _routingService = routingService;
            _locationService = locationService;
            _taskBundleService = taskBundleService;
        }

        public Transfer RequestTransfer(string locationPlcId, string toteBarcode, RequestToteType toteType)
        {
            var scanLocation = _locationRepository.GetLocationByPlcId(locationPlcId);
            if (LocationFunction.Crane.Equals(scanLocation.zone.function))
            {
                if (toteBarcode.Equals(Barcode.Unknown))
                {
                    _logger.LogWarning($"Received UNKNOWN tote from crane location, sending to crane again: {scanLocation}");
                    return Transfer(scanLocation, scanLocation, toteBarcode, toteType);
                }
                var zoneToStoreTote = _locationRepository.GetZoneByFunction(LocationFunction.Technical).zoneId;
                var toteNewBarcodeReadingLocation =
                    _locationService.GetToteStorageLocationFromZone(zoneToStoreTote, GetTote(toteBarcode, toteType));
                var nextLoc = _routingService.GetNextLocation(scanLocation, toteNewBarcodeReadingLocation);
                _logger.LogWarning($"Received wrong scan from crane location, sending to technical location {toteNewBarcodeReadingLocation}");
                return Transfer(scanLocation, nextLoc, toteBarcode, toteType);
            }

            var currentLocation = _locationRepository.GetLocationByPlcId(locationPlcId);
            var craneLocation = _locationService.GetClosestLocationOfFunction(currentLocation,LocationFunction.Crane);
            var nextLocation = _routingService.GetNextLocation(scanLocation, craneLocation);

            _logger.LogInformation("Route Unknown Tote, found next location as: {1}", nextLocation.plcId);

            return Transfer(scanLocation, nextLocation, toteBarcode, toteType);
        }

        private Tote GetTote(string barcode, RequestToteType toteType)
        {
            return new Tote()
            {
                toteBarcode = barcode,
                type = new ToteType(toteType.ToteHeight, toteType.TotePartitioning)
            };
        }

        private Transfer Transfer(
            Location source,
            Location dest,
            string barcode,
            RequestToteType toteType)
        {
            var tote = GetTote(barcode, toteType);

            var moveTask = _taskBundleService.GetInternalMoveTask(tote, dest);
            return new Transfer()
            {
                destLocation = dest,
                sourceLocation = source,
                tote =  tote,
                task = moveTask
            };
        }
    }
}