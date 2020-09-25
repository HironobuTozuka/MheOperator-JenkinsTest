using System.Linq;
using Common.Models.Task;
using Common.Models.Transfer;
using RcsLogic.RcsController.Recovery;

namespace RcsLogic.Robot
{
    public class RobotSortCode : ISortCode
    {
        public ushort Code { get; }
        public string Name { get; }
        public string Description { get; }
        public FailReason? FailReason { get; }
        public bool ShakingCanHelp { get; }

        public RobotSortCode(ushort code, string name, string description, FailReason? failReason, bool shakingCanHelp = false)
        {
            Code = code;
            Name = name;
            Description = description;
            FailReason = failReason;
            ShakingCanHelp = shakingCanHelp;
        }

        public bool IsPlaceToteError()
        {
            return FailReason == Common.Models.Task.FailReason.DestToteError;
        }
        
        public bool IsError()
        {
            return FailReason != null;
        }

        public override string ToString()
        {
            return $"{nameof(Code)}: {Code}, {nameof(Name)}: {Name}, {nameof(Description)}: {Description}";
        }

        protected bool Equals(RobotSortCode other)
        {
            return Code == other.Code;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RobotSortCode) obj);
        }

        public override int GetHashCode()
        {
            return Code;
        }
    }
}