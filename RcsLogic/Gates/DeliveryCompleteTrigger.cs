using System.Timers;
using Common.Models.Tote;

namespace RcsLogic.Gates
{
    public class DeliveryCompleteTrigger
    {
        private readonly Tote _tote;
        private readonly Timer _timer;
        private readonly IDeliveryCompleteDevice _deliveryCompleteDevice;
        
        public DeliveryCompleteTrigger(Tote tote, float timeInSeconds, IDeliveryCompleteDevice deliveryCompleteDevice)
        {
            _tote = tote;
            _deliveryCompleteDevice = deliveryCompleteDevice;
            _timer = new Timer(timeInSeconds * 1000) {Enabled = true, AutoReset = false, Interval = timeInSeconds * 1000};
            _timer.Elapsed += sendBackTimer_Elapsed;
            _timer.Start();
            
        }

        private void sendBackTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Elapsed -= sendBackTimer_Elapsed;
            _timer.Dispose();
            _deliveryCompleteDevice.CompleteDelivery(_tote);
        }
    }
}