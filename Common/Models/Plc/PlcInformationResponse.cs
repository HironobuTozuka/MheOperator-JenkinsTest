namespace Common.Models.Plc
{
    public class PlcInformationResponse
    {
        public string Key { get; set; }
        public string ValueType { get; set; }
        public bool BoolValue { get; set; }
        public int IntValue { get; set; }


        public override string ToString()
        {
            return $"{nameof(Key)}: {Key}, {nameof(ValueType)}: {ValueType}, {nameof(BoolValue)}: {BoolValue}, {nameof(IntValue)}: {IntValue}";
        }
    }
}