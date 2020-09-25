using System;
using System.Collections.Generic;

namespace Common.Models.Task
{
    public class TaskBundle
    {
        public TaskBundleId taskBundleId { get; set; }
        public DateTime creationDate { get; set; }
        public List<TaskBase> tasks { get; set; }
        public bool isInternal { get; set; }
    }
}