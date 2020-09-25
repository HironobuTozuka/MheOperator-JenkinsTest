using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common.Models.Location;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;


namespace MheOperator.Controllers
{
    public class LocationController : Controller
    {
        private readonly Data.StoreDbContext _context;
        private readonly ILogger<LocationController> _logger;

        public LocationController(Data.StoreDbContext db, ILogger<LocationController> logger)
        {
            _context = db;
            _logger = logger;
        }
        public IActionResult Index()
        {
            _logger.LogInformation("just location opened");
            return View();
        }

        public IActionResult RackA1View()
        {
            ViewData["context"] = _context;
            return PartialView();
        }

        public IActionResult RackA2View()
        {
            ViewData["context"] = _context;
            return PartialView();
        }

        public IActionResult RackB1View()
        {
            ViewData["context"] = _context;
            return PartialView();
        }

        public IActionResult RackB2View()
        {
            ViewData["context"] = _context;
            return PartialView();
        }

        public IActionResult ConveyorView()
        {
            ViewData["context"] = _context;
            return PartialView();
        }

        public IActionResult ServiceLocationsView()
        {
            ViewData["context"] = _context;
            return PartialView();
        }

        public IActionResult OvwerviewView()
        {
            ViewData["context"] = _context;
            return PartialView();
        }

        /// <summary>
        /// Executed when location is pressed on rack 
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public IActionResult LocationPress(int locationId)
        {
            ViewData["context"] = _context;
            //if (_context.locations.Include(h => h.storedTote.type).First(h => h.id == locationId).storedTote == null) return new EmptyResult();
            return PartialView("LocationInfoView", _context.locations.Include(h => h.storedTote.type).FirstOrDefault(h => h.id == locationId));
        }

        /// <summary>
        /// Enables rack location
        /// </summary>
        /// <param name="id">id of location to enable</param>
        /// <returns></returns>
        public IActionResult EnableLocation(int id)
        {
            ViewData["context"] = _context;
            var location = _context.locations.Find(id);
            if (location.status != LocationStatus.NotAccessible)
            {
                location.status = LocationStatus.Enabled;
                _context.SaveChanges();
            }
            switch (location.rack)
            {
                case "A1":
                    return PartialView("RackA1View");
                case "A2":
                    return PartialView("RackA2View");
                case "B1":
                    return PartialView("RackB1View");
                case "B2":
                    return PartialView("RackB2View");
                default:
                    return PartialView("OvwerviewView");
            }
            
        }

        /// <summary>
        /// Disables rack location
        /// </summary>
        /// <param name="id">id of location to disable</param>
        /// <returns></returns>
        public IActionResult DisableLocation(int id)
        {
            ViewData["context"] = _context;
            var location = _context.locations.Find(id);
            location.status = LocationStatus.Disabled;
            _context.SaveChanges();
            switch (location.rack)
            {
                case "A1":
                    return PartialView("RackA1View");
                case "A2":
                    return PartialView("RackA2View");
                case "B1":
                    return PartialView("RackB1View");
                case "B2":
                    return PartialView("RackB2View");
                default:
                    return PartialView("OvwerviewView");
            }
        }



    }
}

