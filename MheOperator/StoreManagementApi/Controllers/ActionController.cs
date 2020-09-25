using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Models;
using Common.Models.Gate;
using Common.Models.Led;
using Common.Models.Location;
using Data;
using MheOperator.StoreManagementApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MheOperator.StoreManagementApi.Controllers
{
    [Route("api/internal")]
    [ApiController]
    public class ActionController : ControllerBase
    {
        private readonly ILogger<ActionController> _logger;
        private readonly IPlcService _plcService;
        private readonly LocationRepository _locationRepository;

        public ActionController(ILogger<ActionController> logger, IPlcService plcService,
            LocationRepository locationRepository)
        {
            _logger = logger;
            _plcService = plcService;
            _locationRepository = locationRepository;
        }

        [HttpPost("action:led")]
        public IActionResult PostLed(LedPostModel leds)
        {
            _logger.LogInformation("Got request to switch on leds: {0}", leds.ledIds);
            foreach (var ledId in Enumerable.Range(0, 3))
            {
                _plcService.SwitchLed(Convert(ledId), leds.ledIds.Contains(ledId) ? LedState.On : LedState.Off);
            }

            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost("action:open")]
        public IActionResult OpenGate(GatePostModel gatePostModel)
        {
            _logger.LogInformation("Got request to open gate: {0}", gatePostModel.gateId);
            if (!_locationRepository.ZoneExists(gatePostModel.gateId)) return StatusCode(StatusCodes.Status404NotFound);
            _plcService.OpenGate(Convert(gatePostModel.gateId));
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost("action:close")]
        public IActionResult CloseGate(GatePostModel gatePostModel)
        {
            _logger.LogInformation("Got request to close gate: {0}", gatePostModel.gateId);
            if (!_locationRepository.ZoneExists(gatePostModel.gateId)) return StatusCode(StatusCodes.Status404NotFound);
            _plcService.CloseGate(Convert(gatePostModel.gateId));
            return StatusCode(StatusCodes.Status200OK);
        }

        private LedId Convert(int ledId)
        {
            try
            {
                return (LedId) ledId;
            }
            catch
            {
                throw new Exception("Not implemented Led ID");
            }
        }

        private GateDescription Convert(ZoneId zoneId)
        {
            var zone = _locationRepository.GetZoneById(zoneId);
            if (zone.function == LocationFunction.OrderGate)
            {
                return new GateDescription()
                {
                    gateId = new GateId(zone.plcGateId),
                    slotIndexes = new List<int>(){0, 1}
                };
            }
            return new GateDescription()
            {
                gateId = new GateId(zone.plcGateId)
            };
        }
    }
}