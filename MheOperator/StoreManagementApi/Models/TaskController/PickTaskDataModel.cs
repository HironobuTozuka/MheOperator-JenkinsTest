using System;
using Common.Models.Task;

namespace MheOperator.StoreManagementApi.Models.TaskController
{
    public class PickTaskDataModel : TaskBaseDataModel
    {
        public override TaskType type { get; } = TaskType.Pick;

        public string productBarcode { get; set; }
        public ToteDataModel sourceTote { get; set; }
        public ToteDataModel destTote { get; set; }
        public int quantity { get; set; }

        public override TaskBase toTaskBase() =>
            new PickTask()
            {
                taskId = new TaskId(this.taskId),
                barcode = this.productBarcode,
                sourceTote = this.sourceTote.toToteData(),
                destTote = this.destTote.toToteData(),
                quantity = this.quantity,
                processingStartedDate = DateTime.Now,
                lastUpdateDate = DateTime.Now
            };
    }
}