using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models.Tote;
using PlcCommunicationService.SystemPlc;
using Workstation.ServiceModel.Ua;

namespace PlcCommunicationService.Models
{
    public class ScanNotification : Structure
    {
        public string LocationId { get; set; }
        public byte ToteRotation { get; set; }
        public string ToteType { get; set; }
        public string ToteBarcode { get; set; }
       

        public override void Encode(IEncoder encoder)
        {
            encoder.WriteString("LocationId", this.LocationId);
            encoder.WriteByte("ToteRotation", this.ToteRotation);
            encoder.WriteString("ToteType", this.ToteType);
            encoder.WriteString("ToteBarcode", this.ToteBarcode);

        }

        public override void Decode(IDecoder decoder)
        {
            this.LocationId = decoder.ReadString("LocationId");
            this.ToteRotation = decoder.ReadByte("ToteRotation");
            this.ToteType = decoder.ReadString("ToteType");
            this.ToteBarcode = decoder.ReadString("ToteBarcode");

        }

        public ScanNotificationModel ToScanNotificationModel()
        {
            return new ScanNotificationModel()
            {
                LocationId = LocationId,
                ToteBarcode = ToteBarcode,
                ToteRotation = (ToteRotation)ToteRotation,
                ToteType = ToteTypeName.GetToteType(ToteType)
            };
        }

        public override string ToString()
        {
            return $"{nameof(LocationId)}: {LocationId}, {nameof(ToteRotation)}: {ToteRotation}, {nameof(ToteType)}: {ToteType}, {nameof(ToteBarcode)}: {ToteBarcode}";
        }
    }
}
