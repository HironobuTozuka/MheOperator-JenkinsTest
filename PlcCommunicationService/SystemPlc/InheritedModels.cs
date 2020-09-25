using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workstation.ServiceModel.Ua;

namespace PlcCommunicationService.SystemPlc
{
    
    public class InheritedModels
    {
        public static Type[] types = { typeof(MoveRequest), typeof(MoveRequestConf) };

        [BinaryEncodingId(Declarations.BinaryEncodings.MoveRequest)]
        public class MoveRequest : PlcCommunicationService.Models.MoveRequest
        {

        }

        [BinaryEncodingId(Declarations.BinaryEncodings.MoveRequestConf)]
        public class MoveRequestConf : PlcCommunicationService.Models.MoveRequestConf
        {

        }

        [BinaryEncodingId(Declarations.BinaryEncodings.ScanNotification)]
        public class ScanNotification : PlcCommunicationService.Models.ScanNotification
        {

        }


    }
}
