using Common.Models.Tote;

namespace Common.Models.Task
{
    public class PickTask : TaskBase
    {
        public string barcode { get; set; }
        public PickToteData sourceTote { get; set; }
        public PickToteData destTote { get; set; }
        public int quantity { get; set; }

        public override string ToString()
        {
            return
                $"Pick Task id: {taskId}, barcode: {barcode}, quantity: {quantity}," +
                $" source tote:  {sourceTote}, dest tote: {destTote}";
        }
    }
}