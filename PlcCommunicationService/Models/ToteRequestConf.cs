using System;
using Common.Models;
using Common.Models.Pick;
using Common.Models.Transfer;
using Workstation.ServiceModel.Ua;

namespace PlcCommunicationService.Models
{
    public class ToteRequestConf : Structure
    {
        public string RequestId { get; set; }
        public string SourceLocationId { get; set; }
        public string SourceToteBarcode { get; set; }
        public string RequestedDestLocationId { get; set; }
        public string ActualDestLocationId { get; set; }
        public string DestToteBarcode { get; set; }
        public string PartName { get; set; }
        public ushort RequestedPickCount { get; set; }
        public ushort ActualPickCount { get; set; }
        public ushort SortCode { get; set; }


        public override void Encode(IEncoder encoder)
        {
            encoder.WriteString("RequestId", this.RequestId);
            encoder.WriteString("SourceLocationId", this.SourceLocationId);
            encoder.WriteString("SourceToteBarcode", this.SourceToteBarcode);
            encoder.WriteString("RequestedDestLocationId", this.RequestedDestLocationId);
            encoder.WriteString("ActualDestLocationId", this.ActualDestLocationId);
            encoder.WriteString("DestToteBarcode", this.DestToteBarcode);
            encoder.WriteString("PartName", this.PartName);
            encoder.WriteUInt16("RequestedPickCount", this.RequestedPickCount);
            encoder.WriteUInt16("ActualPickCount", this.ActualPickCount);
            encoder.WriteUInt16("SortCode", this.SortCode);
        }

        public override void Decode(IDecoder decoder)
        {
            this.RequestId = decoder.ReadString("RequestId");
            this.SourceLocationId = decoder.ReadString("SourceLocationId");
            this.SourceToteBarcode = decoder.ReadString("SourceToteBarcode");
            this.RequestedDestLocationId = decoder.ReadString("RequestedDestLocationId");
            this.ActualDestLocationId = decoder.ReadString("ActualDestLocationId");
            this.DestToteBarcode = decoder.ReadString("DestToteBarcode");
            this.PartName = decoder.ReadString("PartName");
            this.RequestedPickCount = decoder.ReadUInt16("RequestedPickCount");
            this.ActualPickCount = decoder.ReadUInt16("ActualPickCount");
            this.SortCode = decoder.ReadUInt16("SortCode");
        }

        public override string ToString() =>
            $"{{ Request Id = {this.RequestId}; Source location Id = {this.SourceLocationId}; Source tote Barcode = {this.SourceToteBarcode};  Requested dest location id = {this.RequestedDestLocationId}; Actual dest location Id = {this.ActualDestLocationId}; Dest tote Barcode = {this.DestToteBarcode}; Part name = {this.PartName};  Requested pick count = {this.RequestedPickCount};  Actual pick count = {this.ActualPickCount}; Sort code = {this.SortCode}; }}";

        public ToteTransferRequestDoneModel ToTransferRequestConf()
        {
            if (string.IsNullOrWhiteSpace(SourceToteBarcode) 
                || string.IsNullOrWhiteSpace(SourceLocationId) 
                || string.IsNullOrWhiteSpace(RequestId))
                return null;
            return new ToteTransferRequestDoneModel()
            {
                requestId = new TransferId(RequestId),
                actualDestLocationId = ActualDestLocationId,
                requestedDestLocationId = RequestedDestLocationId,
                sortCode = SortCode,
                sourceLocationId = SourceLocationId,
                sourceToteBarcode = SourceToteBarcode
            };
        }

        public PickRequestDoneModel ToRobotPickRequestDoneModel()
        {
            if (string.IsNullOrEmpty(SourceToteBarcode) || string.IsNullOrEmpty(DestToteBarcode)) return null;
            return new PickRequestDoneModel()
            {
                requestId = new PickId(RequestId),
                actualPickCount = ActualPickCount,
                destLocationId = RequestedDestLocationId,
                destToteBarcode = DestToteBarcode,
                partName = PartName,
                requestedPickCount = RequestedPickCount,
                sortCode = SortCode,
                sourceLocationId = SourceLocationId,
                sourceToteBarcode = SourceToteBarcode
            };
        }
    }
}