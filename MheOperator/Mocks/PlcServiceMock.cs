using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using Common;
using Common.Models;
using Common.Models.Gate;
using Common.Models.Led;
using Common.Models.Plc;
using Common.Models.Tote;
using Microsoft.Extensions.Logging;
using PlcCommunicationService.Models;
using PlcRequestQueueService;
using RcsLogic;

namespace Tests
{
    public class PlcServiceMock : IPlcService
    {
        private TransferRequestDoneListenerRegistry _transferRequestDoneListenerRegistry;
        private PickRequestDoneListenerRegistry _pickRequestDoneListenerRegistry;
        private readonly ScanNotificationListenerRegistry _scanNotificationListenerRegistry;
        bool _scanNotificationRead = true;
        bool _TransferRequestConfRead = true;
        bool _PickRequestConfRead = true;
        ILogger<PlcServiceMock> _logger;
        bool picking;
        private int counter = 0;
        private readonly Timer craneIdleTimer;
        private IPlcNotificationListener _plcNotificationListener;
        public ushort returnedRobotSortCode { get; set; } = 1;
        public ushort returnedSystemSortCode { get; set; } = 1;
        public bool craneNoResponse { get; set; } = false;
        public bool changeNoreadToReadOnTransferDone { get; set; } = false;
        public bool changeNoreadToReadOnScan { get; set; } = false;
        public bool changeReadToNoreadOnTransfer { get; set; } = false;
        public bool changeReadToNoreadOnScan { get; set; } = false;

        public PlcServiceMock(ILoggerFactory loggerFactory,
            ScanNotificationListenerRegistry scanNotificationListenerRegistry, 
            TransferRequestDoneListenerRegistry transferRequestDoneListenerRegistry,
            PickRequestDoneListenerRegistry pickRequestDoneListenerRegistry)
        {
            _transferRequestDoneListenerRegistry = transferRequestDoneListenerRegistry;
            _pickRequestDoneListenerRegistry = pickRequestDoneListenerRegistry;
            _scanNotificationListenerRegistry = scanNotificationListenerRegistry;
            _logger = loggerFactory.CreateLogger<PlcServiceMock>();
            craneIdleTimer = new Timer(200);
            craneIdleTimer.AutoReset = true;
            craneIdleTimer.Elapsed+= CraneIdleTimerOnElapsed;
            craneIdleTimer.Enabled = true;
        }

        public void Subscribe(IPlcNotificationListener plcNotificationListener)
        {
            _plcNotificationListener = plcNotificationListener;
        }

        private void CraneIdleTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            _plcNotificationListener?.NotifyListener(new PlcNotification()
            {
                NotificationType = PlcNotificationType.CraneIdle,
                Parameters = new PlcNotificationParameters(){ CraneId = "CA_P"}
            });
            _plcNotificationListener?.NotifyListener(new PlcNotification()
            {
                NotificationType = PlcNotificationType.CraneIdle,
                Parameters = new PlcNotificationParameters(){ CraneId = "CB_P"}
            });
        }

        public void Connect()
        {

        }
        public void SwitchLed(LedId ledId, LedState ledState)
        {
            switch (ledId)
            {
                case LedId.Led1:
                case LedId.Led2:
                case LedId.Led3:
                    _logger.LogInformation("Led number {0} state: {0} !", ledId, ledState);
                    break;
                default:
                    throw new Exception("Not implemented Led ID");
            }
        }

        public bool IsRobotPicking()
        {
            return picking;
        }

        public async Task AwaitRobotConnected()
        {

        }
        public void OpenGate(GateDescription gate)
        {

        }

        public void ResetPickRequestConfRed()
        {

        }

        public bool IsRobotConnected()
        {
            return true;
        }
        public bool IsSystemConnected()
        {
            return true;
        }

        public void CloseGate(GateDescription gate)
        {

        }

        public void RequestPick(RobotPickRequestBundleModel requestBundle)
        {
            picking = true;
            _ = Task.Run(() =>
            {
                MockPickRequestDone(requestBundle.pickRequest);
                picking = false;
                
            });


        }
        public void RequestTransfer(TransferRequestModel transferRequest)
        {
            //TODO check acks
            //   if (!_TransferRequestConfRead) throw new Exception("Transfer request conf ack not sent");
            _ = Task.Run(() => { ProcessTransferRequest(transferRequest); });


        }

        public bool IsConveyorOccupied(string locationId)
        {
            
            counter++;
            if (counter > 3) counter = 0;
            return counter > 2;
        }

        public bool IsPlcInExecute()
        {
            return true;
        }

        private void ProcessTransferRequest(TransferRequestModel transferRequest)
        {
            _TransferRequestConfRead = false;
            if (craneNoResponse) return;
            System.Threading.Thread.Sleep(150);

            MockTransferDone(transferRequest.toteRequest1, transferRequest.toteRequest2);

            if(transferRequest.toteRequest1!=null) ProcessToteRequest(transferRequest.toteRequest1);
            if(transferRequest.toteRequest2!=null) ProcessToteRequest(transferRequest.toteRequest2);
        }

        private void ProcessToteRequest(ToteTransferRequestModel transferRequest)
        {
            if (changeNoreadToReadOnScan && transferRequest.ToteBarcode.Contains(Barcode.NoRead))
                transferRequest.ToteBarcode = "0000001";
            if (changeReadToNoreadOnScan)
                transferRequest.ToteBarcode = "NOREAD";
            if (transferRequest.DestLocationId.Contains("CNV"))
            {
                System.Threading.Thread.Sleep(250);
                if (transferRequest.DestLocationId == "CNV1")
                {
                    MockScanNotification("CNV1_3", transferRequest.ToteBarcode,
                        transferRequest.ToteType);
                }
                else if (transferRequest.DestLocationId == "CNV2_2")
                {
                    MockScanNotification("CNV2_2", transferRequest.ToteBarcode,
                        transferRequest.ToteType);
                }
                else
                {
                    MockScanNotification(transferRequest.DestLocationId, transferRequest.ToteBarcode,
                        transferRequest.ToteType);
                }
            }

            if (transferRequest.DestLocationId.Contains("LOAD1"))
            {
                System.Threading.Thread.Sleep(200);
                if (transferRequest.DestLocationId == "LOAD1_1")
                {
                    MockScanNotification("LOAD1_2", transferRequest.ToteBarcode,
                        transferRequest.ToteType);
                }

                if (transferRequest.DestLocationId == "LOAD1_3")
                {
                    MockScanNotification("LOAD1_4", transferRequest.ToteBarcode,
                        transferRequest.ToteType);
                }
            }

            if (transferRequest.DestLocationId.Contains("ORDER") || transferRequest.DestLocationId.Contains("RPP") || transferRequest.DestLocationId.Contains("CA_P") || transferRequest.DestLocationId.Contains("CB_P"))
            {
                System.Threading.Thread.Sleep(200);
                MockScanNotification(transferRequest.DestLocationId, transferRequest.ToteBarcode,
                    transferRequest.ToteType);
            }
        }

        public void TransferRequestConfRed()
        {
            _TransferRequestConfRead = true;
            _logger.LogInformation("Transfer conf read");
        }

        public void PickRequestConfRed()
        {
            _PickRequestConfRead = true;
            _logger.LogInformation("Pick conf read");
        }
        public void ScanNotificationRed()
        {

            _scanNotificationRead = true;
            _logger.LogInformation("Scan notification conf read");
        }
        
        public void MockScanNotification(string locationId, string toteBarcode, ToteType toteType)
        {
            MockScanNotification(locationId,toteBarcode,new RequestToteType(toteType));

        }

        public void MockScanNotification(string locationId, string toteBarcode, RequestToteType toteType)
        {
            _scanNotificationRead = false;
            _scanNotificationListenerRegistry.NotifyListeners(new ScanNotificationModel()
            {
                LocationId = locationId,
                ToteBarcode = toteBarcode,
                ToteRotation = ToteRotation.normal,
                ToteType = toteType
            });

        }

        public void MockTransferDone(ToteTransferRequestModel transferRequest1, ToteTransferRequestModel transferRequest2)
        {
            _TransferRequestConfRead = false;
            System.Threading.Thread.Sleep(10);

            var done2 = MockTransferRequestDoneModel(transferRequest2);
            var done1 = MockTransferRequestDoneModel(transferRequest1);

            if (done1 != null)
                _transferRequestDoneListenerRegistry.NotifyListeners(new TransferRequestDoneModel(done1, null));
            if (done2 != null)
                _transferRequestDoneListenerRegistry.NotifyListeners(new TransferRequestDoneModel(null, done2));

        }

        public ToteTransferRequestDoneModel MockTransferRequestDoneModel(ToteTransferRequestModel transferRequest)
        {
            if (transferRequest == null || transferRequest.ToteBarcode == null || transferRequest.ToteBarcode == "") return null;
            if (changeNoreadToReadOnTransferDone && transferRequest.ToteBarcode.Contains(Barcode.NoRead))
                transferRequest.ToteBarcode = "0000001";
            if (changeReadToNoreadOnTransfer)
                transferRequest.ToteBarcode = "NOREAD";
            return new ToteTransferRequestDoneModel()
            {
                requestId = transferRequest.Id,
                actualDestLocationId = transferRequest.DestLocationId,
                sourceToteBarcode = transferRequest.ToteBarcode,
                requestedDestLocationId = transferRequest.DestLocationId,
                sourceLocationId = transferRequest.SourceLocationId,
                sortCode = returnedSystemSortCode
            };
        }

        public void MockPickRequestDone(RobotPickRequestModel pickRequest)
        {
            
            if(pickRequest!=null)_pickRequestDoneListenerRegistry.NotifyListeners
            (
                new PickRequestDoneModel()
                {
                    actualPickCount = returnedRobotSortCode == 1 ? pickRequest.pickCount : (ushort)0,
                    requestedPickCount = pickRequest.pickCount,
                    partName = pickRequest.partName,
                    sortCode = returnedRobotSortCode,
                    requestId = pickRequest.id,
                    destLocationId = pickRequest.dest.locationId,
                    destToteBarcode = pickRequest.dest.barcode,
                    sourceLocationId = pickRequest.source.locationId,
                    sourceToteBarcode = pickRequest.source.barcode,
                }
            );

        }
    }
}
