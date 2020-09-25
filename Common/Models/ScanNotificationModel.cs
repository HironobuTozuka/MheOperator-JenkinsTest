using Common.Models.Plc;
using Common.Models.Tote;

namespace Common.Models
{
    public class ScanNotificationModel
    {
        public string LocationId { get; set; }
        public ToteRotation ToteRotation { get; set; }
        public RequestToteType ToteType { get; set; }
        public string ToteBarcode { get; set; }
        
        public bool IsWrongScan()
        {
            return Barcode.IsWrongBarcode(ToteBarcode);
        }

        public override string ToString()
        {
            return $"{nameof(LocationId)}: {LocationId}, {nameof(ToteRotation)}: {ToteRotation}, {nameof(ToteType)}: {ToteType}, {nameof(ToteBarcode)}: {ToteBarcode}";
        }
    }
}
