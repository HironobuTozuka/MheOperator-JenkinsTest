using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workstation.ServiceModel.Ua;

namespace PlcCommunicationService.Models
{
    public interface IMoveRequestConf
    {
        public ToteRequestConf ToteRequest1Conf { get; set; }
        public ToteRequestConf ToteRequest2Conf { get; set; }

        public void Encode(IEncoder encoder);
        public void Decode(IDecoder decoder);
        public string ToString();
    }
}
