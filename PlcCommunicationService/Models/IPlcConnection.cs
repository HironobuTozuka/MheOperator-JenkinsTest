using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlcCommunicationService.Models
{
    interface IPlcConnection
    {
        //public IInSignals inSignals { get; }
        //public IOutSignals outSignals { get; }

        //public Workstation.ServiceModel.Ua.CommunicationState PlcStatus { get; }
        //public bool AllCommunicationOK { get; }
        public Task SendMoveRequest(Models.MoveRequest moveRequest);
    }
}