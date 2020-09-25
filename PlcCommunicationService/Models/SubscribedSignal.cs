namespace PlcCommunicationService.Models
{
    public class SubscribedSignal
    {
        public MonitoredItemDefinition MonitoredItem { get; set; }

        public bool Value
        {
            get
            {
                return _value != null && (bool)_value;
            }
            set
            {
                if (_value.Equals(value))
                {
                    ValueChanged = false;
                }
                else
                {
                    ValueChanged = true;
                    _value = value;
                }
            }
        }

        private bool? _value;
        public bool ValueChanged { get; private set; }
    }
}