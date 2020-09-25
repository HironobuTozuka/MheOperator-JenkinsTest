using System;
using Common.Models.Location;

namespace Common.Exceptions
{
    public class MoreThanOneDownstreamLocation : Exception
    {
        public Location Location { get; set; }
        
        public override string ToString()
        {
            return $"Found more than one downstream location for location: {Location}";
        }
    }
}