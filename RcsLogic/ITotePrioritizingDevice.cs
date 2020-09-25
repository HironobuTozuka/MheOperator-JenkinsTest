namespace RcsLogic
{
    public interface ITotePrioritizingDevice
    {
        public void AddPriorityTote(string barcode);
        public void RemovePriorityTote(string barcode);
    }
}