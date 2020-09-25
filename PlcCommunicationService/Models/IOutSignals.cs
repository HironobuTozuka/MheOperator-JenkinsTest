using System.Threading.Tasks;

namespace PlcCommunicationService.Models
{
    public interface IOutSignals
    {
        public Task SetReadMoveRequest(bool value);
        public bool GetReadMoveRequest();
        public Task SetMoveRequestConfRead(bool value);
        public Task SetHeartBeat(byte value);
        public Task SetMoveRequest(IMoveRequest value);
    }
}