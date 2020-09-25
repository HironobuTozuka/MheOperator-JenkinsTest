using Common.Models;

namespace RcsLogic.Models
{
    public interface ITaskBundleRemovedListener
    {
        public void HandleTaskBundleRemoved();
    }
}