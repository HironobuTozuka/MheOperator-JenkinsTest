using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Common.Models.Location;
using Common.Models.Pick;
using Common.Models.Task;
using Common.Models.Tote;
using Common.Models.Transfer;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;

namespace RcsLogic.Services
{
    public class TaskBundleService
    {
        private readonly List<TaskBundle> _taskBundles = new List<TaskBundle>();
        private readonly ILogger<TaskBundleService> _logger;
        private readonly List<ITaskBundleListListener> _listChangedListeners = new List<ITaskBundleListListener>();

        private readonly List<ITaskBundleRemovedListener> _taskBundleRemovedListeners =
            new List<ITaskBundleRemovedListener>();
        private readonly List<ITaskBundleCanceledListener> _taskBundleCanceledListeners =
            new List<ITaskBundleCanceledListener>();
        private readonly List<ITaskBundleAddedListener>
            _taskBundleAddedListeners = new List<ITaskBundleAddedListener>();
        private readonly List<ITaskRemovedListener>
            _taskRemovedListeners = new List<ITaskRemovedListener>();
        private readonly List<ITaskBundleUpdatedListener>
            _taskBundleUpdatedListeners = new List<ITaskBundleUpdatedListener>();

        private readonly IStoreManagementClient _storeManagementClient;
        private readonly LocationService _locationService;
        private IMujinClient _mujinClient;
        private readonly ToteRepository _toteRepository;
        private LocationRepository _locationRepository;

        public int Count => _taskBundles.Count;

        public TaskBundleService(ILoggerFactory loggerFactory, IStoreManagementClient storeManagementClient,
            LocationService locationService, IMujinClient mujinClient, ToteRepository toteRepository,
            LocationRepository locationRepository)
        {
            _logger = loggerFactory.CreateLogger<TaskBundleService>();
            _storeManagementClient = storeManagementClient;
            _locationService = locationService;
            _mujinClient = mujinClient;
            _toteRepository = toteRepository;
            _locationRepository = locationRepository;
        }

        public void RegisterTaskBundleAddedListener(ITaskBundleAddedListener addedListener)
        {
            _taskBundleAddedListeners.Add(addedListener);
        }

        public void RegisterTaskBundleRemovedListener(ITaskBundleRemovedListener removedListener)
        {
            _taskBundleRemovedListeners.Add(removedListener);
        }
        
        public void RegisterTaskBundleCanceledListener(ITaskBundleCanceledListener canceledListener)
        {
            _taskBundleCanceledListeners.Add(canceledListener);
        }
        
        public void RegisterTaskRemovedListener(ITaskRemovedListener removedListener)
        {
            _taskRemovedListeners.Add(removedListener);
        }
        
        public void RegisterTaskBundleUpdatedListener(ITaskBundleUpdatedListener updatedListener)
        {
            _taskBundleUpdatedListeners.Add(updatedListener);
        }

        public void AddTaskBundle(TaskBundle taskBundle)
        {

            CheckSkuIsKnownByMujin(taskBundle);
            CheckTotesExist(taskBundle);
            CheckTotesAreReady(taskBundle);
            CheckZonesExist(taskBundle);
            CheckForDuplicatedTaskBundles(taskBundle);
            CheckForDuplicatedTaskIds(taskBundle.tasks);
            CheckTotesAreOnEnabledLocations(taskBundle.tasks);
            
            //TODO Assigning pick locations here, can be moved somewhere else, to only generate it on task bundle execution start
            _locationService.AssignDestLocationsToTaskBundle(taskBundle);
            
            _logger.LogInformation("New tasks added to RcsLogic: " + string.Join(";", taskBundle.tasks));
            Add(taskBundle);
        }

        private void CheckTotesAreOnEnabledLocations(List<TaskBase> taskBundleTasks)
        {
            var totesOnDisabledLocations = taskBundleTasks.SelectMany(task =>
            {
                var asMoveTask = task as MoveTask;
                var asPickTask = task as PickTask;
                var toteList = new List<Tote>();
                if(asMoveTask != null) toteList.Add(_toteRepository.GetToteByBarcode(asMoveTask.toteId));
                if(asPickTask != null) toteList.Add(_toteRepository.GetToteByBarcode(asPickTask.sourceTote.toteId));
                if(asPickTask != null) toteList.Add(_toteRepository.GetToteByBarcode(asPickTask.destTote.toteId));
                return toteList;
            }).Where(tote => tote.location != null && tote.location.status != Common.Models.Location.LocationStatus.Enabled)
                .ToList();
            if(totesOnDisabledLocations.Any()) throw new TotesLocationDisabledException(totesOnDisabledLocations);
        }

        public void CancelTaskBundle(TaskBundleId taskBundleId)
        {
            TaskBundle taskBundleToCancel = null;
            lock (this)
            {
                taskBundleToCancel = _taskBundles.FirstOrDefault(taskBundle => taskBundle.taskBundleId.Equals(taskBundleId));
                if(taskBundleToCancel == null) throw new TaskBundleNotFoundException() { Id = taskBundleId };
                var tasksToCancel = taskBundleToCancel.tasks.Where(task => task.taskStatus == RcsTaskStatus.Idle
                || task.taskStatus == RcsTaskStatus.Executing).Select(task =>
                {
                    task.taskStatus = RcsTaskStatus.Cancelled;
                    return task;
                }).ToList();
                tasksToCancel.ForEach(task =>
                {
                    if (task is PickTask pickTask)
                    {
                        CompleteTask(pickTask, picked: 0, failed: pickTask.quantity);
                    }
                    else
                    {
                        CompleteTask(task);
                    }
                });

            }
            _taskBundleCanceledListeners.ForEach(listener => listener.HandleTaskBundleCanceled(taskBundleToCancel));
        }
        
        public void UpdateTaskBundle(TaskBundle taskBundle)
        {
            TaskBundle taskBundleToUpdate;
            List<TaskBase> oldTasks;
            lock (this)
            {
                CheckSkuIsKnownByMujin(taskBundle);
                CheckTotesExist(taskBundle);
                CheckZonesExist(taskBundle);
                taskBundleToUpdate = _taskBundles.FirstOrDefault(bundle => bundle.taskBundleId.Equals(taskBundle.taskBundleId));
                if(taskBundleToUpdate == null) throw new TaskBundleNotFoundException() { Id = taskBundle.taskBundleId };
                oldTasks = taskBundleToUpdate.tasks;
                taskBundleToUpdate.tasks = taskBundle.tasks;
            }
            _taskBundleUpdatedListeners.ForEach(listener => listener.HandleTaskBundleUpdated(taskBundle, oldTasks, taskBundle.tasks));
            Task.Run(()=> _taskBundleAddedListeners.ForEach(listener => listener.HandleNewTaskBundle(taskBundleToUpdate)));
        }

        public MoveTask GetInternalMoveTask(Tote tote, Location destLocation)
        {
            MoveTask moveTask = _taskBundles.Where(tb => tb.isInternal)
                .SelectMany(tb => tb.tasks).OfType<MoveTask>()
                .FirstOrDefault(t => t.toteId.Equals(tote.toteBarcode) && t.destLocation.Equals(destLocation));
            if(moveTask == null)
            {
                moveTask = new MoveTask()
                {
                    destLocation = destLocation,
                    destZone = destLocation.zone.zoneId,
                    taskId = new TaskId(Guid.NewGuid().ToString()),
                    taskStatus = RcsTaskStatus.Idle,
                    toteId = tote.toteBarcode,
                    isInternal = true
                };
                
                if (!Barcode.IsWrongBarcode(tote.toteBarcode))
                {
                    _taskBundles.Add(new TaskBundle()
                    {
                        creationDate = DateTime.Now,
                        taskBundleId = new TaskBundleId(Guid.NewGuid().ToString()),
                        isInternal = true,
                        tasks = new List<TaskBase>()
                        {
                            moveTask
                        }
                    });
                }
                _logger.LogDebug("Internal move task was created: {0}", moveTask);

            }
            
            _logger.LogDebug("Returned internal move task: {0}", moveTask);

            return moveTask;
        }

        public void Add(TaskBundle taskBundle)
        {

            _taskBundles.Add(taskBundle);

            Task.Run(()=> _taskBundleAddedListeners.ForEach(listener => listener.HandleNewTaskBundle(taskBundle)));
        }

        public void Remove(TaskBundle taskBundle)
        {

            _logger.LogInformation("Removing completed task bundle {1} and notifying listeners!",
                taskBundle.taskBundleId);
            _taskBundles.Remove(taskBundle);


            _taskBundleRemovedListeners.ForEach(listener => listener.HandleTaskBundleRemoved());
        }

        public List<TaskBundle> ToList()
        {

            return _taskBundles.ToList();
            
        }

        public void CheckForDuplicatedTaskIds(List<TaskBase> tasksToAdd)
        {
            lock (this)
            {
                var duplicatedTasks = _taskBundles.SelectMany(bundle =>
                    bundle.tasks.Where(task =>
                        tasksToAdd.Any(taskToAdd => taskToAdd.taskId.Equals(task.taskId))))
                    .Select(task => task.taskId).ToList();

                if (!duplicatedTasks.Any()) return;
                _logger.LogError("Duplicated tasks from SM: {0}", string.Join(", ", duplicatedTasks));
                throw new TaskIdAlreadyExistsException() {Tasks = duplicatedTasks};
            }
        }
        
        public void CheckForDuplicatedTaskBundles(TaskBundle taskBundle)
        {
            lock (this)
            {
                if (!_taskBundles.Any(bundle => bundle.taskBundleId.Equals(taskBundle.taskBundleId))) return;
                _logger.LogError("Duplicated taskBundleId from SM: {0}", taskBundle.taskBundleId);
                throw new TaskBundleIdAlreadyExistsException() {Id = taskBundle.taskBundleId};

            }
        }

        public TaskBundle GetTaskBundle(TaskId taskId)
        {

            return _taskBundles.FirstOrDefault(bundle => bundle.tasks.Any(task => Equals(task.taskId, taskId)));
            
        }

        public DeliverTask GetDeliverTask(string toteBarcode)
        {
            var taskBundle = _taskBundles.FirstOrDefault(it =>
                    it.tasks.OfType<DeliverTask>().Any(deliverTask => deliverTask.toteId.Equals(toteBarcode)));
                return taskBundle?.tasks.OfType<DeliverTask>()
                    .FirstOrDefault(deliverTask => deliverTask.toteId.Equals(toteBarcode));
            
        }

        public MoveTask GetNonDeliveryMoveTask(string toteBarcode)
        {

            var taskBundle = _taskBundles.FirstOrDefault(it =>
                !it.tasks.OfType<DeliverTask>().Any() &&
                it.tasks.OfType<MoveTask>().Any(moveTask => moveTask.toteId.Equals(toteBarcode)));
            return taskBundle?.tasks.OfType<MoveTask>().FirstOrDefault(moveTask => moveTask.toteId.Equals(toteBarcode));
            
        }

        public MoveTask GetDeliveryMoveTask(string toteBarcode)
        {

            var taskBundle = _taskBundles.FirstOrDefault(it =>
                it.tasks.OfType<DeliverTask>().Any() &&
                it.tasks.OfType<MoveTask>().Any(moveTask => moveTask.toteId.Equals(toteBarcode)));
            return taskBundle?.tasks.OfType<MoveTask>().FirstOrDefault(moveTask => moveTask.toteId.Equals(toteBarcode));
            
        }

        public MoveTask GetMoveTask(TaskId taskId)
        {
            var taskBase = GetTaskBase(taskId);
            return taskBase as MoveTask;
        }

        public TaskBase GetTaskBase(TaskId taskId)
        {
            var taskBundle = GetTaskBundle(taskId);

            return taskBundle?.tasks.FirstOrDefault(task => Equals(task.taskId, taskId));
            
        }

        public void UpdateTaskStatus(TaskId taskId, RcsTaskStatus status)
        {
            if (taskId == null) return;
            var task = GetTaskBase(taskId);
            if (task == null) return;
            if(task.taskStatus == RcsTaskStatus.Idle) task.processingStartedDate = DateTime.Now;
            task.taskStatus = status;
            task.lastUpdateDate = DateTime.Now;
        }

        private void RemoveEmptyTaskBundles()
        {
            List<TaskBundle> taskBundlesToBeRemoved;
            lock (_taskBundles)
            {
                taskBundlesToBeRemoved = _taskBundles.Where(taskBundle => taskBundle.tasks.Count == 0).ToList();

                _taskBundles.RemoveAll(taskBundle => taskBundle.tasks.Count == 0);
            }


            if (!taskBundlesToBeRemoved.Any()) return;
            _logger.LogInformation("Removed completed task bundles {1}!",
                string.Join(",", taskBundlesToBeRemoved));
            _taskBundleRemovedListeners.ForEach(listener => listener.HandleTaskBundleRemoved());
        }

        private void RemoveTask(TaskId taskId)
        {
            TaskBundle taskBundle = GetTaskBundle(taskId);

            taskBundle?.tasks.RemoveAll(task => Equals(task.taskId, taskId));
            
        }

        public TaskBundle GetCurrentPickTaskBundle()
        {
            TaskBundle taskBundle =
                _taskBundles.FirstOrDefault(it => it.tasks.OfType<PickTask>().Any());
            if (taskBundle != null)
            {
                _locationService.AssignDestLocationsToTaskBundle(taskBundle);
            }

            return taskBundle;
        }

        public TaskBundle GetNextPickTaskBundle()
        {
            lock (this)
            {
                TaskBundle first = GetCurrentPickTaskBundle();

                if (first != null)
                {
                    TaskBundle taskBundle = _taskBundles.Where(bundle => !first.Equals(bundle))
                        .FirstOrDefault(bundle => bundle.tasks.OfType<PickTask>().Any());
                    if (taskBundle != null) return taskBundle;
                    taskBundle = _taskBundles.FirstOrDefault(bundle => !first.Equals(bundle));
                    if (taskBundle != null)
                    {
                        _locationService.AssignDestLocationsToTaskBundle(taskBundle);
                    }

                    return taskBundle;
                }

                return null;
            }
        }

        public TaskBundle GetFirstPickTaskBundle()
        {
            TaskBundle taskBundle =
                    _taskBundles.FirstOrDefault(it => it.tasks.OfType<PickTask>().Any());
            return taskBundle;
            
        }

        public bool TaskBundleForPickRequestIdExists(PickId requestId)
        {

            return _taskBundles.Any(tb => tb.tasks.Any(task => task.taskId.Equals(requestId.GetTaskId())));
            
        }

        public List<TaskBundle> GetMoveBundles()
        {
            return _taskBundles.Where(taskBundle => taskBundle.tasks.OfType<MoveTask>().Any()).ToList();
        }
        
        public List<TaskBase> GetAllStartedTasks()
        {

            return _taskBundles
                .SelectMany(taskBundle => 
                    taskBundle.tasks.Where(task => task.taskStatus != RcsTaskStatus.Idle && !task.IsFinished()))
                .ToList();
            
        }

        public TaskId CompleteDeliveryTask(string toteBarcode)
        {
            var deliverTask = GetDeliverTask(toteBarcode);

            if (deliverTask == null) return null;
            deliverTask.taskStatus = RcsTaskStatus.Complete;
            CompleteTask(deliverTask);
            return deliverTask.taskId;

        }

        public MoveTask CompleteMoveTaskIfExists(string toteBarcode, string locationId)
        {
            MoveTask moveTask = null;
            var location = _locationRepository.GetLocationByPlcId(locationId);
            _logger.LogTrace("Found location {0}", location);
            lock (this)
            {
                moveTask = _taskBundles.SelectMany(taskBundle =>
                taskBundle.tasks.OfType<MoveTask>().Where(task =>
                    task.destLocation.plcId.Equals(locationId) 
                    && task.toteId.Equals(toteBarcode) 
                    || (task.destLocation.zone.function == LocationFunction.LoadingGate
                    && location.zone.function == LocationFunction.LoadingGate)
                    && (task.toteId.Equals( location.storedTote?.toteBarcode)
                        || (task.toteId.Contains(Barcode.NoRead) 
                            || toteBarcode.Contains(Barcode.NoRead)))))
                .FirstOrDefault();

                if (moveTask != null) moveTask.taskStatus = RcsTaskStatus.Complete;
                
            }

            if (moveTask != null)
            {
                _logger.LogDebug("Completing move task {0}", moveTask);
                CompleteTask(moveTask);
            }

            return moveTask;
        }

        public void CompleteTask(TaskId taskId, int? picked = null, int? failed = null, ISortCode sortCode = null)
        {
            var task = GetTaskBase(taskId);
            if (task != null)
            {
                CompleteTask(task, picked: picked, failed: failed, failReason: sortCode?.FailReason, failDescription: sortCode?.Name);
            }
            else
            {
                _logger.LogInformation("Unable to find task: {0} to report complete", taskId);
            }
        }
        
        public void ReportTaskState(TaskId taskId, int? picked = null, int? failed = null, ISortCode sortCode = null)
        {
            var task = GetTaskBase(taskId);
            if (task != null)
            {
                ReportTaskState(task, picked: picked, failed: failed, failReason: sortCode?.FailReason, failDescription: sortCode?.Name);
            }
            else
            {
                _logger.LogInformation("Unable to find task: {0} to report state", taskId);
            }
        }

        public void CompleteTask(TaskBase task, int? picked = null, int? failed = null, FailReason? failReason = null, string failDescription = null)
        {

            if(!task.isInternal)
            {
                ReportTaskState(task, picked, failed, failReason, failDescription);
            }

            RemoveTask(task.taskId);
            

            _taskRemovedListeners.ForEach(listener => listener.HandleTaskRemoved(task));

            RemoveEmptyTaskBundles();
        }

        private void ReportTaskState(TaskBase task, int? picked, int? failed, FailReason? failReason, string failDescription)
        {
            try
            {
                if ((task as PickTask) != null)
                {
                    _storeManagementClient.ReportTaskState(task, picked, failed, failReason, failDescription);
                }
                else
                {
                    _storeManagementClient.ReportTaskState(task);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to send Task state {1}");
            }
        }


        public void FailTask(TaskId taskId)
        {
            try
            {
                var task = GetTaskBase(taskId);
                if (task == null) return;
                UpdateTaskStatus(task.taskId, RcsTaskStatus.Faulted);
                if (task is PickTask pick)
                {
                    CompleteTask(pick, 0, pick.quantity);
                }
                else
                {
                    if(task is MoveTask)
                    {
                        var taskBundle = GetTaskBundle(task.taskId);
                        if(taskBundle.tasks.OfType<DeliverTask>().Any())
                        {
                            taskBundle.tasks.OfType<DeliverTask>().ToList().ForEach(t => FailTask(t.taskId));
                        }
                    }
                    CompleteTask(task);
                }
                RemoveEmptyTaskBundles();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Exception!");
            }
        }
        
        private void CheckZonesExist(TaskBundle taskBundle)
        {
            var unknownZones = taskBundle.tasks.OfType<MoveTask>().ToList()
                .Where(task => !_locationRepository.ZoneExists(task.destZone)).Select(task => task.destZone.Id)
                .ToList();
            //Throw exception if unknown zones found
            if (unknownZones.Count > 0)
            {
                _logger.LogError("Received unknown Zones: " + string.Join(";", unknownZones));
                throw new UnknownZoneException() {Zones = unknownZones};
            }
        }

        private void CheckSkuIsKnownByMujin(TaskBundle taskBundle)
        {
            //Find unknown SKUs
            var barcodesToCheck = new List<string>();

            barcodesToCheck.AddRange(taskBundle.tasks.OfType<PickTask>().ToList()
                .Select(pickTask => pickTask.barcode));

            _mujinClient.CheckSkuBarcodes(barcodesToCheck);
        }

        private void CheckTotesExist(TaskBundle taskBundle)
        {
            //Find unknown Totes
            var toteBarcodesToCheck = new List<string>();

            toteBarcodesToCheck.AddRange(taskBundle.tasks.OfType<PickTask>().ToList()
                .Select(pickTask => pickTask.destTote.toteId));
            toteBarcodesToCheck.AddRange(taskBundle.tasks.OfType<PickTask>().ToList()
                .Select(pickTask => pickTask.sourceTote.toteId));
            toteBarcodesToCheck.AddRange(taskBundle.tasks.OfType<DeliverTask>().ToList()
                .Select(pickTask => pickTask.toteId));
            toteBarcodesToCheck.AddRange(taskBundle.tasks.OfType<MoveTask>().ToList()
                .Select(pickTask => pickTask.toteId));

            var unknownTotes = toteBarcodesToCheck
                .Where(barcode => !_toteRepository.Any(barcode)).ToList();

            //Throw exception if unknown Totes found
            if (unknownTotes.Count <= 0) return;
            _logger.LogError("Received unknown Totes: " + string.Join(";", unknownTotes));
            throw new UnknownTotesException() {Totes = unknownTotes};
        }
        
        private void CheckTotesAreReady(TaskBundle taskBundle)
        {
            //Find unknown Totes
            var loadingGateZones = _locationRepository.GetZones().Where(zone => zone.function == LocationFunction.LoadingGate);
            var toteBarcodesToCheck = new List<string>();

            toteBarcodesToCheck.AddRange(taskBundle.tasks.OfType<PickTask>().ToList()
                .Select(pickTask => pickTask.destTote.toteId));
            toteBarcodesToCheck.AddRange(taskBundle.tasks.OfType<PickTask>().ToList()
                .Select(pickTask => pickTask.sourceTote.toteId));
            toteBarcodesToCheck.AddRange(taskBundle.tasks.OfType<MoveTask>()
                .Where(task => !loadingGateZones.Any(zone => zone.zoneId.Equals(task.destZone)))
                .ToList()
                .Select(pickTask => pickTask.toteId));

            var notReadyTotes = toteBarcodesToCheck
                .Where(barcode => !_toteRepository.IsReady(barcode)).ToList();

            //Throw exception if found not ready Totes found
            if (notReadyTotes.Count <= 0) return;
            _logger.LogError("Received not ready Totes: " + string.Join(";", notReadyTotes));
            throw new TotesNotReadyException() {Totes = notReadyTotes};
        }

        public void FailMoveTasksForTote(Tote tote)
        {
            try
            {
                var task = GetNonDeliveryMoveTask(tote.toteBarcode);
                if (task == null) return;
                UpdateTaskStatus(task.taskId, RcsTaskStatus.Faulted);
                CompleteTask(task);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Exception!");
            }
        }
    }
}