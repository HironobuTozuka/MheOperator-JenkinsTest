using Common;
using Common.Models;
using Common.Models.Tote;
using Data;
using RcsLogic.Models;
using RcsLogic.Services;

namespace RcsLogic
{
    public class MoveTaskCompletingScanNotificationListener : IScanNotificationListener
    {
        private readonly TaskBundleService _taskBundleService;
        private readonly ToteRepository _toteRepository;

        public MoveTaskCompletingScanNotificationListener(TaskBundleService taskBundleService, ToteRepository toteRepository)
        {
            _taskBundleService = taskBundleService;
            _toteRepository = toteRepository;
        }

        public void ProcessScanNotification(ScanNotificationModel scanNotification)
        {
            var deletedTask = _taskBundleService.CompleteMoveTaskIfExists(scanNotification.ToteBarcode, scanNotification.LocationId);
            if (deletedTask == null) return;
            if (deletedTask.toteId.Contains(Barcode.NoRead))
            {
                _toteRepository.Remove(_toteRepository.GetToteByBarcode(deletedTask.toteId));
            }
        }
    }
}