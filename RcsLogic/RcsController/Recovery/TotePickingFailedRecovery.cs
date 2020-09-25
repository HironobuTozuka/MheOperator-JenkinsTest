using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Services;

namespace RcsLogic.RcsController.Recovery
{
    public class TotePickingFailedRecovery : IRecoveryStrategy
    {
        private readonly TaskBundleService _taskBundleService;
        private ILogger<TotePickingFailedRecovery> _logger;

        public TotePickingFailedRecovery(TaskBundleService taskBundleService, ILoggerFactory loggerFactory)
        {
            _taskBundleService = taskBundleService;
            _logger = loggerFactory.CreateLogger<TotePickingFailedRecovery>();
        }

        public List<Transfer> Recover(ITransferCompletingDevice device, TransferResult result)
        {
            _logger.LogInformation("Failing task for transfer result: ", result);
            _taskBundleService.FailTask(result.RequestDone.requestId.GetTaskId());
            return new List<Transfer>();
        }
    }
}