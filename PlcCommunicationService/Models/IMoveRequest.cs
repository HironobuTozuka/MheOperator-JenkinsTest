using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workstation.ServiceModel.Ua;

namespace PlcCommunicationService.Models
{
    public interface IMoveRequest
    {
        public ToteRequest ToteRequest1 { get; set; }
        public ToteRequest ToteRequest2 { get; set; }

        public void Encode(IEncoder encoder);
        public void Decode(IDecoder decoder);
        public string ToString();
    }
}
