using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RcsLogic.Watchdog;

namespace MheOperator.StoreManagementApi.Controllers
{
    [Route("api/internal")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        private readonly ILogger<FeatureController> _logger;
        private readonly ToteLocationWatchdog _toteLocationWatchdog;
        private readonly TaskBundleWatchdog _taskBundleWatchdog;
        private readonly ToteLocationUnknownWatchdog _toteLocationUnknownWatchdog;

        public FeatureController(ILogger<FeatureController> logger,
            ToteLocationWatchdog toteLocationWatchdog,
            TaskBundleWatchdog taskBundleWatchdog, ToteLocationUnknownWatchdog toteLocationUnknownWatchdog)
        {
            _logger = logger;
            _toteLocationWatchdog = toteLocationWatchdog;
            _taskBundleWatchdog = taskBundleWatchdog;
            _toteLocationUnknownWatchdog = toteLocationUnknownWatchdog;
        }
        
        [HttpPost("tote_watchdog:disable")]
        public IActionResult ToteLocationWatchdogDisable()
        {
            _logger.LogInformation("Got request to disable ToteLocationWatchdog");
            _toteLocationWatchdog.Enabled = false;
            
            return StatusCode(StatusCodes.Status200OK);
        }
        
        [HttpPost("tote_watchdog:enable")]
        public IActionResult ToteLocationWatchdogEnable()
        {
            _logger.LogInformation("Got request to enable ToteLocationWatchdog");
            _toteLocationWatchdog.Enabled = true;
            
            return StatusCode(StatusCodes.Status200OK);
        }
        
        [HttpPost("tote_watchdog:execute")]
        public IActionResult ToteLocationWatchdogExecute()
        {
            _logger.LogInformation("Got request to execute ToteLocationWatchdog");
            _toteLocationWatchdog.MoveBackTimedOutTotes();
            
            return StatusCode(StatusCodes.Status200OK);
        }
        
        [HttpPost("task_watchdog:disable")]
        public IActionResult _taskBundleWatchdogDisable()
        {
            _logger.LogInformation("Got request to disable TaskBundleWatchdog");
            _taskBundleWatchdog.Enabled = false;
            
            return StatusCode(StatusCodes.Status200OK);
        }
        
        [HttpPost("task_watchdog:enable")]
        public IActionResult _taskBundleWatchdogEnable()
        {
            _logger.LogInformation("Got request to enable TaskBundleWatchdog");
            _taskBundleWatchdog.Enabled = true;
            
            return StatusCode(StatusCodes.Status200OK);
        }
        
        [HttpPost("task_watchdog:execute")]
        public IActionResult _taskBundleWatchdogExecute()
        {
            _logger.LogInformation("Got request to execute TaskBundleWatchdog");
            _taskBundleWatchdog.FailTimedOutTasks();
            
            return StatusCode(StatusCodes.Status200OK);
        }
        
        [HttpPost("tote_unknown_location_watchdog:disable")]
        public IActionResult ToteUnknownLocationWatchdogDisable()
        {
            _logger.LogInformation("Got request to disable ToteLocationWatchdog");
            _toteLocationUnknownWatchdog.Enabled = false;
            
            return StatusCode(StatusCodes.Status200OK);
        }
        
        [HttpPost("tote_unknown_location_watchdog:enable")]
        public IActionResult ToteUnknownLocationWatchdogEnable()
        {
            _logger.LogInformation("Got request to enable ToteLocationWatchdog");
            _toteLocationUnknownWatchdog.Enabled = true;
            
            return StatusCode(StatusCodes.Status200OK);
        }
        
        [HttpPost("tote_unknown_location_watchdog:execute")]
        public IActionResult ToteUnknownLocationWatchdogExecute()
        {
            _logger.LogInformation("Got request to execute ToteLocationWatchdog");
            _toteLocationUnknownWatchdog.ForceExecute();
            
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}