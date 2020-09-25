using Common.Models;
using Common.Models.Task;

namespace RcsLogic.Models
{
    public interface ITaskRemovedListener
    {
        public void HandleTaskRemoved(TaskBase task);
    }
}