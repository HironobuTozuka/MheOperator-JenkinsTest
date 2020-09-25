using System.Collections.Generic;
using Common;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Services;

namespace RcsLogic.RcsController.Recovery
{
    public class NoToteOnSourceRecovery : IRecoveryStrategy
    {
        private readonly TaskBundleService _taskBundleService;
        private readonly ToteRepository _toteRepository;
        private readonly IStoreManagementClient _storeManagementClient;
        private ILogger<TotePickingFailedRecovery> _logger;

        public NoToteOnSourceRecovery(TaskBundleService taskBundleService, ILoggerFactory loggerFactory, 
            ToteRepository toteRepository, IStoreManagementClient storeManagementClient)
        {
            _taskBundleService = taskBundleService;
            _toteRepository = toteRepository;
            _storeManagementClient = storeManagementClient;
            _logger = loggerFactory.CreateLogger<TotePickingFailedRecovery>();
        }

        public List<Transfer> Recover(ITransferCompletingDevice device, TransferResult result)
        {
            _logger.LogInformation("Failing task for transfer result: ", result);
            _taskBundleService.FailTask(result.RequestDone.requestId.GetTaskId());
            var tote = _toteRepository.GetToteByBarcode(result.RequestedTransfer?.tote?.toteBarcode) 
                       ?? _toteRepository.GetToteByBarcode(result.RequestDone.sourceToteBarcode);

            if (tote == null || !tote.location.plcId.Equals(result.RequestDone.sourceLocationId))
            {
                _logger.LogDebug("Not setting tote location to null for result {0}", result.RequestDone);
                return new List<Transfer>();
            }
            
            _logger.LogDebug("Setting tote: {0} location to null", tote);
            _toteRepository.UpdateToteLocation(tote, null);
            _toteRepository.UpdateToteStatus(tote, ToteStatus.LocationUnknown);
            _storeManagementClient.ToteNotification(tote, null, ToteRotation.unknown, tote.status);
            return new List<Transfer>();
        }
    }
}