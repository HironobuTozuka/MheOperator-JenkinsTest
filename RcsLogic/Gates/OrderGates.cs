using System;
using System.Collections.Generic;
using Common;
using Common.Models.Gate;
using Common.Models.Location;
using Common.Models.Tote;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;
using RcsLogic.Models.Device;
using RcsLogic.Services;

namespace RcsLogic.Gates
{
    public class OrderGates : Device, ISlotExposingDevice, IDeliveryCompleteDevice
    {
        private readonly ILogger<OrderGates> _logger;
        private readonly IPlcService _plcService;
        private readonly List<IReturnToteHandler> _returnTotesHandler = new List<IReturnToteHandler>();
        private readonly ServicedLocationProvider _servicedLocationsProvider;
        private readonly float _returnToteTimeout;
        public List<ServicedLocation> ServicedLocations => _servicedLocationsProvider.GetOrderGateServicedLocations();

        public OrderGates(DeviceId deviceId, IPlcService plcService, ILoggerFactory loggerFactory,
            TaskBundleService tasks, ServicedLocationProvider servicedLocationsProvider, IConfiguration configuration) : base(tasks, deviceId)
        {
            _logger = loggerFactory.CreateLogger<OrderGates>();
            _plcService = plcService;
            _servicedLocationsProvider = servicedLocationsProvider;
            _returnToteTimeout = string.IsNullOrEmpty(configuration["OrderGate:ReturnToteTimeout"])
                ? 15 : float.Parse(configuration["OrderGate:ReturnToteTimeout"]);
        }
        
        public void RegisterReturnHandler(IReturnToteHandler toteReleasedListener)
        {
            lock (this)
            {
                _logger.LogDebug("{0} subscribed to tote released by robot", toteReleasedListener.GetType());
                _returnTotesHandler.Add(toteReleasedListener);
            }
        }

        public void CompleteDelivery(Tote tote)
        {
            try
            {
                _logger.LogInformation("deviceId: {1}, sending back tote: {2}", _deviceId, tote);
                var completeDeliveryTask = _taskBundles.CompleteDeliveryTask(tote.toteBarcode);
                _logger.LogTrace("deviceId: {1}, Completed delivery task for tote: {2}, taskId: {3}", _deviceId,
                    tote, completeDeliveryTask);
                _returnTotesHandler.ForEach(it => it.ReturnTote(tote.toteBarcode));
                _logger.LogTrace("deviceId: {1}, returning tote: {2}", _deviceId, tote, completeDeliveryTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while sending tote back! ToteId: " + tote);
            }
        }

        public void Expose(SlotsToExpose slotsToExpose)
        {
            _logger.LogInformation("Order gate exposing slots: {0}", slotsToExpose);
            
            _plcService.OpenGate(new GateDescription()
                {gateId = Convert(slotsToExpose.Location), slotIndexes = slotsToExpose.Slots});
   
            _ = new DeliveryCompleteTrigger(slotsToExpose.Tote, _returnToteTimeout, this);
            _logger.LogTrace("Delivery complete trigger created");
        }

        private GateId Convert(Location location)
        {
            return new GateId(location.zone.plcGateId);
        }
    }
}