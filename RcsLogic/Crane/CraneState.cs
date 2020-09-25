using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RcsLogic.Crane;
using RcsLogic.Models.Device;

namespace RcsLogic
{
    public class CraneState
    {
        private readonly ILogger<CraneState> _logger;

        private readonly Dictionary<Shelf, ShelfState> _shelveStates = new Dictionary<Shelf, ShelfState>();
        private readonly CraneType _craneType;
        private readonly DeviceId _deviceId;
        private int _idleCounter = 0;
        private const int _idleThreshold = 20;

        public CraneState(CraneType craneType, DeviceId deviceId, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CraneState>();
            _deviceId = deviceId;
            _craneType = craneType;
        }

        public void SetShelfReady(Shelf shelf)
        {
            SetShelfState(shelf, ShelfState.Ready);
        }

        public void SetShelfBusy(Shelf shelf)
        {
            SetShelfState(shelf, ShelfState.Busy);
            _idleCounter = 0;
        }

        private void SetShelfState(Shelf shelfIndex, ShelfState shelfState)
        {
            _logger.LogInformation("Device: {0}, Setting shelf state: {1} for shelf: {2}", _deviceId, shelfState,
                shelfIndex);
            _shelveStates[shelfIndex] = shelfState;
        }

        public bool IsBusy()
        {
            var busyShelves = _shelveStates
                .Where(it => it.Value.Equals(ShelfState.Busy))
                .ToList();

            busyShelves.ForEach(kv =>
                _logger.LogInformation("Device: {0}, Shelf {1} is {2}", _deviceId, kv.Key, kv.Value));

            return busyShelves.Count > 0;
        }

        public bool IsTransferTimedOut()
        {
            if (_shelveStates.Any(state => state.Value == ShelfState.Busy))
            {
                _idleCounter += 1;
            }
            return _idleCounter >= _idleThreshold;
        }

        public enum ShelfState
        {
            Busy,
            Ready
        }

        public class Shelf : IEquatable<Shelf>
        {
            public static readonly Shelf First = new Shelf("First");
            public static readonly Shelf Second = new Shelf("Second");

            private readonly string _name;

            private Shelf(string name)
            {
                _name = name;
            }

            public bool Equals(Shelf other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return _name == other._name;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Shelf) obj);
            }

            public override int GetHashCode()
            {
                return (_name != null ? _name.GetHashCode() : 0);
            }

            public static bool operator ==(Shelf left, Shelf right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(Shelf left, Shelf right)
            {
                return !Equals(left, right);
            }

            public override string ToString()
            {
                return $"{nameof(_name)}: {_name}";
            }
        }

        public int GetNumberOfOperatingShelves()
        {
            return (int) _craneType;
        }
    }
}