using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workstation.ServiceModel.Ua;

namespace PlcCommunicationService.Robot
{
    
    public class InheritedModels
    {
        public static Type[] types = { typeof(MoveRequest), typeof(MoveRequestConf) };

        [BinaryEncodingId(Declarations.BinaryEncodings.MoveRequest)]
        public class MoveRequest : Models.MoveRequest
        {

        }

        [BinaryEncodingId(Declarations.BinaryEncodings.MoveRequestConf)]
        public class MoveRequestConf : Models.MoveRequestConf
        {

        }

    }
}
