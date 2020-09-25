namespace PlcCommunicationService.Models
{
    public interface IMoveRequestRedStateChangeListener
    {
        public void NotifyListener(bool moveRequestRed);
    }
}