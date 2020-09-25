using System.Collections.Generic;
using Common.Exceptions;
using Common.Models.Location;
using Common.Models.Plc;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Services;

namespace RcsLogic.RcsController.Recovery
{
    public class NoReadToteOnCraneRecovery : IRecoveryStrategy
    {
        private readonly TaskBundleService _taskBundleService;
        private ILogger<TotePickingFailedRecovery> _logger;
        private readonly LocationRepository _locationRepository;
        private readonly ToteRepository _toteRepository;

        public NoReadToteOnCraneRecovery(TaskBundleService taskBundleService,
            ILoggerFactory loggerFactory,
            LocationRepository locationRepository, 
            ToteRepository toteRepository)
        {
            _taskBundleService = taskBundleService;
            _locationRepository = locationRepository;
            _toteRepository = toteRepository;
            _logger = loggerFactory.CreateLogger<TotePickingFailedRecovery>();
        }

        public List<Transfer> Recover(ITransferCompletingDevice device, TransferResult result)
        {
            _logger.LogInformation("Failing task for transfer result: ", result);
            _taskBundleService.FailTask(result.RequestDone.requestId.GetTaskId());

            if (result.RequestedTransfer != null)
            {
                try
                {
                    _logger.LogDebug("Updating tote {0} status to NOREAD: ", result.RequestedTransfer.tote);
                    _toteRepository.UpdateToteStatus(result.RequestedTransfer.tote, ToteStatus.NoRead);
                }
                catch (ToteNotFoundException ex)
                {
                    _logger.LogDebug("Tote {0} was not found: ", result.RequestedTransfer.tote);
                }
                
            }
            
            var toteTransfers = new List<Transfer>();

            var actualLocation = _locationRepository.GetLocationByPlcId(result.RequestDone.actualDestLocationId);
            if (actualLocation?.zone?.function == LocationFunction.Crane)
            {
                toteTransfers.Add(Transfer(actualLocation, actualLocation, result.RequestDone.sourceToteBarcode));  
            }

            return toteTransfers;
        }
        
        private Transfer Transfer(
            Location source,
            Location dest,
            string barcode)
        {
            var tote = GetTote(barcode);

            var moveTask = _taskBundleService.GetInternalMoveTask(tote, dest);
            return new Transfer()
            {
                destLocation = dest,
                sourceLocation = source,
                tote =  tote,
                task = moveTask
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