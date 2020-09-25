using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Models;
using Common.Models.Location;
using Common.Models.Plc;
using Common.Models.Task;
using Common.Models.Tote;
using Common.Models.Transfer;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Models.Device;
using RcsLogic.RcsController.Recovery;
using RcsLogic.Robot;
using RcsLogic.Services;
using static RcsLogic.CraneState.Shelf;
test
namespace RcsLogic.Crane
{
    public enum CraneType
    {
        SingleShelved = 1,
        DoubleShelved = 2
    }

    public class CraneDevice : Device, ITransferRequestDoneListener,  ITaskBundleAddedListener, 
        ITaskBundleRemovedListener, ITransferCompletingDevice, ITotePrioritizingDevice, ITransferPlanningDevice
    {
        private readonly ILogger<CraneDevice> _logger;

        public CraneState CraneState { get; }
        private readonly TransferCollection _transfers;

        private readonly RoutingService _routingService;
        private readonly IPlcService _plcService;
        private readonly IStoreManagementClient _storeManagementClient;
        private readonly ToteRepository _toteRepository;
        private readonly CraneTransfersPlanner _craneTransfersPlanner;
        private readonly BlockedTotesSolver _blockedTotesSolver;
        private readonly TotesReadyForPicking _totesReadyForPicking;

        public CraneDevice(DeviceId deviceId,
            IPlcService plcService,
            ILoggerFactory loggerFactory,
            CraneType type,
            TaskBundleService tasks,
            RoutingService routingService,
            IStoreManagementClient storeManagementClient,
            ToteRepository toteRepository,
            LocationRepository locationRepository,
            LocationService locationService,
            LocationStatus locationStatus,
            TotesReadyForPicking totesReadyForPicking) : base(tasks, deviceId)
        {
            _transfers = new TransferCollection(loggerFactory, deviceId, locationStatus, totesReadyForPicking);
            CraneState = new CraneState(type, _deviceId, loggerFactory);
            _logger = loggerFactory.CreateLogger<CraneDevice>();
            _plcService = plcService;
            _routingService = routingService;
            _storeManagementClient = storeManagementClient;
            _toteRepository = toteRepository;
            _totesReadyForPicking = totesReadyForPicking;
            _blockedTotesSolver = new BlockedTotesSolver(loggerFactory, toteRepository, locationRepository,
                locationService, _transfers, _deviceId, _taskBundles);
            _craneTransfersPlanner = new CraneTransfersPlanner(loggerFactory, routingService);
        }


        public void ProcessTransferRequestDone(TransferRequestDoneModel transferRequestDone)
        {
            lock (this)
            {
                _logger.LogInformation("device: {1}, Processing of transfer request done: {2} on ", _deviceId,
                    transferRequestDone);

                if (!IsRequestDoneForThisDevice(transferRequestDone))
                {
                    _logger.LogInformation("Skipped processing transfer done {0} on crane {1}",
                        transferRequestDone,
                        _deviceId);
                    return;
                }

                _transfers.LogQueueStatus("Request queue status before processing transfer done");

                ResetShelfState(transferRequestDone);

                ClosePendingRequests(transferRequestDone.transferRequest1Done);
                ClosePendingRequests(transferRequestDone.transferRequest2Done);

                _transfers.LogQueueStatus("Request queue status after processing transfer done");
                
                _PlanMoves();
                _ExecuteNextMoveOrDoNothing();
            }
        }

        private void ClosePendingRequests(ToteTransferRequestDoneModel transferDone)
        {
            if (transferDone == null) return;

            var sortCode = SystemSortCodes.Get(transferDone.sortCode);

            if (SystemSortCodes.RecoverableErrors.Contains(sortCode)
                || sortCode.FailReason == SystemFailReason.PlaceLocationOccupied
                && transferDone.requestedDestLocationId.Contains("CNV"))
            {
                _logger.LogInformation(
                    "deviceId: {1}, received transfer done with error code {2}, skipping as this is handled elsewhere",
                    _deviceId, sortCode);
                if (sortCode.FailReason != SystemFailReason.DeviceBusy)
                {
                    _transfers.ResetExecuteStatus(transferDone);
                }
                else
                {
                    _logger.LogWarning("deviceId: {1}, Not resetting device status, since device is busy!");
                }
            }
            else if (SystemSortCodes.TaskRecoverableError.Contains(sortCode))
            {
                RemoveAndReturnTransferRequest(transferDone);
            }
            else
            {
                var removeAndReturnTransferRequest = RemoveAndReturnTransferRequest(transferDone);
                ReportToStoreManagementIfRequestDone(transferDone, removeAndReturnTransferRequest);
            }
        }

        private Transfer RemoveAndReturnTransferRequest(ToteTransferRequestDoneModel transferDone)
        {
            if (transferDone == null) return null;
            var craneRequest = _transfers.GetByTaskId(transferDone.requestId.GetTaskId());
            _transfers.RemoveAllMatchingRequestDone(transferDone);
            return craneRequest;
        }

        private void ResetShelfState(TransferRequestDoneModel transferRequestDone)
        {
            if (CraneMoveRequestExistsForTransferDone(transferRequestDone.transferRequest1Done))
                CraneState.SetShelfReady(First);
            if (CraneMoveRequestExistsForTransferDone(transferRequestDone.transferRequest2Done))
                CraneState.SetShelfReady(Second);
        }
        

        public void HandleNewTaskBundle(TaskBundle newTaskBundle)
        {
            lock (this)
            {
                _logger.LogInformation("deviceId: {1}, handling new task bundle: {2}, all is well - planning moves",
                    _deviceId, newTaskBundle);
                _PlanMoves();
                _ExecuteNextMoveOrDoNothing();
            }
        }

        public void HandleTaskBundleRemoved()
        {
            lock (this)
            {
                _logger.LogInformation("deviceId: {1}, task bundle was removed, planning moves if necessary",
                    _deviceId);
                if (_taskBundles.Count <= 0)
                {
                    _logger.LogInformation("deviceId: {1}, no pending task bundles, waiting...", _deviceId);
                    return;
                }

                _logger.LogInformation("deviceId: {1}, there are pending task bundles- planning moves", _deviceId);
                _PlanMoves();
                _ExecuteNextMoveOrDoNothing();
            }
        }

        private void ReportToStoreManagementIfRequestDone(ToteTransferRequestDoneModel transferRequestDone,
            Transfer craneRequest)
        {
            if (craneRequest == null)
            {
                _logger.LogInformation("No crane request for {0} exists for device {1}",
                    transferRequestDone,
                    _deviceId);
                return;
            }

            if (transferRequestDone.sortCode != 1)
            {
                _logger.LogInformation("deviceId: {1}, skipping reporting of failed transfer request done, " +
                                       "this should be done in Recovery Handler! Transfer request done: {2}",
                    _deviceId,
                    transferRequestDone);
                return;
            }

            _logger.LogInformation("Transfer request DONE. ToteId: {1}, Source: {2} Dest: {3}, RequestId: {4}",
                transferRequestDone.sourceToteBarcode,
                transferRequestDone.sourceLocationId,
                transferRequestDone.actualDestLocationId,
                transferRequestDone.requestId
            );

            var moveTask = _taskBundles.GetMoveTask(transferRequestDone.requestId.GetTaskId());

            if (moveTask == null)
            {
                _logger.LogInformation("Move task not found for requestId: {0}, skipping",
                    transferRequestDone.requestId);
                return;
            }

            if (craneRequest.destLocation.plcId != moveTask.destLocation.plcId)
            {
                _logger.LogInformation("Crane request dest: {0}, moveTask dest: {1}, not matching, skipping!",
                    craneRequest.destLocation.plcId, moveTask.destLocation.plcId);
                return;
            }

            _taskBundles.UpdateTaskStatus(moveTask.taskId, GetRcsStatus(transferRequestDone, craneRequest));
            _taskBundles.CompleteTask(moveTask);
        }

        private void _ExecuteNextMoveOrDoNothing()
        {
            if (CraneState.IsBusy())
            {
                _logger.LogDebug("DeviceId: {0} is already executing request, skipping", _deviceId);
                return;
            }

            var requests = _transfers.GetUpTo(CraneState.GetNumberOfOperatingShelves());
            requests = _blockedTotesSolver.ShuffleIfRequired(requests);
            var craneTransfersPlan = _craneTransfersPlanner.PlanTransfer(requests, _deviceId);

            Execute(craneTransfersPlan);
        }

        private void Execute(CraneTransfersPlan plannedTransfers)
        {
            if (plannedTransfers.IsEmpty())
            {
                _logger.LogDebug("Device: {0}, there is nothing to execute!", _deviceId);
                return;
            }

            _logger.LogDebug("Device: {0}, executing requests: {1}", _deviceId, plannedTransfers);

            plannedTransfers.GetShelves().ForEach(shelf => CraneState.SetShelfBusy(shelf));
            _transfers.SetExecuteStatus(plannedTransfers.GetRequestForShelfOrDefault(First));
            _transfers.SetExecuteStatus(plannedTransfers.GetRequestForShelfOrDefault(Second));
            _taskBundles.UpdateTaskStatus(plannedTransfers.GetRequestForShelfOrDefault(First)?.task?.taskId, RcsTaskStatus.Executing);
            _taskBundles.UpdateTaskStatus(plannedTransfers.GetRequestForShelfOrDefault(Second)?.task?.taskId, RcsTaskStatus.Executing);

            _plcService.RequestTransfer(new TransferRequestModel(
                CreateTransferRequest(UpdateToteRequestedLocation(plannedTransfers.GetRequestForShelfOrDefault(First))),
                CreateTransferRequest(
                    UpdateToteRequestedLocation(plannedTransfers.GetRequestForShelfOrDefault(Second))))
            );
        }

        private Transfer UpdateToteRequestedLocation(Transfer request)
        {
            if (request == null) return null;

            _logger.LogInformation("Updating tote {0} requested location to: {1}", request.tote.toteBarcode,
                request.destLocation.plcId);
            
            if(request.sourceLocation?.zone.function == LocationFunction.Place)_totesReadyForPicking.Remove(request.tote);
            return request;
        }

        private ToteTransferRequestModel CreateTransferRequest(Transfer request)
        {
            if (request == null) return null;

            var toteDetails = GetToteDetails(request);

            var toteTransferRequestModel = new ToteTransferRequestModel
            {
                DestLocationId = request.destLocation.plcId,
                Id = new TransferId(request.task.taskId),
                MaxAcc = (float) toteDetails.maxAcc,
                Weight = (float) toteDetails.weight,
                SourceLocationId = request.sourceLocation.plcId,
                ToteBarcode = request.tote.status != ToteStatus.NoRead ? request.tote.toteBarcode : Barcode.NoRead,
                ToteType = new RequestToteType(request.tote.type)
            };
            return toteTransferRequestModel;
        }

        private ToteData GetToteDetails(Transfer request)
        {
            ToteData toteDetails;
            if (!Barcode.Unknown.Equals(request.tote.toteBarcode) && !Barcode.NoTote.Equals(request.tote.toteBarcode) &&
                !Barcode.NoRead.Equals(request.tote.toteBarcode))
            {
                toteDetails = _storeManagementClient.GetToteDetails(request.tote.toteBarcode);
            }
            else
            {
                toteDetails = new ToteData()
                {
                    maxAcc = 1d,
                    weight = 1000d
                };
            }

            return toteDetails;
        }

        private void _PlanMoves()
        {
            lock (this)
            {
                var moveBundles = _taskBundles.GetMoveBundles();

                moveBundles.ForEach(moveBundle =>
                {
                    _transfers.AddNonOverlapingRange(CreateCraneRequestsForMoveTasks(moveBundle));
                    _transfers.LogQueueStatus("Request queue status after adding request from move bundle: " +
                                                  moveBundle.taskBundleId);
                });

                var taskBundle = _taskBundles.GetCurrentPickTaskBundle();
                if (taskBundle != null)
                {
                    _logger.LogInformation("deviceId: {1}, planning moves for current task bundle: {2}", _deviceId,
                        taskBundle.taskBundleId);

                    _transfers.AddNonOverlapingRange(CreateCraneRequestsForMoveTasks(taskBundle));
                    _transfers.AddNonOverlapingRange(CreateCraneRequestsForPickTasks(taskBundle,
                        task => task.destTote,
                        "destination"));
                    _transfers.AddNonOverlapingRange(CreateCraneRequestsForPickTasks(taskBundle,
                        task => task.sourceTote,
                        "source"));

                    _transfers.LogQueueStatus(
                        "Request queue status after adding request from current task bundle " +
                        taskBundle.taskBundleId);

                    var nextBundle = _taskBundles.GetNextPickTaskBundle();
                    if (nextBundle != null)
                    {
                        _logger.LogInformation("deviceId: {1}, planning moves for next task bundle: {2}", _deviceId,
                            nextBundle.taskBundleId);

                        _transfers.AddNonOverlapingRange(CreateCraneRequestsForMoveTasks(nextBundle));
                        _transfers.AddNonOverlapingRange(CreateCraneRequestsForPickTasks(nextBundle,
                            task => task.sourceTote,
                            "source", 2));

                        _transfers.LogQueueStatus(
                            "Request queue status after adding request for next task bundle " +
                            nextBundle.taskBundleId);
                    }
                }
            }
        }

        private List<Transfer> CreateCraneRequestsForPickTasks(TaskBundle taskBundle,
            Func<PickTask, PickToteData> toteSelector, string logInfo, int? max = null)
        {
            var tasks = new ArrayList(taskBundle.tasks);
            var craneRequests = tasks.OfType<PickTask>()
                .Select(LogTask)
                .Where(task => task.taskStatus == RcsTaskStatus.Idle || task.taskStatus == RcsTaskStatus.Executing)
                .Where(task => CraneShouldMoveTote(toteSelector.Invoke(task).toteId))
                .Select(LogTask)
                .GroupBy(task => toteSelector.Invoke(task).toteId)
                .Select(it => it.First())
                .Select(LogTask)
                .Select(task =>
                {
                    var tote = _toteRepository.GetToteByBarcode(toteSelector.Invoke(task).toteId);
                    var craneNextLocation = GetCraneNextLocation(tote, toteSelector.Invoke(task).pickLocation);

                    if (craneNextLocation == null)
                    {
                        _logger.LogWarning(
                            "deviceId: {1}, not adding task as tote is already on target location: {2} or it's not routed by crane. Current location: {3}",
                            _deviceId,
                            toteSelector.Invoke(task).pickLocation, tote.location.plcId);
                        return null;
                    }

                    if (tote?.location?.zone.function == LocationFunction.Conveyor)
                    {
                        _logger.LogWarning(
                            "deviceId: {1}, not adding task as tote is on conveyor: {2}",
                            _deviceId, tote.location.plcId);
                        return null;
                    }

                    return new Transfer
                    {
                        task = task,
                        tote = tote,
                        destLocation = craneNextLocation,
                        sourceLocation = tote.location
                    };
                }).Where(it => it != null)
                //Move back tote from robot place location if a tote is present there
                .SelectMany(ClearPlaceLocation())
                .ToList();

            if (max != null)
            {
                _logger.LogInformation(
                    "deviceId: {1}, given max: {2}, reducing number of potential requests from bundle: {3}", _deviceId,
                    max, taskBundle.taskBundleId);
                craneRequests = craneRequests.GetRange(0, Math.Min(max.Value, craneRequests.Count));
            }

            _logger.LogInformation("deviceId: {1}, created {2} requests for {3} totes from task bundle: {4}", _deviceId,
                craneRequests.Count, logInfo, taskBundle.taskBundleId);

            return craneRequests;
        }
        //Move back tote from robot place location if a tote is present there
        private Func<Transfer, IEnumerable<Transfer>> ClearPlaceLocation()
        {
            return request =>
            {
                if (request.destLocation.zone.function != LocationFunction.Place ||
                    request.destLocation.storedTote == null) return new[] {request};
                var tote = _toteRepository.GetToteByBarcode(request.destLocation.storedTote.toteBarcode);
                var craneNextLocation = GetCraneNextLocation(tote, tote.storageLocation);

                if (_transfers.IsPriorityTote(tote)) return new[] {request};

                
                var additionalRequest = new Transfer
                {
                    task = _taskBundles.GetInternalMoveTask(tote, tote.storageLocation),
                    tote = tote,
                    destLocation = craneNextLocation,
                    sourceLocation = tote.location
                };
                _logger.LogDebug("Moving tote: {0} from RPP1, created additional craneRequest {1}", tote, additionalRequest);
                return new[] {additionalRequest, request};
            };
        }

        private List<Transfer> CreateCraneRequestsForMoveTasks(TaskBundle taskBundle)
        {
            var craneRequests = taskBundle.tasks.OfType<MoveTask>()
                .Select(LogTask)
                .Where(task => task.taskStatus == RcsTaskStatus.Idle)
                .Where(task => CraneShouldMoveTote(task.toteId))
                .Select(LogTask)
                .Select(task =>
                {
                    var tote = _toteRepository.GetToteByBarcode(task.toteId);
                    var craneNextLocation = GetCraneNextLocation(tote, task.destLocation);

                    if (craneNextLocation == null)
                    {
                        _logger.LogWarning(
                            "deviceId: {1}, not adding task as tote is already on target location: {2}", _deviceId,
                            task.destLocation);
                        return null;
                    }

                    if (tote?.location?.zone.function == LocationFunction.Conveyor)
                    {
                        _logger.LogWarning(
                            "deviceId: {1}, not adding task as tote is on conveyor: {2}",
                            _deviceId, tote.location.plcId);
                        return null;
                    }

                    var sourceLocation = tote.location;
                    

                    return new Transfer
                    {
                        task = task,
                        tote = tote,
                        destLocation = craneNextLocation,
                        sourceLocation = sourceLocation
                    };
                }).Where(it => it != null)
                //Move back tote from robot place location if a tote is present there
                .SelectMany(ClearPlaceLocation())
                .ToList();

            _logger.LogInformation("deviceId: {1}, created {2} requests for move tasks from task bundle: {3}",
                _deviceId, craneRequests.Count, taskBundle.taskBundleId);

            return craneRequests;
        }

        private MoveTask LogTask(MoveTask task)
        {
            _logger.LogInformation("deviceId: {0}, Considering task {1} for converting to crane request", _deviceId,
                task);
            return task;
        }

        private PickTask LogTask(PickTask task)
        {
            _logger.LogInformation("deviceId: {0}, Considering task {1} for converting to crane request", _deviceId,
                task);
            return task;
        }

        private bool CraneShouldMoveTote(string toteId)
        {
            var sourceTote = _toteRepository.GetToteByBarcode(toteId);
            return _routingService.AnyRouteForDeviceContainsTotesLocation(_deviceId, sourceTote.location);
        }

        private Location GetCraneNextLocation(Tote tote, Location destLocation)
        {
            var nextLocation = _routingService.GetNextLocation(tote.location, destLocation);
            _logger.LogInformation(
                "deviceId: {0} Router returned next location: {1} for tote: {2} on current location {3} routed to {4} ",
                _deviceId, nextLocation.plcId, tote.toteBarcode, tote.location.plcId, destLocation.plcId);
            return _routingService.IsRoutedBy(_deviceId, tote.location, nextLocation) ? nextLocation : null;
        }

        private static RcsTaskStatus GetRcsStatus(ToteTransferRequestDoneModel transferRequestDone,
            Transfer craneRequest)
        {
            return transferRequestDone.actualDestLocationId == craneRequest.destLocation.plcId
                ? RcsTaskStatus.Complete
                : RcsTaskStatus.Faulted;
        }

        private bool CraneMoveRequestExistsForTransferDone(ToteTransferRequestDoneModel transferRequestDone)
        {
            if (transferRequestDone == null) return false;
            if (string.IsNullOrEmpty(transferRequestDone.sourceToteBarcode)) return false;

            return _transfers.AnyMatchingRequestDone(transferRequestDone);
        }

        private bool IsRequestDoneForThisDevice(TransferRequestDoneModel transferRequestDone)
        {
            return CraneMoveRequestExistsForTransferDone(transferRequestDone.transferRequest1Done)
                   || CraneMoveRequestExistsForTransferDone(transferRequestDone.transferRequest2Done);
        }

        public void Execute(Transfer request)
        {
            if (request == null)
            {
                _logger.LogInformation("deviceId: {1}, Requested to execute null!, Skipping!", _deviceId);
                return;
            }

            lock (this)
            {
                _transfers.InsertNonOverlapping(request);
                _ExecuteNextMoveOrDoNothing();
            }
        }

        public void Execute(List<Transfer> transfers)
        {
            transfers.Reverse();
            transfers.ForEach(Execute);
        }

        public bool ShouldHandleTransferDone(ToteTransferRequestDoneModel transferRequestDone)
        {
            lock (this)
            {
                return _transfers.AnyMatchingRequestDone(transferRequestDone);
            }
        }

        public Transfer GetCompletedTransfer(ToteTransferRequestDoneModel transferRequestDoneModel)
        {
            lock (this)
            {
                return _transfers.GetMatchingRequestDone(transferRequestDoneModel);
            }
        }

        public void AddPriorityTote(string barcode)
        {
            _transfers.AddPriorityTote(barcode);
        }

        public void RemovePriorityTote(string barcode)
        {
            _transfers.RemovePriorityTote(barcode);
        }

        public void CraneIdle()
        {
            lock (this)
            {
                _ExecuteNextMoveOrDoNothing();
            }
        }

        public List<Transfer> GetTimedOutRequests()
        {
            lock (this)
            {
                if (CraneState.IsTransferTimedOut())
                {
                    return _transfers.GetRequestsInExecution();
                }
                return new List<Transfer>();
            }
        }

        public void FailCraneRequest(Transfer craneRequest)
        {
            lock (this)
            {
                if (craneRequest == null) return;
                _transfers.Remove(craneRequest);
                if (_transfers.GetRequestsInExecution().Any()) return;
                CraneState.SetShelfReady(First);
                CraneState.SetShelfReady(Second);
            }
        }

        public void RemovePlannedTransfers(TaskBase task)
        {
            lock (this)
            {
                _transfers.RemoveNotStartedRequestsForTask(task);
            }
        }
    }
}
