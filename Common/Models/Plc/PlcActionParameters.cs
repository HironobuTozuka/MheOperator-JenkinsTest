using Common.Models.Gate;
using Common.Models.Led;

namespace Common.Models.Plc
{
    public class PlcActionParameters
    {
        public GateDescription GateDescription { get; set; }
        public GateAction GateAction { get; set; }
        
        public LedId LedId { get; set; }
        public LedState LedState { get; set; }
    }
}