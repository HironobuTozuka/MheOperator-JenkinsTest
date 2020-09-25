using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workstation.ServiceModel.Ua;

namespace PlcCommunicationService.Models
{
    public class MoveRequestConf : Structure, IMoveRequestConf
    {
        public ToteRequestConf ToteRequest1Conf { get; set; }
        public ToteRequestConf ToteRequest2Conf { get; set; }

        public override void Encode(IEncoder encoder)
        {
            encoder.WriteEncodable("ToteRequest1Conf", this.ToteRequest1Conf);
            encoder.WriteEncodable("ToteRequest2Conf", this.ToteRequest2Conf);

        }

        public override void Decode(IDecoder decoder)
        {
            this.ToteRequest1Conf = decoder.ReadEncodable<ToteRequestConf>("ToteRequest1Conf");
            this.ToteRequest2Conf = decoder.ReadEncodable<ToteRequestConf>("ToteRequest2Conf");

        }

        public override string ToString() => $"{{ Tote Request 1 Conf = {this.ToteRequest1Conf}; Tote Request 2 Conf = {this.ToteRequest2Conf}; }}";
    }
}
