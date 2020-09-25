namespace Common.Models.Plc
{
    public class PlcNotification
    {
        public PlcNotificationType NotificationType { get; set; }
        public PlcNotificationParameters Parameters { get; set; }
    }
}