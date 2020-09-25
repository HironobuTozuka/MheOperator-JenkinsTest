namespace Common.Models.Tote
{
    public class ToteData
    {
        public string toteId { get; set; }
        public double maxAcc { get; set; }
        public double weight { get; set; }

        public string ToString() => $"{{ ToteId = {this.toteId}; MaxAcc = {this.maxAcc.ToString()}; weight = {this.weight.ToString()}; }}";
    }
}
