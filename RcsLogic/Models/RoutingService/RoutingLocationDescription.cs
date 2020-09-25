namespace RcsLogic.Models.RoutingService
{
    public class RoutingLocationDescription
    {
        public int? locationId { get; set; }
        public int? locationGroupId { get; set; }
        public RoutingLocationDescription routedFrom { get; set; }
        public RoutingLocationDescription routedTo { get; set; }
        public int routeCost { get; set; }

        public string DeviceId { get; set; }
    }
}
