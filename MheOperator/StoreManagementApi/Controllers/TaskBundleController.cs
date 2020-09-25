using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Common.Exceptions;
using Common.Models.Task;
using MheOperator.StoreManagementApi.Models;
using Microsoft.AspNetCore.Http;
using MheOperator.StoreManagementApi.Models.Exceptions;
using MheOperator.StoreManagementApi.Models.TaskController;
using RcsLogic.Services;

namespace MheOperator.StoreManagementApi.Controllers
{
    [Route("api/internal")]
    [ApiController]
    public class TaskBundleController : ControllerBase
    {
        private readonly ILogger<TaskBundleController> _logger;
        private readonly TaskBundleService _taskBundleService;


        public TaskBundleController(ILogger<TaskBundleController> logger, TaskBundleService taskBundleService)
        {
            _logger = logger;
            _taskBundleService = taskBundleService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("task-bundle:execute")]
        public IActionResult PostTasks([FromBody] TaskBundleDataModel taskBundleDataModel)
        {

            try
            {
                //Add tasks to Zone Controller
                _taskBundleService.AddTaskBundle(taskBundleDataModel.toTaskBundle());

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (SmHttpControllerException ex)
            {
                return StatusCode(ex.HttpStatusCode, ApiErrorFactory.CreateApiError(ex));
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("task-bundle:cancel")]
        public IActionResult CancelTasks([FromBody] CancelTaskBundleDataModel cancelTaskBundleDataModel)
        {
            try
            {
                Task.Run(() => _taskBundleService.CancelTaskBundle(new TaskBundleId(cancelTaskBundleDataModel.TaskBundleId)));

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (SmHttpControllerException ex)
            {
                return StatusCode(ex.HttpStatusCode, ApiErrorFactory.CreateApiError(ex));
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("task-bundle:update")]
        public IActionResult UpdateTasks([FromBody] TaskBundleDataModel taskBundleDataModel)
        {
            try
            {
                _taskBundleService.UpdateTaskBundle(taskBundleDataModel.toTaskBundle());

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (SmHttpControllerException ex)
            {
                return StatusCode(ex.HttpStatusCode, ApiErrorFactory.CreateApiError(ex));
            }

        }

    }
}