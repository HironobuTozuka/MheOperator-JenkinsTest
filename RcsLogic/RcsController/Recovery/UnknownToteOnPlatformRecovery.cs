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

namespace RcsLogic.RcsController.Recovery
{
    public class UnknownToteOnPlatformRecovery : IRecoveryStrategy
    {
        private readonly LocationRepository _locationRepository;
        private readonly ILogger _logger;

        public UnknownToteOnPlatformRecovery(LocationRepository locationRepository,
            ILoggerFactory loggerFactory)
        {
            _locationRepository = locationRepository;
            _logger = loggerFactory.CreateLogger<UnknownToteOnPlatformRecovery>();
        }

        public List<Transfer> Recover(ITransferCompletingDevice device, TransferResult result)
        {
            string platformLocationPlcId = device.DeviceId.id + result.ResultOrdinal;

            Location platformLocation = _locationRepository.GetLocationByPlcId(platformLocationPlcId);

            var transfers = new List<Transfer>();

            var transfer = Transfer(platformLocation, platformLocation, "UNKNOWN", new RequestToteType()
            {
                ToteHeight = ToteHeight.unknown,
                TotePartitioning = TotePartitioning.unknown
            });

            _logger.LogInformation("Created transfer from platform: {1} for result: {2}", transfer, result);
            transfers.Add(transfer);

            return transfers;
        }

        private Transfer Transfer(
            Location source,
            Location dest,
            string barcode,
            RequestToteType toteType)
        {
            var tote = new Tote()
            {
                toteBarcode = barcode,
                type = new ToteType(toteType.ToteHeight, toteType.TotePartitioning)
            };

            var moveTask = new MoveTask {taskId = new TaskId(Guid.NewGuid().ToString()), destLocation = dest};
            return new Transfer()
            {
                destLocation = dest,
                sourceLocation = source,
                tote = tote,
                task = moveTask
            };
        }
    }
}