using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Models.Gate;
using Common.Models.Led;
using Common.Models.Location;
using Data;
using MheOperator.StoreManagementApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RcsLogic.Watchdog;

namespace MheOperator.Controllers
{
    public class FeatureController : Controller
    {
        private readonly ILogger<FeatureController> _logger;
        private readonly ToteLocationWatchdog _toteLocationWatchdog;
        private readonly TaskBundleWatchdog _taskBundleWatchdog;

        public FeatureController(ILogger<FeatureController> logger,
            ToteLocationWatchdog toteLocationWatchdog,
            TaskBundleWatchdog taskBundleWatchdog)
        {
            _logger = logger;
            _toteLocationWatchdog = toteLocationWatchdog;
            _taskBundleWatchdog = taskBundleWatchdog;
        }
        
        public IActionResult Index()
        {
            return View();
        }
    }
}