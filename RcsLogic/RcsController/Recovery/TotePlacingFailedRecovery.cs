using System;
using System.Collections.Generic;
using Common.Models;
using Common.Models.Location;
using Common.Models.Plc;
using Common.Models.Task;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Services;

namespace RcsLogic.RcsController.Recovery
{
    public class TotePlacingFailedRecovery : IRecoveryStrategy
    {
        private readonly LocationRepository _locationRepository;
        private readonly ToteRepository _toteRepository;
        private readonly Services.LocationService _locationService;
        private readonly UnknownToteRouter _unknownToteRouter;
        private readonly TaskBundleService _taskBundleService;
        private ILogger<TotePlacingFailedRecovery> _logger;

        public TotePlacingFailedRecovery(ToteRepository toteRepository,
            LocationRepository locationRepository,
            Services.LocationService locationService,
            UnknownToteRouter unknownToteRouter,
            TaskBundleService taskBundleService,
            ILoggerFactory loggerFactory)
        {
            _toteRepository = toteRepository;
            _locationRepository = locationRepository;
            _locationService = locationService;
            _unknownToteRouter = unknownToteRouter;
            _taskBundleService = taskBundleService;
            _logger = loggerFactory.CreateLogger<TotePlacingFailedRecovery>();
        }

        public List<Transfer> Recover(ITransferCompletingDevice device, TransferResult result)
        {
            _logger.LogInformation("TotePlacingFailedRecoveryStrategy: Recovering from: {1}", result);
            var locationPlcId = result.RequestDone.actualDestLocationId;
            var toteBarcode = result.RequestDone.sourceToteBarcode;

            var tote = _toteRepository.GetToteByBarcode(toteBarcode);
            var destination = _locationRepository.GetLocationByPlcId(result.RequestDone.requestedDestLocationId);
            var currentLocation = _locationRepository.GetLocationByPlcId(locationPlcId);

            var transfers = new List<Transfer>();

            if (IsRackingLocation(destination))
            {
                _logger.LogInformation("Handling racking place failure for: {1}", result);
                transfers.AddRange(HandleRackingPlaceFailure(result,
                    tote,
                    destination,
                    currentLocation));
            }
            else
            {
                _logger.LogInformation("Handling conveyor place failure for: {1}", result);
                transfers.AddRange(HandleConveyorPlaceFailure(result,
                    tote,
                    destination,
                    currentLocation));
            }

            return transfers;
        }

        private List<Transfer> HandleConveyorPlaceFailure(TransferResult result, Tote tote, Location destination,
            Location currentLocation)
        {
            if (tote == null)
            {
                _logger.LogDebug("Tote is null, not adding transfer");
                return new List<Transfer>();
            }
            if (result.SystemSortCode.FailReason != SystemFailReason.PlaceLocationOccupied)
            {
                _taskBundleService.FailTask(result.RequestDone.requestId.GetTaskId());
            }
            return new List<Transfer> {MoveToStorage(tote, tote.storageLocation, currentLocation)};
        }

        private static bool IsRackingLocation(Location destLocation)
        {
            return destLocation.zone.function.Equals(LocationFunction.Staging)
                   || destLocation.zone.function.Equals(LocationFunction.Storage);
        }

        private List<Transfer> HandleRackingPlaceFailure(TransferResult result,
            Tote tote,
            Location destination,
            Location currentLocation)
        {
            var tempTransfers = new List<Transfer>();
            var shuffleLocation = _locationRepository.GetShuffleLocation(destination);

            if (tote?.toteBarcode == null || tote.toteBarcode.Contains(Barcode.Unknown) ||
                tote.toteBarcode.Contains(Barcode.NoRead))
            {
                _logger.LogInformation("Not saving tote location in HandleRackingPlaceFailure, UNKNOWN or NOREAD");
                if (tote?.toteBarcode == null || tote.toteBarcode.Contains(Barcode.NoRead))
                {
                    _logger.LogInformation("Not handling NOREAD in HandleRackingPlaceFailure");
                    return tempTransfers;
                }
            }
            else
            {
                _toteRepository.UpdateToteStorageLocation(tote, shuffleLocation);

                _logger.LogInformation("Updated tote {1} storage location: {2}", tote.toteBarcode, shuffleLocation);
            }

            var moveToShuffle = MoveToStorage(tote, shuffleLocation, currentLocation);

            _logger.LogInformation("Created move for tote: {1}", moveToShuffle);

            var transferOfUnknownTote = _unknownToteRouter.RequestTransfer(
                result.RequestDone.requestedDestLocationId, "UNKNOWN",
                new RequestToteType(tote.type.toteHeight, TotePartitioning.bipartite));

            _logger.LogInformation("Created move to scan unexpected tote: {1}", transferOfUnknownTote);

            tempTransfers.Add(transferOfUnknownTote);
            tempTransfers.Add(moveToShuffle);

            return tempTransfers;
        }

        private static Transfer MoveToStorage(Tote tote, Location destinationLocation, Location currentLocation)
        {
            var moveTask = new MoveTask {taskId = new TaskId(Guid.NewGuid().ToString()), destLocation = destinationLocation};
            var moveToShuffle = new Transfer()
            {
                task = moveTask,
                tote = tote,
                destLocation = destinationLocation,
                sourceLocation = currentLocation
            };
            return moveToShuffle;
        }
    }
}