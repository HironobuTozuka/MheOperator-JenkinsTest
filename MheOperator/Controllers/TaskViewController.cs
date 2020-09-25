using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Common.Models.Task;
using RcsLogic.Services;
using RcsLogic.Watchdog;
using RcsLogic;


namespace MheOperator.Controllers
{
    public class TaskViewController : Controller
    {
        private readonly ILogger<TaskViewController> _logger;
        private TaskBundleService _taskBundleService;
        private readonly TaskBundleWatchdog _taskBundleWatchdog;


        public TaskViewController(ILoggerFactory loggerFactory, TaskBundleService taskBundleService, TaskBundleWatchdog taskBundleWatchdog)
        {
            _logger = loggerFactory.CreateLogger<TaskViewController>();
            _taskBundleService = taskBundleService;
            _taskBundleWatchdog = taskBundleWatchdog;
        }

        public IActionResult Index()
        {
            return View(_taskBundleService.ToList());
        }

        public IActionResult CompleteTask(string taskId)
        {
            var id = new TaskId(taskId);
            var task = _taskBundleService.GetTaskBase(id);
            task.taskStatus = RcsTaskStatus.Complete;
            if (task is PickTask pickTask)
            {
                _taskBundleService.CompleteTask(id, pickTask.quantity, 0);
            }
            else
            {
                _taskBundleService.CompleteTask(id);
            }

            return View("Index", _taskBundleService.ToList());
        }
        
        public IActionResult FailTask(string taskId)
        {
            var id = new TaskId(taskId);
            var task = _taskBundleService.GetTaskBase(id);
            _taskBundleWatchdog.FailTask(task);

            return View("Index", _taskBundleService.ToList());
        }

        public IActionResult DeleteTaskBundle(string taskBundleId)
        {
            var taskBundle = _taskBundleService.ToList()
                .First(taskBundle => taskBundle.taskBundleId.ToString() == taskBundleId);
            _taskBundleService.Remove(taskBundle);

            return View("Index", _taskBundleService.ToList());
        }
    }
}