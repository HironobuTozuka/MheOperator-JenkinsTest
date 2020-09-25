using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workstation.ServiceModel.Ua;

namespace PlcCommunicationService.Models
{
    public class ToteRequest : Structure
    {
        public string RequestId { get; set; }
        public string SourceLocationId { get; set; }
        public string SourceToteBarcode { get; set; }
        public string SourceToteType { get; set; }
        public string DestLocationId { get; set; }
        public string DestToteBarcode { get; set; }
        public string DestToteType { get; set; }
        public string PartName { get; set; }
        public float Weight { get; set; }
        public float MaxAcc { get; set; }
        public ushort PickCount { get; set; }

        public ToteRequest() { }
        public ToteRequest(Common.Models.RobotPickRequestModel request)
        {
            RequestId = request.id.GetPlcString();
            PickCount = request.pickCount;
            SourceToteBarcode = request.source.barcode;
            SourceLocationId = request.source.locationId;
            SourceToteType = Robot.ToteTypeNameGenerator.Generate(request.source.toteType, request.source.slotId);
            DestToteBarcode = request.dest.barcode;
            DestLocationId = request.dest.locationId;
            DestToteType = Robot.ToteTypeNameGenerator.Generate(request.dest.toteType, request.dest.slotId);
            PartName = request.partName;
        }

        public ToteRequest(Common.Models.ToteTransferRequestModel request)
        {
            RequestId = request.Id.GetPlcString();
            SourceToteBarcode = request.ToteBarcode;
            SourceLocationId = request.SourceLocationId;
            SourceToteType = SystemPlc.ToteTypeName.Generate(request.ToteType);
            DestLocationId = request.DestLocationId;
            Weight = request.Weight;
            MaxAcc = request.MaxAcc;

        }


        public override void Encode(IEncoder encoder)
        {
            encoder.WriteString("RequestId", this.RequestId);
            encoder.WriteString("SourceLocationId", this.SourceLocationId);
            encoder.WriteString("SourceToteBarcode", this.SourceToteBarcode);
            encoder.WriteString("SourceToteType", this.SourceToteType);
            encoder.WriteString("DestLocationId", this.DestLocationId);
            encoder.WriteString("DestToteBarcode", this.DestToteBarcode);
            encoder.WriteString("DestToteType", this.DestToteType);
            encoder.WriteString("PartName", this.PartName);
            encoder.WriteFloat("Weight", this.Weight);
            encoder.WriteFloat("MaxAcc", this.MaxAcc);
            encoder.WriteUInt16("PickCount", this.PickCount);
        }

        public override void Decode(IDecoder decoder)
        {
            this.RequestId = decoder.ReadString("RequestId");
            this.SourceLocationId = decoder.ReadString("SourceLocationId");
            this.SourceToteBarcode = decoder.ReadString("SourceToteBarcode");
            this.SourceToteType = decoder.ReadString("SourceToteType");
            this.DestLocationId = decoder.ReadString("DestLocationId");
            this.DestToteBarcode = decoder.ReadString("DestToteBarcode");
            this.DestToteType = decoder.ReadString("DestToteType");
            this.PartName = decoder.ReadString("PartName");
            this.Weight = decoder.ReadFloat("Weight");
            this.MaxAcc = decoder.ReadFloat("MaxAcc");
            this.PickCount = decoder.ReadUInt16("PickCount");
        }


        public override string ToString() => $"{{ Request Id = {this.RequestId}; Source location Id = {this.SourceLocationId}; Source tote Barcode = {this.SourceToteBarcode};  Source tote Type = {this.SourceToteType}; Dest location Id = {this.DestLocationId}; Dest tote Barcode = {this.DestToteBarcode};  Dest tote Type = {this.DestToteType}; Part name = {this.PartName}; Weight = {this.Weight}; MaxAcc = {this.MaxAcc};  Pick count = {this.PickCount}; }}";
    }
}
