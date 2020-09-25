using System;
using System.Collections.Generic;
using Common.Models.Location;
using Common.Models.Task;
using JsonSubTypes;
using Newtonsoft.Json;
using RcsLogic.Models;

namespace MheOperator.StoreManagementApi.Models.TaskController
{

    public class DeliverTaskDataModel : TaskBaseDataModel
    {
        public override TaskType type { get; } = TaskType.Deliver;

        public string toteId { get; set; }
        public int[] slots { get; set; }

        public override TaskBase toTaskBase()
        {
            return new DeliverTask()
            {
                taskId = new TaskId(this.taskId),
                toteId = this.toteId,
                slots = this.slots,
                processingStartedDate = DateTime.Now,
                lastUpdateDate = DateTime.Now
            };
        }
    }
}