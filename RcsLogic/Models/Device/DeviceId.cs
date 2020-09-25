namespace RcsLogic.Models.Device
{
    public class DeviceId
    {
        public DeviceId(string id)
        {
            this.id = id;
        }

        public string id { get; }

        public override string ToString()
        {
            return $"{id}";
        }

        protected bool Equals(DeviceId other)
        {
            return id == other.id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DeviceId) obj);
        }

        public override int GetHashCode()
        {
            return (id != null ? id.GetHashCode() : 0);
        }
    }
}