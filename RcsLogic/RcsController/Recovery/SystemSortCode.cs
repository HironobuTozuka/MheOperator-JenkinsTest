using System.Linq;

namespace RcsLogic.RcsController.Recovery
{
    public class SystemSortCode
    {
        public ushort Code { get; }
        public string Name { get; }
        public string Description { get; }
        public SystemFailReason? FailReason { get; }

        public SystemSortCode(ushort code, string name, string description, SystemFailReason? failReason)
        {
            Code = code;
            Name = name;
            Description = description;
            FailReason = failReason;
        }

        public override string ToString()
        {
            return $"{nameof(Code)}: {Code}, {nameof(Name)}: {Name}, {nameof(Description)}: {Description}";
        }

        protected bool Equals(SystemSortCode other)
        {
            return Code == other.Code;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SystemSortCode) obj);
        }

        public override int GetHashCode()
        {
            return Code;
        }
       
    }
}