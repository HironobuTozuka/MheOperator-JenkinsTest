using System.Linq;
using Common.Models.Plc;
using Microsoft.Extensions.Logging;
using PlcRequestQueueService;
using RcsLogic.Crane;
using RcsLogic.Gates;
using RcsLogic.RcsController;
using RcsLogic.Robot;
using RcsLogic.Services;
using RcsLogic.Watchdog;

namespace RcsLogic
{
    public class RcsInitializer
    {
        public ScanNotificationListenerRegistry ScanNotificationListenerRegistry { get; private set; }

        public RcsInitializer(
            ILoggerFactory loggerFactory,
            TaskBundleService taskBundles,
            TotesReadyForPicking totesReadyForPicking,
            TransferRequestDoneListenerRegistry transferRequestDoneListenerRegistry, 
            PickRequestDoneListenerRegistry pickRequestDoneListenerRegistry,
            ScanNotificationListenerRegistry scanNotificationListenerRegistry,
            MoveTaskCompletingScanNotificationListener moveTaskCompletingScanNotificationListener,
            IKafkaConsumerGroup kafkaConsumerGroup,
            PlcNotificationListenerRegistry plcNotificationListenerRegistry,
            DeviceRegistry deviceRegistry,
            RcsController.RcsController rcsController,
            TransferRequestDoneWatcher transferRequestDoneWatcher,
            ToteLocationUpdatingScanNotificationListener toteLocationUpdatingScanNotificationListener,
            StoreManagementNotifyingTransferDoneListener storeManagementNotifyingTransferDoneListener,
            ToteBarcodeReadOnRequestForNoReadTransferDoneListener toteBarcodeReadOnRequestForNoReadTransferDoneListener,
            WatchdogExecutor watchdogExecutor,
            ToteLocationWatchdog toteLocationWatchdog,
            TaskBundleWatchdog taskBundleWatchdog,
            ToteLocationUnknownWatchdog toteLocationUnknownWatchdog
            )
        {
            //Be careful when modifying this code!!!
            //Sequence of registering listeners matters
            ScanNotificationListenerRegistry = scanNotificationListenerRegistry;
            deviceRegistry.GetDevicesOfType<RobotDevice>().ForEach(pickRequestDoneListenerRegistry.RegisterListener);
            
            deviceRegistry.GetDevicesOfType<CraneDevice>().ForEach(taskBundles.RegisterTaskBundleAddedListener);
            deviceRegistry.GetDevicesOfType<LoadingGate>().ForEach(taskBundles.RegisterTaskBundleAddedListener);
            
            deviceRegistry.GetDevicesOfType<CraneDevice>().ForEach(taskBundles.RegisterTaskBundleRemovedListener);
            
            //TODO
            deviceRegistry.GetDevicesOfType<RobotDevice>().ForEach(it => it.RegisterReturnHandler(rcsController 
                ));
            if(deviceRegistry.GetDevicesOfType<CraneDevice>().Any(device => device.DeviceId.id.Contains("CB_P")))
                deviceRegistry.GetDevicesOfType<RobotDevice>().ForEach(it => it.RegisterTotePrioritisingDevice(
                deviceRegistry.GetDevicesOfType<CraneDevice>().First(device => device.DeviceId.id.Contains("CB_P"))));
            deviceRegistry.GetDevicesOfType<OrderGates>().ForEach(it => it.RegisterReturnHandler(rcsController));
            if(deviceRegistry.GetDevicesOfType<IToteShakingDevice>().Any())
                deviceRegistry.GetDevicesOfType<RobotDevice>().ForEach(it =>
                it.RegisterToteShakingDevice(deviceRegistry.GetDevicesOfType<IToteShakingDevice>().First()));
            
            transferRequestDoneListenerRegistry.RegisterListener(transferRequestDoneWatcher);
            transferRequestDoneListenerRegistry.RegisterListener(toteBarcodeReadOnRequestForNoReadTransferDoneListener);
            transferRequestDoneListenerRegistry.RegisterListener(storeManagementNotifyingTransferDoneListener);
            deviceRegistry.GetDevicesOfType<CraneDevice>().ForEach(transferRequestDoneListenerRegistry.RegisterListener);

            scanNotificationListenerRegistry.RegisterListener(toteLocationUpdatingScanNotificationListener);
            scanNotificationListenerRegistry.RegisterListener(rcsController);
            scanNotificationListenerRegistry.RegisterListener(totesReadyForPicking);
            scanNotificationListenerRegistry.RegisterListener(moveTaskCompletingScanNotificationListener);
            
            plcNotificationListenerRegistry.RegisterListener(new CraneIdleListener(loggerFactory, rcsController, deviceRegistry), PlcNotificationType.CraneIdle);
            
            taskBundles.RegisterTaskRemovedListener(rcsController);
            taskBundles.RegisterTaskBundleCanceledListener(rcsController);
            taskBundles.RegisterTaskBundleUpdatedListener(rcsController);

            //Connect after all subscribers are already listening
            kafkaConsumerGroup.Subscribe(new ScanNotificationConsumer(loggerFactory, scanNotificationListenerRegistry));
            kafkaConsumerGroup.Subscribe(new TransferRequestDoneConsumer(loggerFactory, transferRequestDoneListenerRegistry));
            kafkaConsumerGroup.Subscribe(new PickRequestDoneConsumer(loggerFactory, pickRequestDoneListenerRegistry));
            kafkaConsumerGroup.Subscribe(new PlcNotificationConsumer(loggerFactory, plcNotificationListenerRegistry));
            
            watchdogExecutor.RegisterWatchdog(toteLocationWatchdog);
            watchdogExecutor.RegisterWatchdog(taskBundleWatchdog);
            watchdogExecutor.RegisterWatchdog(toteLocationUnknownWatchdog);
        }
        
    }
}