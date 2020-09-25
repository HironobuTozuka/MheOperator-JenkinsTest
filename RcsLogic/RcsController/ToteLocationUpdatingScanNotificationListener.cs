using Common;
using Common.Models;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Logging;
using RcsLogic.Services;

namespace RcsLogic.RcsController
{
    public class ToteLocationUpdatingScanNotificationListener : IScanNotificationListener
    {
        private readonly ILogger<ToteLocationUpdatingScanNotificationListener> _logger;
        private readonly ToteService _toteService;

        public ToteLocationUpdatingScanNotificationListener(ILoggerFactory loggerFactory, ToteService toteService)
        {
            _logger = loggerFactory.CreateLogger<ToteLocationUpdatingScanNotificationListener>();
            _toteService = toteService;
        }

        public void ProcessScanNotification(ScanNotificationModel scanNotification)
        {
            _logger.LogInformation("Received scan notification. Tote {0} position {1}", scanNotification.ToteBarcode,
                scanNotification.LocationId);
            if (!scanNotification.IsWrongScan())
            {
                _toteService.SaveTote(scanNotification);
                _logger.LogDebug("Received scan notification. Tote {0} position {1} stored in db",
                    scanNotification.ToteBarcode, scanNotification.LocationId);
            }
            else
            {
                _toteService.DeleteToteIfNoToteOnLoadingGate(scanNotification);
                _toteService.SavePreviousToteAsNoReadOnLoadingGate(scanNotification);
                _logger.LogDebug("Received wrong tote barcode: {0} on location {1}",
                    scanNotification.ToteBarcode, scanNotification.LocationId);
            }
        }
    }
}