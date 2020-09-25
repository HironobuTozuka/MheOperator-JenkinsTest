using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PlcCommunicationService.Models;


namespace PlcCommunicationService
{
    public abstract class PlcConnection : IPlcConnection
    {
        protected readonly ILogger Logger;
        private readonly ILoggerFactory loggerFactory;
        protected MoveRequestSender MoveRequestSender;
        protected OpcClient OpcClient;

        public PlcConnection(ILoggerFactory loggerFactory, ILogger logger)
        {
            this.loggerFactory = loggerFactory;
            Logger = logger;
        }

        public async Task SendMoveRequest(MoveRequest moveRequest)
        {
            Logger.LogInformation("Move request enqueued in OpcClient:: {1}", moveRequest.ToString());
            await MoveRequestSender.Send(moveRequest);
        }
    }
}