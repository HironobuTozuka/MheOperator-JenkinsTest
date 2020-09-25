using Common.Models.Location;

namespace Common.Models.Task
{
    public class MoveTask : TaskBase
    {
        public string toteId { get; set; }
        public ZoneId destZone { get; set; }
        public Location.Location destLocation { get; set; }

        public override string ToString()
        {
            return $"Move Task id: {taskId} , tote Id: {toteId} , destZone: {destZone}, destLocation: {destLocation}";
        }
    }
}