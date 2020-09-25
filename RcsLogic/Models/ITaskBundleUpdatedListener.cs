using System.Collections.Generic;
using Common.Models.Task;

namespace RcsLogic.Models
{
    public interface ITaskBundleUpdatedListener
    {
        public void HandleTaskBundleUpdated(TaskBundle taskBundle, List<TaskBase> oldTasks, List<TaskBase> newTasks);
    }
}