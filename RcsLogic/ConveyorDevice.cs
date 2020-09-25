using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Models;
using Common.Models.Plc;
using Common.Models.Task;
using Common.Models.Tote;
using Common.Models.Transfer;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Models.Device;
using RcsLogic.Robot;
using RcsLogic.Services;

namespace RcsLogic
{
    public class ConveyorDevice : Device, IToteShakingDevice, ITransferRequestDoneListener, ITransferCompletingDevice
    {
        private readonly ILogger<ConveyorDevice> _logger;
        private readonly IPlcService _plcService;
        private readonly TransferCollection _transfers;

        public ConveyorDevice(DeviceId deviceId,
            IPlcService plcService,
            ILoggerFactory loggerFactory,
            TaskBundleService tasks, 
            LocationStatus locationStatus,
            TotesReadyForPicking totesReadyForPicking) : base(tasks, deviceId)
        {
            _transfers = new TransferCollection(loggerFactory, deviceId, locationStatus, totesReadyForPicking);
            _logger = loggerFactory.CreateLogger<ConveyorDevice>();
            _plcService = plcService;
        }

        public void Execute(Transfer request)
        {
            if (request == null)
            {
                _logger.LogInformation("deviceId: {1}, Requested to execute null!, Skipping!", _deviceId);
                return;
            }

            request.status = Transfer.RequestStatus.Execute;
            _transfers.GetRequestsInExecution().Where(r =>
                request.sourceLocation.Equals(r.sourceLocation) && request.destLocation.Equals(r.destLocation))
                .ToList().ForEach(t => _transfers.Remove(t));
            _transfers.Add(request);
            
            _taskBundles.UpdateTaskStatus(request.task?.taskId, RcsTaskStatus.Executing);

            _plcService.RequestTransfer(new TransferRequestModel(
                new ToteTransferRequestModel()
                {
                    DestLocationId = request.destLocation.plcId,
                    SourceLocationId = request.sourceLocation.plcId,
                    ToteBarcode = request.tote.toteBarcode,
                    ToteType = new RequestToteType(request.tote.type),
                    Id = new TransferId(request.task.taskId)
                }, null));
        }

        public void Execute(List<Transfer> transfers)
        {
            transfers.ForEach(Execute);
        }

        public void ShakeTote(Tote tote)
        {
            _logger.LogDebug("Shaking tote: {0}", tote);
            _plcService.RequestTransfer(new TransferRequestModel(
                new ToteTransferRequestModel()
                {
                    DestLocationId = tote.location.plcId,
                    SourceLocationId = tote.location.plcId,
                    ToteBarcode = tote.toteBarcode,
                    ToteType = new RequestToteType(tote.type),
                    Id = new TransferId(Guid.NewGuid().ToString())
                }, null));
        }

        public void ProcessTransferRequestDone(TransferRequestDoneModel moveRequestDone)
        {
            _transfers.RemoveAllMatchingRequestDone(moveRequestDone.transferRequest1Done);
        }

        public bool ShouldHandleTransferDone(ToteTransferRequestDoneModel transferRequestDone)
        {
            lock (this)
            {
                return _transfers.AnyMatchingRequestDone(transferRequestDone);
            }
        }

        public Transfer GetCompletedTransfer(ToteTransferRequestDoneModel transferRequestDoneModel)
        {
            return _transfers.GetMatchingRequestDone(transferRequestDoneModel);
        }
    }
}