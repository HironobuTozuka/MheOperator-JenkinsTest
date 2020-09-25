using Common;
using Common.Models.Location;
using Common.Models.Tote;

namespace RcsLogic.RcsController.ToteCommand
{
    public class NotifySMToteOnLocationCommand : IToteCommand
    {
        private readonly IStoreManagementClient _storeManagementClient;
        private readonly Location _scanLocation;
        private readonly Tote _tote;
        private readonly ToteRotation _toteRotation;

        public NotifySMToteOnLocationCommand(IStoreManagementClient storeManagementClient, Location scanLocation, Tote tote, ToteRotation toteRotation)
        {
            _storeManagementClient = storeManagementClient;
            _scanLocation = scanLocation;
            _tote = tote;
            _toteRotation = toteRotation;
        }

        public void Execute()
        {
            _storeManagementClient.ToteNotification(_tote, _scanLocation, _toteRotation, _tote.status);
        }
    }
}