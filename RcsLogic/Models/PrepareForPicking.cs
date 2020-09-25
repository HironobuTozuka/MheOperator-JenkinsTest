using Common.Models;
using Common.Models.Location;
using Common.Models.Tote;

namespace RcsLogic.Models
{
    public class PrepareForPicking
    {
        public Tote Tote { get; set; }
        public Location Location  { get; set; }
        public ToteRotation ToteRotation { get; set; }
    }
}