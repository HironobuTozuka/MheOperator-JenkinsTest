using System.Linq;
using Microsoft.EntityFrameworkCore;
using Common.Models;
using Common.Models.Location;

namespace RcsLogic.Models
{
    public class ServicedLocation
    {
        public int Id { get; }
        public string PlcId { get; }
        public Location Location { get; }


        public ServicedLocation(Location location)
        {
            Id = location.id;
            Location = location;
            PlcId = location.plcId;
        }
    }
}