using Common;
using Common.Models;
using Microsoft.Extensions.Logging;
using PlcRequestQueueService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RcsLogic
{
    public class PickRequestDoneListenerRegistry : IPickRequestDoneListenerRegistry
    {
        private readonly ILogger<PickRequestDoneListenerRegistry> _logger;
        private List<IPickRequestDoneListener> _pickRequestDoneListeners = new List<IPickRequestDoneListener>();

        public PickRequestDoneListenerRegistry(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PickRequestDoneListenerRegistry>();
        }

        public async Task NotifyListeners(PickRequestDoneModel pickRequestDone)
        {
            _logger.LogDebug("Notifying subscribers about pick request done for part name {0} for source tote {1}",
                pickRequestDone?.partName, pickRequestDone?.sourceToteBarcode);
            _pickRequestDoneListeners.ForEach(listener => listener.ProcessPickRequestDone(pickRequestDone));
        }

        public void RegisterListener(IPickRequestDoneListener pickRequestDoneListener)
        {
            _logger.LogInformation("{0} subscribed to pick requests", pickRequestDoneListener.GetType());
            _pickRequestDoneListeners.Add(pickRequestDoneListener);
        }
    }
}