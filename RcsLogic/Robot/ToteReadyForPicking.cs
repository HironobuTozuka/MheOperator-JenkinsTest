using Common.Models;
using Common.Models.Location;
using Common.Models.Tote;

namespace RcsLogic.Robot
{
    public class ToteReadyForPicking
    {
        public Tote Tote { get; set; }
        public Location Location { get; set; }
        public ToteRotation ToteRotation { get; set; }
        public bool Blocked { get; set; }
        public ToteReadyForPickingStatus Status { get; set; } = ToteReadyForPickingStatus.Ready;
        public int ShakingCount { get; set; } = 0;
    }

    public enum ToteReadyForPickingStatus
    {
        Ready,
        Shaking,
    }
}