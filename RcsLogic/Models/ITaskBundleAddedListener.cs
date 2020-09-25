using Common.Models;
using Common.Models.Task;

namespace RcsLogic.Models
{
    public interface ITaskBundleAddedListener
    {
        public void HandleNewTaskBundle(TaskBundle newTaskBundle);
    }
}