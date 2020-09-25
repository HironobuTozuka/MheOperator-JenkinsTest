using Common;
using MheOperator.StoreManagementApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MheOperator.StoreManagementApi.Controllers
{
    [Route("api/internal")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly ILogger<ActionController> _logger;
        private readonly IPlcService _plcService;

        public HealthCheckController(ILogger<ActionController> logger, IPlcService plcService)
        {
            _logger = logger;
            _plcService = plcService;
        }
        
        [HttpGet("health-check")]
        public IActionResult GetHealthCheck()
        {
            return StatusCode(StatusCodes.Status200OK, new RcsStatusModel(_plcService.IsPlcInExecute()));
        }

    }
}