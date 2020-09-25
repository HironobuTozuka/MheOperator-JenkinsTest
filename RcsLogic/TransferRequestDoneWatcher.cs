using System;
using Common;
using Common.Models;
using Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Common.Models.Location;
using Common.Models.Tote;
using Microsoft.EntityFrameworkCore;
using RcsLogic.Models;
using RcsLogic.RcsController;
using RcsLogic.RcsController.Exceptions;
using RcsLogic.RcsController.Recovery;

namespace RcsLogic
{
    public class TransferRequestDoneWatcher : ITransferRequestDoneListener
    {
        private readonly ILogger<TransferRequestDoneWatcher> _logger;
        private readonly LocationRepository _locationRepository;
        private readonly ToteRepository _toteRepository;
        private readonly RcsController.RcsController _rcsController;
        private DeviceRegistry _deviceRegistry;

        public TransferRequestDoneWatcher(ILoggerFactory loggerFactory,
            LocationRepository locationRepository,
            ToteRepository toteRepository,
            RcsController.RcsController rcsController, DeviceRegistry deviceRegistry)
        {
            _logger = loggerFactory.CreateLogger<TransferRequestDoneWatcher>();
            _locationRepository = locationRepository;
            _toteRepository = toteRepository;
            _rcsController = rcsController;
            _deviceRegistry = deviceRegistry;
        }

        public void ProcessTransferRequestDone(TransferRequestDoneModel transferRequestDone)
        {
            if (transferRequestDone.transferRequest1Done != null &&
                !string.IsNullOrWhiteSpace(transferRequestDone.transferRequest1Done.requestId.Id))
                HandleRequestDone(new TransferResult()
                {
                    RequestDone = transferRequestDone.transferRequest1Done,
                    SystemSortCode = SystemSortCodes.Get(transferRequestDone.transferRequest1Done.sortCode),
                    ResultOrdinal = 1,
                    RequestedTransfer = _deviceRegistry.GetRequestedTransferForRequestDone(transferRequestDone.transferRequest1Done)
                });
            if (transferRequestDone.transferRequest2Done != null &&
                !string.IsNullOrWhiteSpace(transferRequestDone.transferRequest2Done.requestId.Id))
                HandleRequestDone(new TransferResult()
                {
                    RequestDone = transferRequestDone.transferRequest2Done,
                    SystemSortCode = SystemSortCodes.Get(transferRequestDone.transferRequest2Done.sortCode),
                    ResultOrdinal = 2,
                    RequestedTransfer = _deviceRegistry.GetRequestedTransferForRequestDone(transferRequestDone.transferRequest2Done)
                });
        }

        private void HandleRequestDone(TransferResult result)
        {
            ToteTransferRequestDoneModel requestDoneModel = result.RequestDone;
            Tote tote;
            var location = _locationRepository.GetLocationByPlcId(requestDoneModel.actualDestLocationId);
            
            _logger.LogTrace("Location found {0}, actualLocation: {1}", location.plcId,
                requestDoneModel.actualDestLocationId);
            Transfer requestedTransfer = null;
            try
            {
                requestedTransfer = _deviceRegistry.GetRequestedTransferForRequestDone(result.RequestDone);
            }
            catch (NoDeviceCanHandleTransferDoneException ex)
            {
                _logger.LogTrace($"Transfer not found in any device {result.RequestDone}");
            }

            if (requestedTransfer != null && requestedTransfer.tote.toteBarcode.Contains(Barcode.NoRead))
            {
                tote = _toteRepository.GetToteByBarcode(requestedTransfer.tote.toteBarcode);
            }
            else
            {
                tote = _toteRepository.GetToteByBarcode(requestDoneModel.sourceToteBarcode)
                           ?? _toteRepository.GetToteByBarcode(requestedTransfer?.tote.toteBarcode);
            }
            
            if (tote != null)
            {
                if (result.SystemSortCode.FailReason != SystemFailReason.NoTote)
                {
                    _logger.LogDebug("Tote found {0}, requested tote id: {1}, updating tote location", tote.toteBarcode,
                        requestDoneModel.sourceToteBarcode);
                    _toteRepository.UpdateToteLocation(tote, location);
                }
                else
                {
                    _logger.LogDebug("Tote found {0}, requested tote id: {1}, not saving tote location," +
                                     " since received sort code is {2}", tote.toteBarcode,
                                        requestDoneModel.sourceToteBarcode, result.SystemSortCode);
                }

            }
            else
            {
                _logger.LogDebug("Tote not found from transfer request conf {0}, not saved in db",
                    requestDoneModel.sourceToteBarcode);
            }

            if (FailedRequest(requestDoneModel))
            {
                _rcsController.HandleFailedTransferDoneRequest(result);
            }
        }

        private bool FailedRequest(ToteTransferRequestDoneModel transferRequestDone)
        {
            if (transferRequestDone.sortCode != 1)
            {
                _logger.LogError("In move request conf received sort code different than 1: {0} on dest location {1}",
                    transferRequestDone.sourceToteBarcode, transferRequestDone.actualDestLocationId);
                return true;
            }

            return false;
        }
    }
}