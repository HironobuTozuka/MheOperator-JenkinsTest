using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Task;
using MheOperator.StoreManagementApi.Models.Exceptions;

namespace MheOperator.StoreManagementApi.Models.TaskController
{
    public class TaskBundleDataModel
    {
        public string taskBundleId { get; set; }
        public List<TaskBaseDataModel> tasks { get; set; }
        
        public TaskBundle toTaskBundle()
        {
            if (string.IsNullOrEmpty(taskBundleId))
                throw new TaskBundleParsingException("Received task bundle with empty taskBundleId");
            if (tasks.Any(task => string.IsNullOrEmpty(task.taskId)))
                throw new TaskBundleParsingException("Received tasks with empty taskId");
            if (tasks.GroupBy(task => task.taskId).Any(taskGroup => taskGroup.Count() > 1))
                throw new TaskBundleParsingException("Received duplicated task Id's in task bundle");
            if (tasks.OfType<PickTaskDataModel>().Any(task => string.IsNullOrEmpty(task.productBarcode)))
                throw new TaskBundleParsingException("Received tasks with empty sku id");
            if (tasks.OfType<PickTaskDataModel>().Any(task => task.quantity == 0))
                throw new TaskBundleParsingException("Received pick tasks with no parts to pick");
            if (tasks.OfType<MoveTaskDataModel>().Any(task => string.IsNullOrEmpty(task.toteId)))
                throw new TaskBundleParsingException("Received move tasks with empty toteId");
            
            return new TaskBundle()
            {
                taskBundleId = new TaskBundleId(taskBundleId),
                tasks = tasks.Select(task => task.toTaskBase()).ToList(),
                creationDate = DateTime.Now
            };
        }
    }
}