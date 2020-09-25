using Common;
using Common.Models;
using Common.Models.Location;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.RcsController;
using RcsLogic.RcsController.Exceptions;
using RcsLogic.Services;

namespace RcsLogic
{
    public class ToteBarcodeReadOnRequestForNoReadTransferDoneListener : ITransferRequestDoneListener
    {
        private readonly ILogger<ToteBarcodeReadOnRequestForNoReadTransferDoneListener> _logger;
        private readonly LocationService _locationService;
        private readonly DeviceRegistry _deviceRegistry;
        private readonly ToteRepository _toteRepository;
        private readonly LocationRepository _locationRepository;

        public ToteBarcodeReadOnRequestForNoReadTransferDoneListener(ILoggerFactory loggerFactory, 
            LocationService locationService,
            DeviceRegistry deviceRegistry,
            ToteRepository toteRepository, LocationRepository locationRepository)
        {
            _locationService = locationService;
            _deviceRegistry = deviceRegistry;
            _toteRepository = toteRepository;
            _locationRepository = locationRepository;
            _logger = loggerFactory.CreateLogger<ToteBarcodeReadOnRequestForNoReadTransferDoneListener>();
        }

        public void ProcessTransferRequestDone(TransferRequestDoneModel moveRequestDone)
        {
            ProcessToteTransfer(moveRequestDone.transferRequest1Done);
            ProcessToteTransfer(moveRequestDone.transferRequest2Done);
        }

        private void ProcessToteTransfer(ToteTransferRequestDoneModel toteRequestDone)
        {
            if (toteRequestDone == null) return;
            _logger.LogTrace("ToteBarcodeReadOnRequestForNoReadTransferDoneListener is handling Transfer done {0}", toteRequestDone);
            try
            {
                var transfer = _deviceRegistry.GetRequestedTransferForRequestDone(toteRequestDone);
                var tote = _toteRepository.GetToteByBarcode(toteRequestDone.sourceToteBarcode);
                UpdateToteIfTheRequestWasForNoRead(tote, toteRequestDone, transfer);
            }
            catch (NoDeviceCanHandleTransferDoneException ex)
            {
                _logger.LogTrace("No device could handle transfer done {0}", ex);
            }
        }

        private void UpdateToteIfTheRequestWasForNoRead(Tote tote, ToteTransferRequestDoneModel transferDone, Transfer requestedTransfer)
        {
            if(requestedTransfer == null || tote == null) return;
            if (!requestedTransfer.tote.toteBarcode.Equals(Barcode.NoRead) ||
                transferDone.sourceToteBarcode.Equals(requestedTransfer.tote.toteBarcode)) return;
            _logger.LogWarning("Updating tote {0} status to NoRead, previously tote was NOREAD.", tote);
            SetToteStatusToNoRead(tote);
            SetToteStorageLocationToTechnicalLocation(tote);
        }

        private void SetToteStorageLocationToTechnicalLocation(Tote tote)
        {
            _toteRepository.UpdateToteStorageLocation(tote, _locationService
                .GetToteStorageLocationFromZone(_locationRepository
                    .GetZoneByFunction(LocationFunction.Technical).zoneId, tote));
        }

        private void SetToteStatusToNoRead(Tote tote)
        {
            _toteRepository.UpdateToteStatus(tote, ToteStatus.NoRead);
        }
    }
}