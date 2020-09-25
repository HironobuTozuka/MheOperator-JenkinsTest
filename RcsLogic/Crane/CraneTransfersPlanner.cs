using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RcsLogic.Models.Device;
using RcsLogic.Models;
using RcsLogic.Services;

namespace RcsLogic
{
    public class CraneTransfersPlanner
    {
        private readonly RoutingService _routingService;
        private readonly ILogger<CraneTransfersPlanner> _logger;

        public CraneTransfersPlanner(ILoggerFactory loggerFactory,
            RoutingService routingService)
        {
            _routingService = routingService;
            _logger = loggerFactory.CreateLogger<CraneTransfersPlanner>();
        }

        // ReSharper disable once ParameterTypeCanBeEnumerable.Global
        public CraneTransfersPlan PlanTransfer(List<Transfer> craneRequests, DeviceId deviceId)
        {
            //TODO Next step: get all crane requests here, and try to pair not only second request in queue but also third, fourth etc.
            _logger.LogInformation("Crane requests to distribute: {0}", craneRequests);
            var requestsToDistribute = craneRequests.Select(it => GetPossibleShelvesForRequest(it, deviceId)).ToList();
            var transfersForShelves = new Dictionary<CraneState.Shelf, Transfer>();

            // request to distribute:
            //     request
            //     possible shelves

            // while (no requests)
            //    First with 1 shelve => assign shelve
            //    None with 1 shelve => First => assign shelve
            //        if(nothing assigned) break;
            while (requestsToDistribute.Count > 0)
            {
                _logger.LogInformation("Requests to distribute: {0}", requestsToDistribute);
                var request = requestsToDistribute.FirstOrDefault(it => it.PossibleShelves.Count == 1) ??
                              requestsToDistribute.First();
                var chosenShelf = request.PossibleShelves.FirstOrDefault();

                if (chosenShelf == null)
                {
                    _logger.LogInformation("Couldn't assign further shelves, plan is finished;");
                    break;
                }

                _logger.LogInformation("Assigning {0} to {1}", request, chosenShelf);

                requestsToDistribute.Remove(request);
                transfersForShelves[chosenShelf] = request.Request;
                requestsToDistribute.ToList().ForEach(it => it.PossibleShelves.Remove(chosenShelf));
            }

            return new CraneTransfersPlan(transfersForShelves);
        }


        private RequestWithPossibleShelves GetPossibleShelvesForRequest(Transfer request, DeviceId deviceId)
        {
            if (request == null)
            {
                return null;
            }

            var result = new List<CraneState.Shelf>();
            var possibleShelvesForRequest =
                _routingService.GetShelfIds(request.sourceLocation, request.destLocation, deviceId);
            if (possibleShelvesForRequest.Contains(1)) result.Add(CraneState.Shelf.First);
            if (possibleShelvesForRequest.Contains(2)) result.Add(CraneState.Shelf.Second);
            return new RequestWithPossibleShelves(request, result);
        }
    }

    internal class RequestWithPossibleShelves
    {
        public Transfer Request { get; }
        public List<CraneState.Shelf> PossibleShelves { get; }

        public RequestWithPossibleShelves(Transfer request, List<CraneState.Shelf> possibleShelves)
        {
            Request = request;
            PossibleShelves = possibleShelves;
        }

        public override string ToString()
        {
            var formattableString = $"{nameof(Request)}: {Request}, {nameof(PossibleShelves)}: ";
            PossibleShelves.ForEach(it => formattableString += it + ",");
            return formattableString;
        }
    }
}