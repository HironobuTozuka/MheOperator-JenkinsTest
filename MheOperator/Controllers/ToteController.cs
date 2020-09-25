using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common.Models.Location;
using Common.Models.Tote;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace MheOperator.Controllers
{
    public class ToteController : Controller
    {
        private Data.StoreDbContext _context;
        private readonly ILogger<ToteController> _logger;

        public ToteController(Data.StoreDbContext db, ILogger<ToteController> logger)
        {
            _context = db;
            //_logger = logger;
        }

        public IActionResult Index()
        {
            List<Tote> totes = _context.totes.Include(h => h.type).Include(h => h.location)
                .Include(h => h.storageLocation).ToList();

            return View("Index", totes);
        }

        public IActionResult AddTote()
        {
            var tote = new Tote();

            ViewData["context"] = _context;
            return View("AddToteView", tote);
        }


        public IActionResult DeleteTote(int toteId)
        {
            var tote = _context.totes.Find(toteId);
            _context.totes.Remove(tote);
            _context.SaveChanges();


            return RedirectToAction("Index");
        }

        public IActionResult ModifyTote(int toteId)
        {
            var tote = _context.totes.Find(toteId);
            ViewData["context"] = _context;
            return View("ModifyToteView", tote);
        }


        [HttpPost]
        public async Task<ActionResult> ModifyTote(Tote tote, string returnUrl)
        {
            var toteToModify = await _context.totes.FindAsync(tote.id);
            toteToModify.toteBarcode = tote.toteBarcode;
            toteToModify.typeId = tote.typeId;
            toteToModify.locationId = tote.locationId;
            toteToModify.storageLocationId = tote.storageLocationId;
            toteToModify.status = tote.status;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> AddTote(Tote tote, string returnUrl)
        {
            tote.typeId = (await _context.toteTypes.FindAsync(tote.typeId)).id;
            tote.locationId = _context.locations
                .First(h => h.storedTote == null && h.zone.function == LocationFunction.Storage).id;
            tote.storageLocationId = (int) tote.locationId;
            await _context.totes.AddAsync(tote);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}