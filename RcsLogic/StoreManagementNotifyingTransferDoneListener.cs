using System.Collections.Generic;
using Common;
using Common.Models;
using Common.Models.Location;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Crane;
using RcsLogic.Models;
using RcsLogic.Services;

namespace RcsLogic
{
    public class StoreManagementNotifyingTransferDoneListener : ITransferRequestDoneListener
    {
        private readonly ILogger<StoreManagementNotifyingTransferDoneListener> _logger;
        private readonly ToteRepository _toteRepository;
        private readonly ToteService _toteService;
        private readonly LocationRepository _locationRepository;
        private readonly IStoreManagementClient _storeManagementClient;

        public StoreManagementNotifyingTransferDoneListener(
            ILoggerFactory loggerFactory,
            ToteRepository toteRepository, 
            ToteService toteService, 
            LocationRepository locationRepository,
            IStoreManagementClient storeManagementClient)
        {
            _toteRepository = toteRepository;
            _toteService = toteService;
            _locationRepository = locationRepository;
            _storeManagementClient = storeManagementClient;
            _logger = loggerFactory.CreateLogger<StoreManagementNotifyingTransferDoneListener>();
        }

        public void ProcessTransferRequestDone(TransferRequestDoneModel moveRequestDone)
        {
            ProcessToteTransfer(moveRequestDone.transferRequest1Done);
            ProcessToteTransfer(moveRequestDone.transferRequest2Done);
        }

        private void ProcessToteTransfer(ToteTransferRequestDoneModel toteRequestDone)
        {
            if(toteRequestDone == null) return;
            _logger.LogTrace("NoReadHandlingTransferDoneListener is handling Transfer done {0}", toteRequestDone);
            var tote = _toteRepository.GetToteByBarcode(toteRequestDone.sourceToteBarcode);
            tote ??= CreateNoReadTote(toteRequestDone);
            if(tote != null && tote.status != ToteStatus.Ready && tote.location.IsRackingLocation)
                _storeManagementClient.ToteNotification(tote, tote.location, ToteRotation.unknown, tote.status);
            
        }

        private Tote CreateNoReadTote(ToteTransferRequestDoneModel toteRequestDone)
        {
            if (!toteRequestDone.sourceToteBarcode.Equals(Barcode.NoRead));
            var location = _locationRepository.GetLocationByPlcId(toteRequestDone.actualDestLocationId);
            if (location == null || location.zone.function != LocationFunction.Technical) return null;
            _logger.LogTrace("Creating NOREAD tote", toteRequestDone);
            return _toteService.CreateNoReadTote(location, location);
        }

        
    }
}