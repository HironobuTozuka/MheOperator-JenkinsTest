using System;
using System.Collections.Generic;
using System.Text;
using Common.Models;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;

namespace Common
{
    public interface IStoreManagementClient
    {
        public void ReportTaskState(TaskBase task, int? picked = null, int? failed = null, FailReason? failReason = null, string failDescription = null);
        public void ToteNotification(Tote tote, Location scanLocation, ToteRotation toteRotation, ToteStatus toteStatus);
        public ToteData GetToteDetails(string toteBarcode);
    }
}