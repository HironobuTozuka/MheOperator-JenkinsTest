using Common.Models;

namespace Common
{
    public interface IPickRequestDoneListener
    {
        public void ProcessPickRequestDone(PickRequestDoneModel pickRequestDone);
    }
}
