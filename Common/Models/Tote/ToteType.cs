using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Models.Tote
{
    public class ToteType
    {
        [Key] public int id { get; set; }
        [MaxLength(255)] public TotePartitioning totePartitioning { get; private set; }
        [MaxLength(255)] public ToteHeight toteHeight { get; private set; }


        public int GetToteHeightValue()
        {
            return (int) toteHeight;
        }

        public string name
        {
            get { return this.ToString(); }
        }

        protected ToteType()
        {
        }

        public ToteType(ToteHeight toteHeight, TotePartitioning totePartitioning)
        {
            this.totePartitioning = totePartitioning;
            this.toteHeight = toteHeight;
        }

        public string ToString() => toteHeight.ToString() + ", " + totePartitioning.ToString();

        private sealed class IdTotePartitioningToteHeightEqualityComparer : IEqualityComparer<ToteType>
        {
            public bool Equals(ToteType x, ToteType y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.id == y.id && x.totePartitioning == y.totePartitioning && x.toteHeight == y.toteHeight;
            }

            public int GetHashCode(ToteType obj)
            {
                return HashCode.Combine(obj.id, (int) obj.totePartitioning, (int) obj.toteHeight);
            }
        }

        public static IEqualityComparer<ToteType> IdTotePartitioningToteHeightComparer { get; } = new IdTotePartitioningToteHeightEqualityComparer();
    }
}