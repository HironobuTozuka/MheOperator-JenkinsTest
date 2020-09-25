using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Task;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.RcsController.Exceptions;
using RcsLogic.Services;

namespace RcsLogic.Watchdog
{
    public class TaskBundleWatchdog : IWatchdog
    {
        private readonly TaskBundleService _taskBundleService;
        private readonly IReturnToteHandler _returnToteHandler;
        private readonly ILogger<TaskBundleWatchdog> _logger;

        private readonly TimeSpan _moveTaskAbsoluteTimeout;
        private readonly TimeSpan _startedTasksNoActionTimeout;
        private readonly TimeSpan _startedTasksAbsoluteTimeout;
        public bool Enabled { get; set; } = true;

        public TaskBundleWatchdog(
            IConfiguration configuration,
            ILoggerFactory loggerFactory, 
            TaskBundleService taskBundleService, 
            IReturnToteHandler returnToteHandler)
        {
            _taskBundleService = taskBundleService;
            _returnToteHandler = returnToteHandler;
            _logger = loggerFactory.CreateLogger<TaskBundleWatchdog>();
            Enabled = configuration["Watchdog:TaskBundleWatchdog:Enabled"]?.Equals(true.ToString(), StringComparison.CurrentCultureIgnoreCase) ?? true;
            _moveTaskAbsoluteTimeout = GetTimeoutFromConfiguration(configuration, "MoveTaskAbsoluteTimeout", 10);
            _startedTasksNoActionTimeout = GetTimeoutFromConfiguration(configuration, "StartedTasksNoActionTimeout", 5);
            _startedTasksAbsoluteTimeout = GetTimeoutFromConfiguration(configuration, "StartedTasksAbsoluteTimeout", 10);
        }

        public void Execute()
        {
            if(Enabled)
            {
                FailTimedOutTasks();
            }
            else
            {
                GetFailedTasks();
            }
        }

        private List<TaskBase> GetFailedTasks()
        {
            var tasksToFail = _taskBundleService.GetAllStartedTasks()
                .Where(task => (DateTime.Now - task.processingStartedDate) > _startedTasksAbsoluteTimeout
                               || (DateTime.Now - task.lastUpdateDate) > _startedTasksNoActionTimeout)
                .ToList();
            tasksToFail.AddRange(_taskBundleService.GetMoveBundles()
                .Where(taskBundle => (DateTime.Now - taskBundle.creationDate) > _moveTaskAbsoluteTimeout)
                .SelectMany(taskBundle => taskBundle.tasks)
                .ToList());
            tasksToFail = tasksToFail.Distinct().ToList();
            _logger.LogDebug("Timed out tasks to fail: {0}", string.Join(";", tasksToFail));
            return tasksToFail;
        }

        public void FailTimedOutTasks()
        {
            _logger.LogDebug("Failing tasks");
            GetFailedTasks().ForEach(FailTask);
        }

        public void FailTask(TaskBase task)
        {
            _logger.LogWarning("Failing task, because of timeout task: {0}", task);
            _taskBundleService.FailTask(task.taskId);
            ReturnTotes(task);
        }

        private void ReturnTotes(TaskBase task)
        {
            switch (task)
            {

                case PickTask pickTask:
                    ReturnTote(pickTask.sourceTote.toteId);
                    break;
                case MoveTask moveTask:
                    ReturnTote(moveTask.toteId);
                    break;
            }
        }

        private void ReturnTote(string toteId)
        {
            try
            {
                _returnToteHandler.ReturnTote(toteId);
            }
            catch (NoDeviceCanHandleTransferException ex)
            {
                _logger.LogError(ex,"No device could handle transfer for timed out tote");
            }
        }
        
        private static TimeSpan GetTimeoutFromConfiguration(IConfiguration configuration, string variableName, int defaultValue)
        {
            var timeout = configuration[$"Watchdog:TaskBundleWatchdog:{variableName}"];
            return string.IsNullOrEmpty(timeout)
                ? TimeSpan.FromMinutes(defaultValue)
                : TimeSpan.FromMinutes(int.Parse(timeout));
        }
    }
}