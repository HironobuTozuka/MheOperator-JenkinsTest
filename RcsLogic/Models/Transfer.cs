using System;
using System.Collections.Generic;
using Common.Models;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;

namespace RcsLogic.Models
{
    public class Transfer
    {
        public TaskBase task { get; set; }
        public Tote tote { get; set; }
        public Location sourceLocation { get; set; }
        public Location destLocation { get; set; }
        public RequestStatus status { get; set; }

        public Transfer()
        {
            status = RequestStatus.Idle;
        }

        public override string ToString()
        {
            return
                $"{nameof(task)}: {task}, {nameof(tote)}: {tote}, {nameof(sourceLocation)}: {sourceLocation}, {nameof(destLocation)}: {destLocation}, {nameof(status)}: {status}";
        }

        private bool Equals(Transfer other)
        {
            return Equals(task, other.task) && Equals(tote, other.tote) && Equals(sourceLocation, other.sourceLocation) && Equals(destLocation, other.destLocation);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Transfer) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(task, tote, sourceLocation, destLocation);
        }

        public enum RequestStatus
        {
            Idle,
            Execute
        };
    }
}