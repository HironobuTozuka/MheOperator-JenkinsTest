using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Models;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.RcsController.Exceptions;
using RcsLogic.RcsController.ToteCommand;

namespace RcsLogic.RcsController
{
    public class RcsController : IReturnToteHandler, IScanNotificationListener,
        ITaskBundleUpdatedListener, ITaskRemovedListener, ITaskBundleCanceledListener
    {
        private readonly ILogger<RcsController> _logger;
        private readonly DeviceRegistry _deviceRegistry;
        private readonly Recovery.RecoveryHandler _recoveryHandler;
        private readonly LocationRepository _locationRepository;
        private readonly ToteRepository _toteRepository;
        private readonly ToteCommandDecisionTree _toteCommandDecisionTree;

        public RcsController(
            ILoggerFactory loggerFactory,
            DeviceRegistry deviceRegistry,
            Recovery.RecoveryHandler recoveryHandler,
            LocationRepository locationRepository,
            ToteRepository toteRepository,
            ToteCommandDecisionTree toteCommandDecisionTree)
        {
            _logger = loggerFactory.CreateLogger<RcsController>();
            _deviceRegistry = deviceRegistry;
            _recoveryHandler = recoveryHandler;
            _locationRepository = locationRepository;
            _toteRepository = toteRepository;
            _toteCommandDecisionTree = toteCommandDecisionTree;
        } 

        public void ProcessScanNotification(ScanNotificationModel scanNotification)
        {
            try
            {
                _logger.LogInformation("RCS controller handling scan notification: {1}", scanNotification);
                
                var scanLocation = _locationRepository.GetLocationByPlcId(scanNotification.LocationId);
                var tote = GetTote(scanNotification, scanLocation);
                
                _logger.LogDebug("RCS controller found tote: {1} on locations {2} for scan {3}", 
                    tote, scanLocation, scanNotification);

                _toteCommandDecisionTree.CreateCommand(scanLocation, tote, scanNotification.ToteRotation).Execute();
            }
            catch (RoutingNotImplementedException exception)
            {
                _logger.LogWarning(exception, "Routing not implemented for scan notification: {1}", scanNotification);
            }
            catch (NoDeviceCanHandleTransferException exception)
            {
                _logger.LogWarning(exception, "No device can handle transfer: {1} created for scan notification: {2}",
                    exception.Transfer, scanNotification);
            }
        }

        public void HandleFailedTransferDoneRequest(TransferResult result)
        {
            var requestDoneModel = result.RequestDone;
            _logger.LogInformation("Handling failed request done: {1}", requestDoneModel);

            try
            {
                var device = _deviceRegistry.ChooseDeviceForTransferDone(requestDoneModel);
                var transfer = _recoveryHandler.Strategy(result.SystemSortCode).Recover(device, result);
                device.Execute(transfer);
            }
            catch (NoDeviceCanHandleTransferDoneException exception)
            {
                _logger.LogWarning(exception, "No device can handle request done: {1}", requestDoneModel);
            }
        }

        public void HandleRequestTimeout(TimedOutTransfer timedOutTransfer)
        {
            _logger.LogDebug("Handling transfer request timeout on device: {1}, request {2}",
                timedOutTransfer.device.DeviceId, timedOutTransfer.transfer);
            _recoveryHandler.Strategy(timedOutTransfer).Recover(timedOutTransfer);
        }


        public void ReturnTote(string toteId)
        {
            var tote = _toteRepository.GetToteByBarcode(toteId);
            if (tote.location == null)
            {
                _logger.LogDebug("Skipping returning tote: {0}, it's location was unknown");
                return;
            }
            _toteCommandDecisionTree.CreateReturnCommand(tote.location, tote, ToteRotation.unknown).Execute();
        }
        
        private Tote GetTote(ScanNotificationModel scanNotification, Location location)
        {
            Tote maybeTote = null;
            if (location?.zone?.function == LocationFunction.LoadingGate)
                maybeTote = _toteRepository.GetToteOnLocation(location.id);
            
            return _toteRepository.GetToteByBarcode(scanNotification.ToteBarcode) 
                   ?? maybeTote
                   ?? new Tote()
                   {
                       toteBarcode = scanNotification.ToteBarcode, 
                       type = new ToteType(
                           scanNotification.ToteType.ToteHeight, 
                           scanNotification.ToteType.TotePartitioning)
                   };
        }

       
        
        public void HandleTaskBundleUpdated(TaskBundle taskBundle, List<TaskBase> oldTasks, List<TaskBase> newTasks)
        {
            oldTasks.OfType<PickTask>().GroupBy(task => task.destTote.toteId)
                .Select(group => group.First()).ToList().
                ForEach(task => RemovePriorityTote(task.destTote.toteId));

        }

        public void HandleTaskRemoved(TaskBase task)
        {
            RemovePlannedMovesForTask(task);
        }
        
        public void HandleTaskBundleCanceled(TaskBundle taskBundle)
        {
            ReleaseUnusedTotes();
        }
        
        private void RemovePriorityTote(string toteId)
        {
            _deviceRegistry.GetDevicesOfType<ITotePrioritizingDevice>().ToList()
                .ForEach(device => device.RemovePriorityTote(toteId));
        }
        
        private void RemovePlannedMovesForTask(TaskBase task)
        {
            _deviceRegistry.GetDevicesOfType<ITransferPlanningDevice>().ToList().
                ForEach(device => device.RemovePlannedTransfers(task));
        }
        
        private void ReleaseUnusedTotes()
        {
            _deviceRegistry.GetDevicesOfType<IToteReleasingDevice>().ToList().ForEach(device => device.ReleaseTotes());
        }
    }
}