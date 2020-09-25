namespace Common.Models.Tote
{
    public static class Barcode
    {
        public const string NoRead = "NOREAD"; 
        public const string NoTote = "NOTOTE"; 
        public const string Unknown = "UNKNOWN";

        public static bool IsWrongBarcode(string barcode)
        {
            if (barcode == null)
            {
                return true;
            }
            
            return string.IsNullOrWhiteSpace(barcode)
                   || barcode.Contains(NoRead)
                   || barcode.Contains(NoTote)
                   || barcode.Contains(Unknown);
        }
    }
}