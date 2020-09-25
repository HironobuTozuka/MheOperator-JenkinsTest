using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RcsLogic.Crane;
using RcsLogic.Models;
using RcsLogic.Services;

namespace RcsLogic.RcsController.Recovery
{
    public class CraneRequestTimeoutRecovery : IRequestTimeoutStrategy
    {
        private readonly TaskBundleService _taskBundleService;
        private ILogger<CraneRequestTimeoutRecovery> _logger;

        public CraneRequestTimeoutRecovery(TaskBundleService taskBundleService, ILoggerFactory loggerFactory)
        {
            _taskBundleService = taskBundleService;
            _logger = loggerFactory.CreateLogger<CraneRequestTimeoutRecovery>();
        }
        
        public void Recover(TimedOutTransfer timedOutTransfer)
        {
            _logger.LogInformation("Failing task on crane request timout: {0}", timedOutTransfer);
            _taskBundleService.FailTask(timedOutTransfer.transfer.task.taskId);
            var craneDevice = timedOutTransfer.device as CraneDevice;
            craneDevice?.FailCraneRequest(timedOutTransfer.transfer);
        }
    }
}