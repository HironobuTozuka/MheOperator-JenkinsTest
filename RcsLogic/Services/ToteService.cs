using System.Linq;
using Common;
using Common.Models;
using Common.Models.Location;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Logging;

namespace RcsLogic.Services
{
    public class ToteService
    {
        private readonly ILogger<ToteService> _logger;
        private readonly ToteRepository _toteRepository;
        private readonly LocationRepository _locationRepository;
        private readonly IStoreManagementClient _storeManagementClient;

        public ToteService(ILoggerFactory loggerFactory, ToteRepository toteRepository, LocationRepository locationRepository,
            IStoreManagementClient storeManagementClient)
        {
            _toteRepository = toteRepository;
            _locationRepository = locationRepository;
            _storeManagementClient = storeManagementClient;
            _logger = loggerFactory.CreateLogger<ToteService>();
        }
        
        public void SaveTote(ScanNotificationModel scanNotification)
        {
            var toteType = _toteRepository.GetToteType(scanNotification.ToteType.ToteHeight,
                scanNotification.ToteType.TotePartitioning);
            var location = _locationRepository.GetLocationByPlcId(scanNotification.LocationId);
            var tote = _toteRepository.GetToteByBarcode(scanNotification.ToteBarcode);
            if (tote == null)
            {
                //if tote is on loading gate
                if (location.zone.function == LocationFunction.LoadingGate)
                {
                    var toteStoredOnLoadingGate = _toteRepository.GetToteOnStorageLocation(location);
                    if (toteStoredOnLoadingGate != null) _toteRepository.Remove(toteStoredOnLoadingGate);
                    tote = CreateTote(scanNotification.ToteBarcode, toteType, location);
                }
                else
                {
                    tote = CreateTote(scanNotification.ToteBarcode, toteType,
                        _locationRepository.GetLocationByFunction(LocationFunction.Technical));
                    _toteRepository.UpdateToteStatus(tote, ToteStatus.ZoneNotAssigned);
                }

                _logger.LogDebug("Created new tote: {0} on location {1}", scanNotification.ToteBarcode,
                    scanNotification.LocationId);
            }

            _toteRepository.UpdateToteLocation(tote, location);
            if (tote.status != ToteStatus.LocationUnknown) return;
            _logger.LogDebug("Sending notification to SM that the tote {0} location in known now", tote);
            _toteRepository.UpdateToteStatus(tote, ToteStatus.Ready);
            _storeManagementClient.ToteNotification(tote, location, scanNotification.ToteRotation, tote.status);
        }
        
        public Tote CreateNoReadTote(Location storageLocation, Location currentLocation)
        {
            var newBarcode = NewNoReadBarcode();
            var unknownToteType = _toteRepository.GetToteType(ToteHeight.unknown, TotePartitioning.unknown);
            
            var tote = new Tote()
            {
                toteBarcode = newBarcode,
                typeId = unknownToteType.id,
                storageLocationId = storageLocation.id,
                locationId = currentLocation.id,
                status = ToteStatus.NoRead
            };
            _toteRepository.Add(tote);
            _logger.LogDebug("Created new tote: {0} on location {1}, with storage location {2}", tote.toteBarcode,
                currentLocation, storageLocation);
            return _toteRepository.GetToteByBarcode(newBarcode);
        }

        private string NewNoReadBarcode()
        {
            var toteIds = _toteRepository.GetTotesWithBarcodeContaining(Barcode.NoRead)
                .Select(tote => tote.toteBarcode.Replace(Barcode.NoRead, ""))
                .Select(toteId =>
                {
                    int.TryParse(toteId, out var id);
                    return id;
                }).ToList();
            toteIds.Sort();
            var lastId = toteIds.LastOrDefault();
            var newBarcode = Barcode.NoRead + (lastId + 1);
            return newBarcode;
        }

        private Tote CreateTote(string toteBarcode, ToteType toteType, Location storageLocation)
        {
            var tote = new Tote()
            {
                toteBarcode = toteBarcode,
                typeId = toteType.id,
                storageLocationId = storageLocation.id
            };
            _toteRepository.Add(tote);
            return tote;
        }

        public void SavePreviousToteAsNoReadOnLoadingGate(ScanNotificationModel scanNotification)
        {
            if(scanNotification.ToteBarcode != Barcode.NoRead) return;
            var location  =_locationRepository.GetLocationByPlcId(scanNotification.LocationId);
            if (location.zone.function != LocationFunction.LoadingGate) return;
            _logger.LogDebug("Received NOREAD on loading gate, assuming tote on loading gate is the tote," +
                             " that previously was on loading gate's downstream location");
            var tote = _toteRepository.GetToteOnLocation(_locationRepository.GetDownstreamLocation(location).id);
            if (tote != null) _toteRepository.UpdateToteLocation(tote, location);
        }

        public void DeleteToteIfNoToteOnLoadingGate(ScanNotificationModel scanNotification)
        {
            if(scanNotification.ToteBarcode != Barcode.NoTote) return;
            var location  =_locationRepository.GetLocationByPlcId(scanNotification.LocationId);
            if (location.zone.function != LocationFunction.LoadingGate) return;
            var tote = _toteRepository.GetToteOnLocation(location.id);
            _logger.LogDebug("Received NOTOTE on loading gate, deleting tote on loading gate: {1}", tote);
            if (tote != null) _toteRepository.Remove(tote);
        }
    }
}