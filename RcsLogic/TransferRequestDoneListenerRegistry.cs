using Common;
using Common.Models;
using Microsoft.Extensions.Logging;
using PlcRequestQueueService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RcsLogic
{
    public class TransferRequestDoneListenerRegistry : ITransferRequestDoneListenerRegistry
    {
        private readonly ILogger<TransferRequestDoneListenerRegistry> _logger;

        private List<ITransferRequestDoneListener> _transferRequestDoneListeners =
            new List<ITransferRequestDoneListener>();

        public TransferRequestDoneListenerRegistry(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TransferRequestDoneListenerRegistry>();
        }

        public async Task NotifyListeners(TransferRequestDoneModel transferRequestDone)
        {
            _logger.LogDebug("Notifying subscribers about transfer request done for {0}",
                transferRequestDone.ToString());
            _transferRequestDoneListeners.ForEach(listener => listener.ProcessTransferRequestDone(transferRequestDone));
        }

        public void RegisterListener(ITransferRequestDoneListener transferRequestDoneListener)
        {
            _logger.LogInformation("{0} subscribed to transfer requests", transferRequestDoneListener.GetType());
            _transferRequestDoneListeners.Add(transferRequestDoneListener);
        }
    }
}