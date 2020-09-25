using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Common.Models.Tote
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Tote
    {
        public Tote()
        {
            lastLocationUpdate = DateTime.Now;
        }
        [Key] [Display(Name = "Tote id")] public int id { get; set; }

        [JsonProperty]
        [Required]
        [Display(Name = "Tote barcode")]
        [MaxLength(255)] 
        public string toteBarcode { get; set; }

        [Display(Name = "Current location")] public int? locationId { get; set; }
        [Display(Name = "Current location")] public Location.Location location { get; set; }

        [Display(Name = "Storage location")] public int storageLocationId { get; set; }
        [Display(Name = "Storage location")] public Location.Location storageLocation { get; set; }

        [Display(Name = "Tote type")] public int typeId { get; set; }
        [JsonProperty]
        [Display(Name = "Tote type")] public ToteType type { get; set; }
        [JsonProperty]
        [Display(Name = "Update date")] public DateTime lastLocationUpdate { get; set; }
        [JsonProperty]
        [Display(Name = "Status")] public ToteStatus status { get; set; }

        protected bool Equals(Tote other)
        {
            return id == other.id && toteBarcode == other.toteBarcode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tote) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, toteBarcode);
        }

        public override string ToString()
        {
            return $"{nameof(id)}: {id}, {nameof(toteBarcode)}: {toteBarcode}";
        }
    }
}