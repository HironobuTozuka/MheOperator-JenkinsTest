using System.Collections.Generic;
using Common.Models.Task;

namespace RcsLogic.Models
{
    public interface ITaskBundleCanceledListener
    {
        public void HandleTaskBundleCanceled(TaskBundle taskBundle);
    }
}