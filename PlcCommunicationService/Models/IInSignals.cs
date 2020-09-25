using System.Threading.Tasks;


namespace PlcCommunicationService.Models
{
    public interface IInSignals
    {
        public Task<IMoveRequestConf> GetMoveRequestConfAsync();
        public Task<bool> GetReadMoveRequestConfAsync();
        public Task<bool> GetMoveRequestReadAsync();
        public bool GetReadMoveRequestConf();
        public bool GetMoveRequestRead();
    }
}
