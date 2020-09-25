using System.Threading.Tasks;
using Workstation.ServiceModel.Ua;
using Workstation.ServiceModel.Ua.Channels;
using Microsoft.Extensions.Logging;
using PlcCommunicationService.Models;
using PlcCommunicationService.SystemPlc.Models;

namespace PlcCommunicationService.SystemPlc
{
    public class InSignals : SignalReader, IInSignals
    {
        public CnvStatus CnvStatus { get; private set; }
        public Status Status { get; private set; }

        public InSignals(UaTcpSessionChannel channel, ILoggerFactory loggerFactory) : base(channel,
            loggerFactory.CreateLogger<SignalReader>())
        {
            CnvStatus = new CnvStatus(loggerFactory, channel);
            Status = new Status(loggerFactory, channel);
        }

        /// <summary>
        /// Gets the value of MoveRequestConf -> information about success or failure of the robot picking process.
        /// </summary>
        public async Task<IMoveRequestConf> GetMoveRequestConfAsync()
        {
            return (await ReadVariable(Declarations.NodePaths.MoveRequestConf))
                .GetValueOrDefault<InheritedModels.MoveRequestConf>();
        }

        /// <summary>
        /// Gets the value of ScanNotification -> information about a tote, that appeared on the decition point.
        /// </summary>
        public async Task<InheritedModels.ScanNotification> GetScanNotificationAsync()
        {
            return (await ReadVariable(Declarations.NodePaths.ScanNotification))
                .GetValueOrDefault<InheritedModels.ScanNotification>();
        }

        public async Task<bool> GetReadScanNotificationAsync()
        {
            return (await ReadVariable(Declarations.NodePaths.ReadScanNotification)).GetValueOrDefault<bool>();
        }

        public bool GetReadScanNotification()
        {
            var task = GetReadScanNotificationAsync();
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Gets the value of ReadMoveRequestConf -> trigger for RCS OS to read the MoveRequestConf data.
        /// </summary>
        public async Task<bool> GetReadMoveRequestConfAsync()
        {
            return (await ReadVariable(Declarations.NodePaths.ReadMoveRequestConf)).GetValueOrDefault<bool>();
        }

        public bool GetReadMoveRequestConf()
        {
            var task = GetReadMoveRequestConfAsync();
            task.Wait();
            return task.Result;
        }


        /// <summary>
        /// Gets the value of ReadMoveRequestConf -> trigger for RCS OS to read the MoveRequestConf data.
        /// </summary>
        public async Task<bool> GetMoveRequestReadAsync()
        {
            return (await ReadVariable(Declarations.NodePaths.MoveRequestRead)).GetValueOrDefault<bool>();
        }

        public bool GetMoveRequestRead()
        {
            var task = GetMoveRequestReadAsync();
            task.Wait();
            return task.Result;
        }
    }
}