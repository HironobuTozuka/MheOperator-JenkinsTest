using System.Threading.Tasks;
using Workstation.ServiceModel.Ua;
using Workstation.ServiceModel.Ua.Channels;
using Microsoft.Extensions.Logging;

namespace PlcCommunicationService.Robot
{
    public class InSignals : SignalReader, Models.IInSignals
    {
        public string CurrentPartName { get; set; }
        public bool OrderCycleRunning { get; set; }
        public ushort PartsPicked { get; set; }

        public InSignals(UaTcpSessionChannel channel, ILoggerFactory loggerFactory) : base(channel,
            loggerFactory.CreateLogger<SignalReader>())
        {
        }

        /// <summary>
        /// Gets the value of MoveRequestConf -> information about success or failure of the robot picking process.
        /// </summary>
        public async Task<Models.IMoveRequestConf> GetMoveRequestConfAsync()
        {
            return (await ReadVariable(Declarations.NodePaths.MoveRequestConf))
                .GetValueOrDefault<InheritedModels.MoveRequestConf>();
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