using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Models;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;
using Data;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using RcsLogic.Models;

namespace RcsLogic.Robot
{
    public class TotesReadyForPicking : IScanNotificationListener
    {
        private readonly List<ToteReadyForPicking> _totesReadyForPicking = new List<ToteReadyForPicking>();
        private readonly ILogger<TotesReadyForPicking> _logger;

        public TotesReadyForPicking(ILoggerFactory loggerFactory, ToteRepository toteRepository, LocationRepository locationRepository)
        {
            _logger = loggerFactory.CreateLogger<TotesReadyForPicking>();
            _totesReadyForPicking.AddRange(
            locationRepository.GetLocationsByFunction(LocationFunction.Place)
                .Select(location => toteRepository.GetToteOnLocation(location.id))
                .Where(tote => tote != null)
                .Select(tote => new ToteReadyForPicking()
                {
                    Blocked = false,
                    Location = tote.location,
                    Tote = tote
                }));
        }

        public void Add(PrepareForPicking toteReady)
        {
            var tote = _totesReadyForPicking.FirstOrDefault(t => t.Tote.Equals(toteReady.Tote));
            if (tote != null)
            {
                tote.Status = ToteReadyForPickingStatus.Ready;
                tote.ToteRotation = toteReady.ToteRotation;
                return;
            }
            tote = new ToteReadyForPicking() {Tote = toteReady.Tote, Location = toteReady.Location, ToteRotation = toteReady.ToteRotation};
            _totesReadyForPicking.RemoveAll(t => 
                t.Location.Equals(tote.Location) || t.Tote.toteBarcode.Equals(tote.Tote.toteBarcode));
            _totesReadyForPicking.Add(tote);
            _logger.LogTrace("Added tote ready for picking {0}", tote);
        }

        public void Remove(Tote tote)
        {
            _totesReadyForPicking.RemoveAll(t => t.Tote.toteBarcode.Equals(tote.toteBarcode));
            _logger.LogTrace("Removed tote ready for picking {0}", tote);
        }

        public void Remove(ToteReadyForPicking toteReady)
        {
            _totesReadyForPicking.RemoveAll(t => 
                t.Location.Equals(toteReady.Location) || t.Tote.toteBarcode.Equals(toteReady.Tote.toteBarcode));
            _logger.LogTrace("Removed tote ready for picking {0}", toteReady);
        }

        public void Block(Tote tote)
        {
            if (tote == null) return;
            _totesReadyForPicking.First(t => t.Tote.id.Equals(tote.id)).Blocked = true;
        }
        
        public void Release(string toteBarcode)
        {
            if (!_totesReadyForPicking.Any(t => t.Tote.toteBarcode.Equals(toteBarcode))) return;
            _totesReadyForPicking.First(t => t.Tote.toteBarcode.Equals(toteBarcode)).Blocked = false;
        }

        public bool IsBlocked(Tote tote)
        {
            return _totesReadyForPicking.Any(t => t.Tote.id.Equals(tote.id)) 
                   && _totesReadyForPicking.First(t => t.Tote.id.Equals(tote.id)).Blocked;
        }

        public List<ToteReadyForPicking> ToList()
        {
            return _totesReadyForPicking
                .Where(tote => tote.Status == ToteReadyForPickingStatus.Ready)
                .ToList();
        }
        
        public ToteReadyForPicking GetTote(string barcode)
        {
            return _totesReadyForPicking.FirstOrDefault(tote => tote.Tote.toteBarcode.Equals(barcode));
        }
        
        public ToteReadyForPicking GetTote(PickTask currentTask, Func<PickTask, PickToteData> toteSelector)
        {
            return _totesReadyForPicking.FirstOrDefault(tote =>
                currentTask != null && tote.Tote.toteBarcode.Equals(toteSelector.Invoke(currentTask).toteId));
        }
        public List<ToteReadyForPicking> GetTotesToBeReleased(TaskBundle currentTaskBundle)
        {
            return _totesReadyForPicking.Where(tote => currentTaskBundle.tasks.OfType<PickTask>()
                    .Where(task => task.taskStatus != RcsTaskStatus.Complete)
                    .Where(task => task.taskStatus != RcsTaskStatus.Faulted)
                    .All(task =>
                        task.sourceTote.toteId != tote.Tote.toteBarcode &&
                        task.destTote.toteId != tote.Tote.toteBarcode))
                .ToList();
        }

        public void ProcessScanNotification(ScanNotificationModel scanNotification)
        {
            _totesReadyForPicking.RemoveAll(t =>
                t.Tote.toteBarcode.Equals(scanNotification.ToteBarcode) 
                && !t.Location.plcId.Equals(scanNotification.LocationId));
        }
    }
}