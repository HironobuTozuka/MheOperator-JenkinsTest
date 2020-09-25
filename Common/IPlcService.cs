using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using Common.Models.Gate;
using Common.Models.Led;

namespace Common

{
    public interface IPlcService
    {
        
        public void SwitchLed(LedId ledId, LedState ledState);
        public void OpenGate(GateDescription gate);
        public void CloseGate(GateDescription gate);

        public void RequestPick(RobotPickRequestBundleModel requestBundle);
        public void RequestTransfer(TransferRequestModel transferRequest);
        public bool IsConveyorOccupied(string locationId);
        public bool IsPlcInExecute();
    }
}
