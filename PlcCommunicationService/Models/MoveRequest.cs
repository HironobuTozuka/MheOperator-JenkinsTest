using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workstation.ServiceModel.Ua;
using Workstation.ServiceModel.Ua.Channels;

namespace PlcCommunicationService.Models
{
    public class MoveRequest : Structure, IMoveRequest
    {
        public ToteRequest ToteRequest1 { get; set; }
        public ToteRequest ToteRequest2 { get; set; }

        public override void Encode(IEncoder encoder)
        {
            encoder.WriteEncodable("ToteRequest1", this.ToteRequest1);
            encoder.WriteEncodable("ToteRequest2", this.ToteRequest2);
        }

        public override void Decode(IDecoder decoder)
        {
            this.ToteRequest1 = decoder.ReadEncodable<ToteRequest>("ToteRequest1");
            this.ToteRequest2 = decoder.ReadEncodable<ToteRequest>("ToteRequest2");
        }

        public override string ToString() => $"{{ ToteRequest 1 = {this.ToteRequest1.ToString()}; ToteRequest 2 = {this.ToteRequest2.ToString()}; }}";
    }
}
