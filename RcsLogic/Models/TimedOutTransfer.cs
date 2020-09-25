using RcsLogic.Models.Device;

namespace RcsLogic.Models
{
    public class TimedOutTransfer
    {
        public IDevice device { get; set; }
        public Transfer transfer { get; set; }

        public override string ToString()
        {
            return $"{nameof(device)}: {device}, {nameof(transfer)}: {transfer}";
        }
    }
}