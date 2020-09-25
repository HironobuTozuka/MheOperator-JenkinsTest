using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;
using Microsoft.Extensions.Logging;
using RcsLogic.Models.Device;
using RcsLogic.Robot;

namespace RcsLogic.Models
{
    public class TransferCollection
    {
        private readonly List<Transfer> _craneTransfers = new List<Transfer>();
        private readonly ILogger<TransferCollection> _logger;
        private readonly DeviceId _deviceId;
        private readonly LocationStatus _locationStatus;
        private readonly TotesReadyForPicking _totesReadyForPicking;
        private readonly List<string> _priorityTotes = new List<string>();

        public void AddPriorityTote(string barcode)
        {
            _priorityTotes.Add(barcode);
            _logger.LogInformation("deviceId: {1}, Priority tote added: {2}, total priority totes: {3}",
                _deviceId, barcode, prepareBarcodesLog(_priorityTotes));
        }
        
        public void RemovePriorityTote(string barcode)
        {
            if(_priorityTotes.All(tote => !tote.Equals(barcode))) return;
            _priorityTotes.Remove(barcode);
            _logger.LogInformation("deviceId: {1}, Priority tote removed: {2}, total priority totes: {3}",
                _deviceId, barcode, prepareBarcodesLog(_priorityTotes));
        }

        public TransferCollection(ILoggerFactory loggerFactory, DeviceId deviceId, LocationStatus locationStatus, 
            TotesReadyForPicking totesReadyForPicking)
        {
            _logger = loggerFactory.CreateLogger<TransferCollection>();
            _deviceId = deviceId;
            _locationStatus = locationStatus;
            _totesReadyForPicking = totesReadyForPicking;
        }

        public bool IsPriorityTote(Tote tote)
        {
            return _priorityTotes.Any(t => t.Equals(tote.toteBarcode));
        }

        public void SetExecuteStatus(Transfer craneRequest)
        {
            lock (this)
            {
                var requestToUpdate = _craneTransfers.FirstOrDefault(request => Equals(request, craneRequest));
                if (requestToUpdate != null) requestToUpdate.status = Transfer.RequestStatus.Execute;
            }
        }
        
        public void ResetExecuteStatus(ToteTransferRequestDoneModel transferRequestDone)
        {
            lock (this)
            {
                if (transferRequestDone == null) return;
                var requestsToUpdate = _craneTransfers
                    .Where(request => RequestMatchesTransferDone(transferRequestDone, request)).ToList();
                requestsToUpdate.ForEach(request => request.status = Transfer.RequestStatus.Idle);
                
            }
        }

        public List<Transfer> GetRequestsInExecution()
        {
            lock (this)
            {
                return _craneTransfers.Where(request => request.status == Transfer.RequestStatus.Execute).ToList();
            }
        }

        public void Add(Transfer craneRequest)
        {
            lock (this)
            {
                _craneTransfers.Add(craneRequest);
            }

            LogQueueStatus("Queue status after adding a request");
        }

        public List<Transfer> GetUpTo(int maxNumberOfRequests)
        {
            lock (this)
            {
                _logger.LogDebug("deviceId: {0}, All requests: {1}", _deviceId, prepareLog(_craneTransfers));

                //Get all executable requests (where dest location is currently not occupied)
                var craneRequestsToConsider = _craneTransfers
                    .Where(request => request.sourceLocation.Equals(request.destLocation) ||
                                      !_locationStatus.IsLocationOccupied(request.destLocation))
                    .Where(request => !_totesReadyForPicking.IsBlocked(request.tote)).ToList();
                    
                _logger.LogDebug("deviceId: {0}, Requests without occupied location: {1}", _deviceId,
                    prepareLog(craneRequestsToConsider));

                //First check for requests from crane -> if something has to be moved from crane location then
                //crane has to be freed up first
                if (craneRequestsToConsider.Any(request =>
                    request.sourceLocation.zone.function == LocationFunction.Crane))
                {
                    _logger.LogDebug("deviceId: {1}, prioritising totes to transfer from crane", _deviceId);
                    craneRequestsToConsider = craneRequestsToConsider
                        .Where(it => it.sourceLocation.zone.function == LocationFunction.Crane).ToList();
                    _logger.LogDebug("deviceId: {0} priority requests: {1}", _deviceId,
                        prepareLog(craneRequestsToConsider));
                }
                //If no requests from crane, then check for existing priority totes
                else if (_priorityTotes.Count > 0)
                {
                    _logger.LogDebug("deviceId: {1}, priority totes: {2}", _deviceId,
                        prepareBarcodesLog(_priorityTotes));
                    var priorityRequests = craneRequestsToConsider
                        .Where(it => _priorityTotes.First().Equals(it.tote.toteBarcode)).ToList();
                    if (craneRequestsToConsider.Count == 0)
                    {
                        //Find blocked priority requests
                        var blockedPriorityRequests = _craneTransfers.Where(request =>
                            _priorityTotes.Any(tote => tote.Equals(request.tote.toteBarcode))).ToList();
                        _logger.LogDebug("deviceId: {1}, found blocked priority tote requests: {2}", _deviceId,
                            string.Join(",", blockedPriorityRequests));
                        //Prioritise all executable requests where source location is the same as
                        //blocked request's dest location to unblock the prioritised transfer 
                        priorityRequests = craneRequestsToConsider.Where(request =>
                            blockedPriorityRequests.Any(priorityRequest =>
                                priorityRequest.destLocation.id.Equals(request.sourceLocation.id))).ToList();
                        _logger.LogDebug("deviceId: {1}, prioritising following requests to " +
                                         "unblock priority tote requests: {2}", _deviceId,
                            string.Join(",", priorityRequests));
                    }

                    craneRequestsToConsider = priorityRequests;
                    _logger.LogDebug("deviceId: {0} priority requests: {1}", _deviceId,
                        prepareLog(craneRequestsToConsider));
                }

                //If there are more than one requests check for requests that can be executed simultaneously
                //by crane (two platforms) 
                if (craneRequestsToConsider.Count > 1)
                {
                    craneRequestsToConsider = craneRequestsToConsider.Where(it =>
                            RequestsHaveCommonSourceOrDest(craneRequestsToConsider[0], it))
                        .ToList();
                    _logger.LogDebug("deviceId: {0} requests that can be paired: {1}", _deviceId,
                        prepareLog(craneRequestsToConsider));
                }

                craneRequestsToConsider = craneRequestsToConsider
                    .GetRange(0, Math.Min(craneRequestsToConsider.Count, maxNumberOfRequests));
                _logger.LogDebug("Requests to execute: {1}", prepareLog(craneRequestsToConsider));

                if (craneRequestsToConsider.Count > 0)
                {
                    _logger.LogDebug("a");
                }

                return craneRequestsToConsider;
            }
        }

        private string prepareLog(List<Transfer> craneRequestsToConsider)
        {
            string requestsWithoutOccupiedLocation = "";
            craneRequestsToConsider?.ForEach(it => requestsWithoutOccupiedLocation += it + ",");
            return requestsWithoutOccupiedLocation;
        }

        private string prepareBarcodesLog(List<string> toteBarcodes)
        {
            string requestsWithoutOccupiedLocation = "";
            toteBarcodes?.ForEach(it => requestsWithoutOccupiedLocation += it + ",");
            return requestsWithoutOccupiedLocation;
        }

        private static bool RequestsHaveCommonSourceOrDest(Transfer request1, Transfer request2)
        {
            if (Equals(request1, request2)) return true;

            return request2 != null && (request1.destLocation.plcId.Contains("CNV")
                                        && request2.destLocation.plcId.Contains("CNV")
                                        || request1.sourceLocation.plcId.Contains("CNV")
                                        && request2.sourceLocation.plcId.Contains("CNV"));
        }

        public void AddRange(List<Transfer> craneRequests)
        {
            if (craneRequests != null && craneRequests.Count > 0)
            {
                lock (this)
                {
                    _craneTransfers.AddRange(craneRequests);
                }

                LogRequestRange("Added a range of requests to queue", craneRequests);
            }
        }

        public void AddNonOverlapingRange(List<Transfer> craneRequests)
        {
            if (craneRequests != null && craneRequests.Count > 0)
            {
                lock (this)
                {
                    _craneTransfers.AddRange(craneRequests.Where(it =>
                        _craneTransfers.All(request => request.tote.toteBarcode != it.tote.toteBarcode)).ToList());
                }

                LogRequestRange("Added a range of requests to queue", craneRequests);
            }
        }

        public void InsertNonOverlapping(Transfer craneRequest)
        {
            lock (this)
            {
                if(_craneTransfers.Any(transfer => transfer.tote.Equals(craneRequest.tote)
                   && transfer.sourceLocation.Equals(craneRequest.sourceLocation)
                   && transfer.destLocation.Equals(craneRequest.destLocation)))
                {
                    _logger.LogDebug("Request {0} not inserted, since there is identical request already");
                    return;
                }
                _craneTransfers.Insert(0, craneRequest);
            }

            LogQueueStatus("Queue status after inserting a request");
        }

        public Transfer GetByTaskId(TaskId taskId)
        {
            lock (this)
            {
                return _craneTransfers.FirstOrDefault(request => Equals(request.task.taskId, taskId));
            }
        }

        public bool AnyMatchingRequestDone(ToteTransferRequestDoneModel transferRequestDone)
        {
            lock (this)
            {
                return _craneTransfers.Count > 0
                       && _craneTransfers.Any(request => RequestMatchesTransferDone(transferRequestDone, request));
            }
        }

        public Transfer GetMatchingRequestDone(ToteTransferRequestDoneModel transferRequestDone)
        {
            lock (this)
            {
                return _craneTransfers.FirstOrDefault(request => RequestMatchesTransferDone(transferRequestDone, request));
            }
        }

        public void RemoveAllMatchingRequestDone(ToteTransferRequestDoneModel transferRequestDone)
        {
            lock (this)
            {
                if (transferRequestDone == null) return;
                _craneTransfers.RemoveAll(request => RequestMatchesTransferDone(transferRequestDone, request));

                var remove = _priorityTotes.Remove(transferRequestDone.sourceToteBarcode);

                if (remove)
                    _logger.LogInformation("deviceId: {1}, Priority tote removed: {2}, total priority totes: {3}",
                        _deviceId, transferRequestDone.sourceToteBarcode, prepareBarcodesLog(_priorityTotes));
            }
        }

        private static bool RequestMatchesTransferDone(ToteTransferRequestDoneModel transferRequestDone, Transfer request)
        {
            return Equals(request.task.taskId, transferRequestDone.requestId.GetTaskId())
                   && request.destLocation.plcId.Contains(transferRequestDone
                       .requestedDestLocationId)
                   && request.sourceLocation.plcId.Contains(transferRequestDone
                       .sourceLocationId);
        }
        public void Remove(Transfer craneRequest)
        {
            lock (this)
            {
                _craneTransfers.Remove(craneRequest);
            }
        }

        public void LogQueueStatus(string message)
        {
            LogRequestRange(message, _craneTransfers);
        }

        private void LogRequestRange(string message, List<Transfer> craneRequests)
        {
            lock (this)
            {
                string requests = " ";
                craneRequests.ForEach
                (
                    craneRequest =>
                        requests += " ===> request Id: " + craneRequest?.task?.taskId + " for tote: " +
                                    craneRequest?.tote?.toteBarcode + " from location: " +
                                    craneRequest?.sourceLocation?.plcId + " to location: " +
                                    craneRequest?.destLocation?.plcId + ";"
                );

                _logger.LogInformation("deviceId: {0},  {1}, requests: {2}", _deviceId, message, requests);
            }
        }

        public void RemoveNotStartedRequestsForTask(TaskBase task)
        {
            _craneTransfers.RemoveAll(transfer =>
                transfer.status == Transfer.RequestStatus.Idle && transfer.task.Equals(task));
        }
    }
}