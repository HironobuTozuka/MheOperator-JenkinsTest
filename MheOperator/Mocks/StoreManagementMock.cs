using Common;
using Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;

namespace Tests
{
    public class StoreManagementMock : IStoreManagementClient
    {
        private ILogger _logger;
        public int partsPicked { get; private set; }
        public FailReason? failReason { get; private set; }
        public bool recievedScanFromLoadingGate { get; private set; }
        public List<ToteNotificationSent> SentNotifications { get; } = new List<ToteNotificationSent> ();

        public StoreManagementMock(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<StoreManagementMock>();
            _logger.LogInformation("Using StoreManagementMock");
            partsPicked = 0;
        }
        
        public void ToteNotification(Tote tote, Location scanLocation, ToteRotation toteRotation,
            ToteStatus toteStatus)
        {
            SentNotifications.Add(new ToteNotificationSent(tote, scanLocation, toteRotation, toteStatus));
            if(scanLocation?.zone?.function == LocationFunction.LoadingGate) recievedScanFromLoadingGate = true;
            _logger.LogInformation("Tote scan would be sent for: {0}, tote: {1}", scanLocation,
                tote);
        }

        public ToteData GetToteDetails(string toteBarcode)
        {
            return new ToteData()
            {
                toteId = toteBarcode,
                maxAcc = 100,
                weight = 10
            };
        }

        public void ReportTaskState(TaskBase task, int? picked = null, int? failed = null, FailReason? failReason = null,
            string failDescription = null)
        {
            this.failReason = failReason;
            _logger.LogInformation("Task state update would be sent to SM for task: {0}, picked: {1}, failed {2}",
                task.taskId, picked, failed);
            if (picked != null) partsPicked += (int) picked;
        }
        
        public class ToteNotificationSent
        {
            public ToteNotificationSent(Tote tote, Location scanLocation, ToteRotation toteRotation, ToteStatus toteStatus)
            {
                this.tote = tote;
                ScanLocation = scanLocation;
                this.toteRotation = toteRotation;
                this.toteStatus = toteStatus;
            }

            public Tote tote { get; }
            public Location ScanLocation { get; }
            public ToteRotation toteRotation { get; }
            public ToteStatus toteStatus { get; }
        }
    }
}