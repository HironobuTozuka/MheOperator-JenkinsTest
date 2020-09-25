using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Common.Models.Location
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Location
    {
        [Key] [Display(Name = "Location id")] public int id { get; set; }
        [Display(Name = "Location group")] public LocationGroup locationGroup { get; set; }
        public int? locationGroupId { get; set; }

        [JsonProperty]
        [Required]
        [MaxLength(15)]
        [Display(Name = "Location name")]
        public string plcId { get; set; }

        [Required] public int locationHeight { get; set; }
        public bool isBackLocation { get; set; }
        public int? frontLocationId { get; set; }
        public Location frontLocation { get; set; }
        [MaxLength(255)] public string rack { get; set; }
        [Display(Name = "Location column")] public int? col { get; set; }
        [Display(Name = "Location row")] public int? row { get; set; }
        public Tote.Tote storedTote { get; set; }
        public LocationStatus status { get; set; }
        [JsonProperty]
        [Required] [MaxLength(255)] public string zoneId { get; set; }
        public Zone zone { get; set; }
        
        public bool IsRackingLocation =>
            zone.function == LocationFunction.Staging
            || zone.function == LocationFunction.Storage
            || zone.function == LocationFunction.Technical;

        public bool IsGateLocation =>
            zone?.function == LocationFunction.LoadingGate 
            || zone?.function == LocationFunction.OrderGate;
        protected bool Equals(Location other)
        {
            return plcId.Equals(other.plcId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Location) obj);
        }

        public override int GetHashCode()
        {
            return (plcId != null ? plcId.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return $"{nameof(plcId)}: {plcId}";
        }
    }
}