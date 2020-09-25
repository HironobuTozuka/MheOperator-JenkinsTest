using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RcsLogic.Watchdog
{
    public class ToteLocationUnknownWatchdog : IWatchdog
    {
        private readonly ToteRepository _toteRepository;
        private readonly IStoreManagementClient _storeManagementClient;
        private readonly ILogger<ToteLocationUnknownWatchdog> _logger;

        private readonly TimeSpan _toteNoLocationUpdateTimeout;
        public bool Enabled { get; set; } = true;

        public ToteLocationUnknownWatchdog(
            IConfiguration configuration,
            ILoggerFactory loggerFactory,
            ToteRepository toteRepository, 
            IStoreManagementClient storeManagementClient)
        {
            _logger = loggerFactory.CreateLogger<ToteLocationUnknownWatchdog>();
            _toteRepository = toteRepository;
            _storeManagementClient = storeManagementClient;
            Enabled = configuration["Watchdog:ToteUnknownLocationWatchdog:Enabled"]?.Equals(true.ToString(), StringComparison.CurrentCultureIgnoreCase) ?? true;
            var timeout = configuration["Watchdog:ToteUnknownLocationWatchdog:Timeout"];
            _toteNoLocationUpdateTimeout = string.IsNullOrEmpty(timeout) ? TimeSpan.FromMinutes(1) : TimeSpan.FromMinutes(int.Parse(timeout));
        }
        
        public void Execute()
        {
            var timedOutTotes = GetTimedOutTotes();
            if (!Enabled || !timedOutTotes.Any()) return;
            _logger.LogDebug("Setting totes with unknown location status to LocationUnknown{0}", string.Join(";", timedOutTotes));
            timedOutTotes.ForEach(SetStatusToLocationUnknown);
            timedOutTotes.ForEach(NotifySm);
        }
        
        public void ForceExecute()
        {
            var timedOutTotes = GetTimedOutTotes();
            if (!timedOutTotes.Any()) return;
            _logger.LogDebug("Setting totes with unknown location status to LocationUnknown{0}", string.Join(";", timedOutTotes));
            timedOutTotes.ForEach(SetStatusToLocationUnknown);
            timedOutTotes.ForEach(NotifySm);
        }

        public void SetStatusToLocationUnknown(Tote tote)
        {
            _logger.LogTrace("Setting tote: {0} status to LocationUnknown", tote);
            _toteRepository.UpdateToteStatus(tote, ToteStatus.LocationUnknown);
        }
        
        public void NotifySm(Tote tote)
        {
            _logger.LogTrace("Notifying SM about tote: {0} status LocationUnknown", tote);
            _storeManagementClient.ToteNotification(tote, null, ToteRotation.unknown, ToteStatus.LocationUnknown);
        }

        private List<Tote> GetTimedOutTotes()
        {
            var timedOutTotes = _toteRepository.GetTotesWithoutLocation()
                .Where(TimeoutElapsed())
                .ToList();
            _logger.LogDebug("Timed out totes with unknown location: {0}", string.Join(";", timedOutTotes));
            return timedOutTotes;
        }

        private Func<Tote, bool> TimeoutElapsed()
        {
            return tote => (DateTime.Now - tote.lastLocationUpdate) > _toteNoLocationUpdateTimeout;
        }
    }
}