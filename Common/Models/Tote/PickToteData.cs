namespace Common.Models.Tote
{
    public class PickToteData
    {
        public string toteId { get; set; }
        public int slotId { get; set; }
        public Location.Location pickLocation;

        public override string ToString()
        {
            return $"{nameof(toteId)}: {toteId}, {nameof(slotId)}: {slotId}";
        }
    }
}