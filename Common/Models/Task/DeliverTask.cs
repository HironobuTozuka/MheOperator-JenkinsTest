namespace Common.Models.Task
{
    public class DeliverTask : TaskBase
    {
        public string toteId { get; set; }
        public int[] slots { get; set; }


        public override string ToString()
        {
            return "Deliver taskId: " + taskId + ", toteId: " + toteId  + ", slots: " + string.Join(",", slots);
        }
    }
}