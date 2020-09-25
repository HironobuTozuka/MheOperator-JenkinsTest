using System;
using Common.Models.Location;

namespace RcsLogic.Services.Exceptions
{
    public class NextRouteLocationIsLocationGroup : Exception
    {
        public NextRouteLocationIsLocationGroup(Location start, Location destination, int nextLocationGroupId)
        {
            Start = start;
            Destination = destination;
            NextLocationGroupId = nextLocationGroupId;
        }

        public Location Start { get; }
        public Location Destination { get; }
        public int NextLocationGroupId { get;  }
        
        public override string ToString()
        {
            return $"Next location on the route goes through location group. Route from: {Start} to {Destination}, next location group id is {NextLocationGroupId}";
        }
    }
}