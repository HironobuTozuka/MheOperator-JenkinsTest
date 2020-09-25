using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Models;
using Common.Models.Location;
using Common.Models.Pick;
using Common.Models.Task;
using Common.Models.Tote;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Models.Device;
using RcsLogic.Services;

namespace RcsLogic.Robot
{
    public class RobotDevice : Device, IPickRequestDoneListener, IPrepareForPickingDevice, IToteReleasingDevice
    {
        private readonly ILogger<RobotDevice> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPlcService _plcService;
        private readonly IMujinClient _mujinClient;

        private bool _robotPicking;
        private PickTask _preparedForTask;
        private PickTask _currentlyExecutedTask;
        private TotesReadyForPicking _totesReadyForPicking;
        private IToteShakingDevice _toteShakingDevice;
        private bool _rotateToteForMujin = false;

        private readonly List<IReturnToteHandler> _returnTotesHandler = new List<IReturnToteHandler>();
        private readonly List<ITotePrioritizingDevice> _totePrioritizingDevices = new List<ITotePrioritizingDevice>();

        public RobotDevice(DeviceId deviceId, IConfiguration configuration,  IPlcService plcService, ILoggerFactory loggerFactory,
            TaskBundleService taskBundles, IMujinClient mujinClient, TotesReadyForPicking totesReadyForPicking) 
            : base(taskBundles, deviceId)
        {
            _logger = loggerFactory.CreateLogger<RobotDevice>();
            _configuration = configuration;
            _plcService = plcService;
            _mujinClient = mujinClient;
            _totesReadyForPicking = totesReadyForPicking;
            _rotateToteForMujin = _configuration["RobotDevice:RotateToteForMujin"]
                ?.Equals(true.ToString(), StringComparison.OrdinalIgnoreCase) == true;
            _logger.LogInformation($"Rotate tote for mujin is {_rotateToteForMujin}");
        }

        public void RegisterReturnHandler(IReturnToteHandler toteReleasedListener)
        {
            lock (this)
            {
                _logger.LogInformation("{0} subscribed to tote released by robot", toteReleasedListener.GetType());
                _returnTotesHandler.Add(toteReleasedListener);
            }
        }
        
        public void RegisterTotePrioritisingDevice(ITotePrioritizingDevice totePrioritizingDevice)
        {
            lock (this)
            {
                _logger.LogInformation("{0} subscribed toote prioritising device", totePrioritizingDevice.GetType());
                _totePrioritizingDevices.Add(totePrioritizingDevice);
            }
        }

        public void RegisterToteShakingDevice(IToteShakingDevice toteShakingDevice)
        {
            _toteShakingDevice = toteShakingDevice;
        }


        public void ProcessPickRequestDone(PickRequestDoneModel pickRequestDone)
        {
            lock (this)
            {
                var sortCode = RobotSortCodes.Get(pickRequestDone.sortCode);
                var tote = _totesReadyForPicking.GetTote(pickRequestDone.sourceToteBarcode);

                _logger.LogInformation("Processing pick request done: {0}, received sortCode: {1}", pickRequestDone, sortCode);

                _robotPicking = false;

                var taskBundleForRequestIdExists = _taskBundles.TaskBundleForPickRequestIdExists(pickRequestDone.requestId);
                var shouldUpdateTaskStatus = taskBundleForRequestIdExists && !ShouldShakeTote(sortCode, tote);
                if (shouldUpdateTaskStatus)
                {
                    if(tote.ShakingCount>0 && pickRequestDone.actualPickCount != pickRequestDone.requestedPickCount) 
                        _logger.LogDebug("Shaking was not successful, failing task");
                    _taskBundles.UpdateTaskStatus(pickRequestDone.requestId.GetTaskId(),
                        pickRequestDone.actualPickCount == pickRequestDone.requestedPickCount
                            ? RcsTaskStatus.Complete
                            : RcsTaskStatus.Faulted);

                    _logger.LogInformation(
                        "Updating task status, because task bundle exists for task: {0}",
                        pickRequestDone.requestId);
                }

                if (sortCode.IsPlaceToteError())
                {
                    _logger.LogWarning("Releasing dest tote, received place tote error");
                    ReleaseTote(_totesReadyForPicking.GetTote(pickRequestDone.destToteBarcode));
                    _taskBundles.ReportTaskState(pickRequestDone.requestId.GetTaskId(), pickRequestDone.actualPickCount,
                        pickRequestDone.requestedPickCount - pickRequestDone.actualPickCount, sortCode);
                    _logger.LogDebug("Waiting for action from SM");
                    return;
                }

                if (ShouldShakeTote(sortCode, tote))
                {
                    _logger.LogDebug("Robot requesting to shake tote: {0}", tote.Tote);
                    tote.Status = ToteReadyForPickingStatus.Shaking;
                    tote.ShakingCount += 1;
                    _toteShakingDevice.ShakeTote(tote.Tote);
                }

                ReleaseTotes();

                if (shouldUpdateTaskStatus)
                {
                    _taskBundles.CompleteTask(pickRequestDone.requestId.GetTaskId(), pickRequestDone.actualPickCount,
                        pickRequestDone.requestedPickCount - pickRequestDone.actualPickCount, sortCode);
                    _logger.LogInformation(
                        "Completing task, because task bundle exists for task: {0}",
                        pickRequestDone.requestId);
                }

                _logger.LogInformation(
                    "For request: {0} Parts picked: success: {1}, failed: {2}, checking if there is further work to be done",
                    pickRequestDone.requestId, pickRequestDone.actualPickCount,
                    pickRequestDone.requestedPickCount - pickRequestDone.actualPickCount);

                SendToRobot();
            }
        }

        private bool ShouldShakeTote(RobotSortCode sortCode, ToteReadyForPicking tote)
        {
            return sortCode.IsError() && sortCode.ShakingCanHelp && tote.ShakingCount < 1 && _toteShakingDevice!= null;
        }

        public void ToteReady(PrepareForPicking toteReady)
        {
            lock (this)
            {
                _totesReadyForPicking.Add(toteReady);
                _logger.LogInformation("Tote {1} ready for picking by robot on location {2}", toteReady.Tote.toteBarcode,
                    toteReady.Location.plcId);
                SendToRobot();
            }
        }

        public void ReleaseTotes()
        {
            lock (this)
            {
                List<ToteReadyForPicking> allTotes;
                var sourceTotes = new List<ToteReadyForPicking>();
                var destTotes = new List<ToteReadyForPicking>();
                var currentTaskBundle = _taskBundles.GetFirstPickTaskBundle();

                if (currentTaskBundle != null)
                {
                    _logger.LogInformation("Task bundle count greater than one on SendToRobotOrRelease");
                    allTotes = _totesReadyForPicking.GetTotesToBeReleased(currentTaskBundle);
                    sourceTotes = FilterTotes(allTotes, currentTaskBundle, task => task.sourceTote);
                    destTotes = FilterTotes(allTotes, currentTaskBundle, task => task.destTote);
                    FailMoveTasksForTotesLeftForPicking(allTotes);
                }
                else
                {
                    allTotes = _totesReadyForPicking.ToList();
                }

                var totesNotFromTaskBundle = GetTotesNotFromTaskBundle(allTotes);

                destTotes.ForEach(ReleaseTote);
                sourceTotes.ForEach(ReleaseAndReturnTote);
                totesNotFromTaskBundle.ForEach(ReleaseAndReturnTote);
            }
        }

        private void FailMoveTasksForTotesLeftForPicking(List<ToteReadyForPicking> allTotesToRelease)
        {
            _totesReadyForPicking.ToList().Where(tote => allTotesToRelease.All(t => t != tote))
                .Select(tote => tote.Tote)
                .ToList()
                .ForEach(_taskBundles.FailMoveTasksForTote);
        }

        private List<ToteReadyForPicking> GetTotesNotFromTaskBundle(List<ToteReadyForPicking> allTotes)
        {
            var currentTaskBundle = _taskBundles.GetCurrentPickTaskBundle();
            var nextPickTaskBundle = _taskBundles.GetNextPickTaskBundle();

            if (currentTaskBundle != null)
            {
                _logger.LogInformation("deviceId: {1}, removing totes from current pick task bundle: {2}", _deviceId,
                    currentTaskBundle);
                var notInPickBundle = allTotes.Where(tote =>
                        !currentTaskBundle.tasks
                            .OfType<PickTask>()
                            .Any(task =>
                                tote.Tote.toteBarcode.Equals(task.sourceTote.toteId) ||
                                tote.Tote.toteBarcode.Equals(task.destTote.toteId)))
                    .Select(it => LogTote(it, currentTaskBundle));

                if (nextPickTaskBundle != null)
                {
                    _logger.LogInformation("deviceId: {1}, removing totes from next pick task bundle: {2}", _deviceId,
                        nextPickTaskBundle);
                    notInPickBundle = notInPickBundle.Where(tote =>
                            !nextPickTaskBundle.tasks
                                .OfType<PickTask>()
                                .Any(task =>
                                    tote.Tote.toteBarcode.Equals(task.sourceTote.toteId) ||
                                    tote.Tote.toteBarcode.Equals(task.destTote.toteId)))
                        .Select(it => LogTote(it, nextPickTaskBundle));
                }

                return notInPickBundle.Where(tote => tote.Location.zone.function != LocationFunction.Place).ToList();
            }

            return allTotes.Where(tote => tote.Location.zone.function != LocationFunction.Place).ToList();
        }

        private ToteReadyForPicking LogTote(ToteReadyForPicking tote, TaskBundle taskBundle)
        {
            _logger.LogInformation("Tote: {1} is not present in task bundle : {2}", tote.Tote.toteBarcode,
                taskBundle.taskBundleId);
            return tote;
        }

        private void ReleaseAndReturnTote(ToteReadyForPicking tote)
        {
            _logger.LogInformation("Source Tote {1} ready to be released from location {2}", tote.Tote.toteBarcode,
                tote.Location.plcId);
            _totesReadyForPicking.Release(tote.Tote.toteBarcode);
            _totesReadyForPicking.Remove(tote);
            _returnTotesHandler.ForEach(listener => listener.ReturnTote(tote.Tote.toteBarcode));
        }

        private void ReleaseTote(ToteReadyForPicking tote)
        {
            _logger.LogInformation("Dest Tote {1} ready to be released from location {2}", tote.Tote.toteBarcode,
                tote.Location.plcId);
            _totePrioritizingDevices.ForEach(it => it.AddPriorityTote(tote.Tote.toteBarcode));
            _totesReadyForPicking.Release(tote.Tote.toteBarcode);
            _totesReadyForPicking.Remove(tote);
        }



        private List<ToteReadyForPicking> FilterTotes(List<ToteReadyForPicking> totesToBeReleased,
            TaskBundle currentTaskBundle, Func<PickTask, PickToteData> toteFilter)
        {
            return totesToBeReleased
                .Where(tote =>
                    currentTaskBundle.tasks.OfType<PickTask>()
                        .Any(task => toteFilter.Invoke(task).toteId.Equals(tote.Tote.toteBarcode))
                ).ToList();
        }

        private void SendToRobot()
        {
            lock (this)
            {
                lock (_taskBundles)
                {
                    TaskBundle currentPickTaskBundle = _taskBundles.GetFirstPickTaskBundle();

                    //Send task to robot
                    if (currentPickTaskBundle != null)
                    {
                        _logger.LogTrace("Current pick task bundle different than null");

                        PickTask currentTask = null;
                        var totesReadyForPicking = _totesReadyForPicking.ToList();

                        if (_preparedForTask != null && !_robotPicking)
                        {
                            currentTask = currentPickTaskBundle.tasks.OfType<PickTask>().FirstOrDefault(task =>
                                (!task.IsFinished())
                                && totesReadyForPicking.Any(tote => tote.Tote.toteBarcode == task.sourceTote.toteId)
                                && totesReadyForPicking.Any(tote => tote.Tote.toteBarcode == task.destTote.toteId)
                                && task.taskId.Equals(_preparedForTask.taskId));
                            _currentlyExecutedTask = currentTask;
                            if(currentTask != null) 
                                _logger.LogTrace("Found prepared for task {0} for robot", _currentlyExecutedTask);
                        }

                        if (currentTask == null && !_robotPicking)
                        {
                            currentTask = currentPickTaskBundle.tasks.OfType<PickTask>().FirstOrDefault(task =>
                                (!task.IsFinished())
                                && totesReadyForPicking.Any(tote => tote.Tote.toteBarcode == task.sourceTote.toteId)
                                && totesReadyForPicking.Any(tote => tote.Tote.toteBarcode == task.destTote.toteId));
                            _currentlyExecutedTask = currentTask;
                            if(currentTask != null) 
                                _logger.LogTrace("Prepared for task was not found, found new task {0} to robot", _currentlyExecutedTask);
                        }

                        PickTask nextTask = null;

                        //Find task for preparation
                        if (currentTask != null || _preparedForTask == null)
                        {
                            nextTask = currentPickTaskBundle.tasks.OfType<PickTask>().FirstOrDefault(task =>
                                (!task.taskId.Equals(_currentlyExecutedTask?.taskId))
                                && (!task.IsFinished())
                                && totesReadyForPicking.Any(tote => tote.Tote.toteBarcode == task.sourceTote.toteId)
                                && totesReadyForPicking.Any(tote => tote.Tote.toteBarcode == task.destTote.toteId));
                            if(nextTask != null) _logger.LogTrace(
                                "Found task for preparation {0} for robot when _currentlyExecutedTask is {1}",
                                nextTask, _currentlyExecutedTask);
                        }


                        if (currentTask == null && nextTask == null) return;

                        if (currentTask != null)
                        {
                            _robotPicking = true;
                        }
                        

                        _taskBundles.UpdateTaskStatus(currentTask?.taskId, RcsTaskStatus.Picking);

                        _SendRequestToRobot(
                            currentTask,
                            _totesReadyForPicking.GetTote(currentTask, task => task.sourceTote),
                            _totesReadyForPicking.GetTote(currentTask, task => task.destTote),
                            nextTask,
                            _totesReadyForPicking.GetTote(nextTask, task => task.sourceTote),
                            _totesReadyForPicking.GetTote(nextTask, task => task.destTote));
                    }
                }
            }
        }

        private void _SendRequestToRobot(PickTask pickTask, ToteReadyForPicking source,
            ToteReadyForPicking dest, PickTask prepareTask, ToteReadyForPicking prepareSource, ToteReadyForPicking prepareDest)
        {
            _logger.LogInformation(
                "Request sent to pick {0} objects of barcode {1} form tote {2}, slot {3} to tote {4}, " +
                "Preparation sent for {5} objects of barcode {6} form tote {7}, slot {8} to tote {9}",
                pickTask?.quantity, pickTask?.barcode, pickTask?.sourceTote.toteId, pickTask?.sourceTote.slotId,
                pickTask?.destTote.toteId,
                prepareTask?.quantity, prepareTask?.barcode, prepareTask?.sourceTote.toteId,
                prepareTask?.sourceTote.slotId, prepareTask?.destTote.toteId);

            _preparedForTask = prepareTask;
            _totesReadyForPicking.Block(dest?.Tote);
            _totesReadyForPicking.Block(source?.Tote);

            var robotPickRequestBundleModel = new RobotPickRequestBundleModel()
            {
                pickRequest = CreateRobotPickRequestModel(dest, pickTask, source),
                preparationRequest = CreateRobotPickRequestModel(prepareDest, prepareTask, prepareSource)
            };

            _plcService.RequestPick(robotPickRequestBundleModel);
        }

        private RobotPickRequestModel CreateRobotPickRequestModel(ToteReadyForPicking destinationTote, PickTask task,
            ToteReadyForPicking sourceTote)
        {
            if (task == null) return null;

            return new RobotPickRequestModel()
            {
                id = new PickId(task.taskId.Id),
                pickCount = (ushort) task.quantity,
                partName = _mujinClient.GetSkuName(task.barcode),
                source = new ToteRequestModel()
                {
                    barcode = task.sourceTote.toteId,
                    slotId = GetSlotId(task.sourceTote.slotId, sourceTote.ToteRotation, sourceTote.Tote.type),
                    toteType = sourceTote.Tote.type,
                    locationId = sourceTote.Location.plcId
                },
                dest = new ToteRequestModel()
                {
                    barcode = task.destTote.toteId,
                    slotId = GetSlotId(task.destTote.slotId, destinationTote.ToteRotation, destinationTote.Tote.type),
                    toteType = destinationTote.Tote.type,
                    locationId = destinationTote.Location.plcId
                }
            };
        }

        private int GetSlotId(int requestedSlot, ToteRotation toteRotation, ToteType toteType)
        {
            if (!_rotateToteForMujin || toteRotation != ToteRotation.reversed) return requestedSlot;
            switch (toteType.totePartitioning)
            {
                case TotePartitioning.bipartite:
                    return Math.Abs(requestedSlot - 1);
                case TotePartitioning.tripartite:
                    switch (requestedSlot)
                    {
                        case 0:
                            return 2;
                        case 2:
                            return 0;
                    }
                    break;
                default:
                    throw new Exception("Unknown tote type and tote rotating for mujin is enabled");
            }

            return requestedSlot;
            
        }
    }


}