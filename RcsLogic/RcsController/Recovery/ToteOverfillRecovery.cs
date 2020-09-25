using System.Collections.Generic;
using Common.Exceptions;
using Common.Models.Location;
using Common.Models.Plc;
using Common.Models.Task;
using Common.Models.Tote;
using Confluent.Kafka;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Services;

namespace RcsLogic.RcsController.Recovery
{
    public class ToteOverfillRecovery : IRecoveryStrategy
    {
        private readonly TaskBundleService _taskBundleService;
        private ILogger<ToteOverfillRecovery> _logger;
        private readonly LocationRepository _locationRepository;
        private readonly ToteRepository _toteRepository;
        private readonly LocationService _locationService;
        private readonly RoutingService _routingService;

        public ToteOverfillRecovery(TaskBundleService taskBundleService,
            ILoggerFactory loggerFactory,
            LocationRepository locationRepository, 
            ToteRepository toteRepository,
            LocationService locationService,
            RoutingService routingService)
        {
            _taskBundleService = taskBundleService;
            _locationRepository = locationRepository;
            _toteRepository = toteRepository;
            _locationService = locationService;
            _routingService = routingService;
            _logger = loggerFactory.CreateLogger<ToteOverfillRecovery>();
        }

        public List<Transfer> Recover(ITransferCompletingDevice device, TransferResult result)
        {
            
            var tote = _toteRepository.GetToteByBarcode(result.RequestDone.sourceToteBarcode);
            var technicalZone = _locationRepository.GetZoneByFunction(LocationFunction.Technical).zoneId;
            if (tote != null)
            {
                _logger.LogWarning("Setting tote {0} status to overfill", tote);
                _toteRepository.UpdateToteStatus(tote, ToteStatus.Overfill);
                
                _toteRepository.UpdateToteStorageLocation(tote,
                        _locationService.GetToteStorageLocationFromZone(technicalZone, tote));
                _logger.LogTrace("Updated tote: {0} storage location to: {1}", tote, tote.storageLocation);
            }
            else
            {
                tote = GetTote(result.RequestDone.sourceToteBarcode);
                tote.location = _locationRepository.GetLocationByPlcId(result.RequestDone.actualDestLocationId);
                tote.storageLocation = _locationService.GetToteStorageLocationFromZone(technicalZone, tote);
            }

            _logger.LogInformation("Failing task for transfer result: ", result);
            _taskBundleService.FailTask(result.RequestDone.requestId.GetTaskId());

            var toteTransfers = new List<Transfer>();

            var actualLocation = _locationRepository.GetLocationByPlcId(result.RequestDone.actualDestLocationId);
            if (actualLocation?.zone?.function != LocationFunction.Crane)
            {
                _logger.LogDebug("Not adding any further requests, since tote {0} is already on racking", tote);
                return toteTransfers;
            }
            
            var nextLocation = _routingService.GetNextLocation(tote.location, tote.storageLocation);
            var moveTask = _taskBundleService.GetInternalMoveTask(tote, tote.storageLocation);
            toteTransfers.Add(Transfer(tote, actualLocation, nextLocation, moveTask));
            _logger.LogDebug("Not adding any further requests, since tote {0} is already on racking", tote);

            return toteTransfers;
        }
        
        private Transfer Transfer(
            Tote tote,
            Location source,
            Location dest,
            TaskBase task)
        {
            return new Transfer()
            {
                destLocation = dest,
                sourceLocation = source,
                tote =  tote,
                task = task
            };
        }
        
        private Tote GetTote(string barcode)
        {
            return new Tote()
            {
                toteBarcode = barcode,
                type = new ToteType(ToteHeight.unknown, TotePartitioning.unknown)
            };
        }
    }
}