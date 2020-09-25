using System;
using Common.Models.Location;
using Common.Models.Task;

namespace MheOperator.StoreManagementApi.Models.TaskController
{
    public class MoveTaskDataModel : TaskBaseDataModel
    {
        public override TaskType type { get; } = TaskType.Move;

        public string toteId { get; set; }
        public ZoneId destLocation { get; set; }

        public override TaskBase toTaskBase()
        {
            var zone = this.destLocation;
            return new MoveTask()
            {
                destZone = zone,
                taskId = new TaskId(this.taskId),
                toteId = this.toteId,
                processingStartedDate = DateTime.Now,
                lastUpdateDate = DateTime.Now
            };
        }
    }
}