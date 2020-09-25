using System;
using Common.Models.Location;

namespace Common.Exceptions
{
    public class NoDownstreamLocations : Exception
    {
        public Location Location { get; set; }
        
        public override string ToString()
        {
            return $"Found no downstream locations for location: {Location}";
        }
    }
}