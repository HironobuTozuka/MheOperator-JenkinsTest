using System;
using Common.Models.Tote;

namespace Common.Models.Plc
{
    public class RequestToteType
    {
        public TotePartitioning TotePartitioning { get; set; }
        public ToteHeight ToteHeight { get; set; }

        public RequestToteType()
        {
        }
        
        public RequestToteType(ToteHeight toteHeight, TotePartitioning totePartitioning)
        {
            TotePartitioning = totePartitioning;
            ToteHeight = toteHeight;
        }

        public RequestToteType(ToteType toteType)
        {
            TotePartitioning = toteType.totePartitioning;
            ToteHeight = toteType.toteHeight;
        }

        public override string ToString() => ToteHeight.ToString() + ", " + TotePartitioning.ToString();

        protected bool Equals(RequestToteType other)
        {
            return TotePartitioning == other.TotePartitioning && ToteHeight == other.ToteHeight;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RequestToteType) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) TotePartitioning, (int) ToteHeight);
        }
    }
}