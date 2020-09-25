using System.Threading.Tasks;
using Common.Models;
using Common.Models.Plc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlcCommunicationService.Models;
using PlcRequestQueueService;

namespace PlcCommunicationService.SystemPlc
{
    public class PlcNotificationListener : PlcCommunicationService.PlcNotificationListener
    {
        private readonly ScanNotificationProducer _scanNotificationProducer;
        private readonly TransferRequestDoneProducer _transferRequestDoneProducer;
        private readonly PlcNotificationProducer _plcNotificationProducer;

        public PlcNotificationListener(ILoggerFactory loggerFactory, IConfiguration configuration,
            IMoveRequestRedStateChangeListener moveRequestRedStateChangeListener)
            : base(loggerFactory.CreateLogger<PlcNotificationListener>(), moveRequestRedStateChangeListener)
        {
            _scanNotificationProducer = new ScanNotificationProducer(loggerFactory, configuration);
            _transferRequestDoneProducer = new TransferRequestDoneProducer(loggerFactory, configuration);
            _plcNotificationProducer = new PlcNotificationProducer(loggerFactory, configuration);
        }

        protected override async Task HandlePlcSpecificNotification(PlcReadNotification plcReadNotification,
            PlcCommunicationService.OpcClient opcClient)
        {
            switch (plcReadNotification.Key)
            {
                case "ReadScanNotification":
                    Logger.LogInformation("ReadScanNotification: {0}", plcReadNotification.Value);
                    if (((InSignals) opcClient.InSignals).GetReadScanNotification())
                    {
                        try
                        {
                            var scanNotificationTask = ((InSignals) opcClient.InSignals).GetScanNotificationAsync();
                            scanNotificationTask.Wait();
                            var scanNotification = scanNotificationTask.Result;
                            Logger.LogInformation("Scan notification: " + scanNotification);
                            _scanNotificationProducer.Produce(scanNotification.ToScanNotificationModel()).Wait();
                            await ((OutSignals) opcClient.OutSignals).SetScanNotificationRead(true);
                        }
                        catch
                        {
                            Logger.LogInformation("Unable to read ScanNotification from PLC");
                        }
                    }
                    else
                    {
                        await ((OutSignals) opcClient.OutSignals).SetScanNotificationRead(false);
                    }

                    break;
                default:
                    if (plcReadNotification.Key.Contains("_Idle") && plcReadNotification.Value)
                    {
                        var idleDeviceId = plcReadNotification.Key.Replace("_Idle", null);
                        Logger.LogTrace("Received Idle message from {0}", idleDeviceId);
                        ProducePlcNotification(idleDeviceId);
                    }
                    break;
            }
        }

        private void ProducePlcNotification(string craneA)
        {
            _plcNotificationProducer.Produce(new PlcNotification()
            {
                NotificationType = PlcNotificationType.CraneIdle,
                Parameters = new PlcNotificationParameters() {CraneId = craneA}
            });
        }

        protected override void MoveRequestDone(IMoveRequestConf moveRequestConf)
        {
            ToteTransferRequestDoneModel transferRequest1Done = null;
            ToteTransferRequestDoneModel transferRequest2Done = null;

            if (moveRequestConf.ToteRequest1Conf != null)
                transferRequest1Done = moveRequestConf.ToteRequest1Conf.ToTransferRequestConf();
            if (moveRequestConf.ToteRequest2Conf != null)
                transferRequest2Done = moveRequestConf.ToteRequest2Conf.ToTransferRequestConf();


            var transferRequestDone = new TransferRequestDoneModel(transferRequest1Done, transferRequest2Done);

            _transferRequestDoneProducer.Produce(transferRequestDone).Wait();
        }
    }
}