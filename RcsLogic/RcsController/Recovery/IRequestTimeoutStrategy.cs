using RcsLogic.Models;

namespace RcsLogic.RcsController.Recovery
{
    public interface IRequestTimeoutStrategy
    {
        public void Recover(TimedOutTransfer timedOutTransfer);
    }
}