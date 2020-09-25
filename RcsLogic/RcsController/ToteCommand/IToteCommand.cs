using Common.Models.Location;
using Common.Models.Tote;

namespace RcsLogic.RcsController.ToteCommand
{
    public interface IToteCommand
    {
        public void Execute();
    }
}