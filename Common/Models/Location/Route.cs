using System.ComponentModel.DataAnnotations;

namespace Common.Models.Location
{
    public class Route
    {
        [Key] public int id { get; set; }

        [Display(Name = "Device id")]
        [MaxLength(50)]
        public string deviceId { get; set; }

        [Display(Name = "Location type id")] public int? locationTypeId { get; set; }
        [Display(Name = "Location type id")] public LocationGroup LocationGroup { get; set; }
        [Display(Name = "Location id")] public int? locationId { get; set; }
        [Display(Name = "Location id")] public Location location { get; set; }
        public int? routedLocationTypeId { get; set; }

        [Display(Name = "Routed Location Type id")]
        public LocationGroup RoutedLocationGroup { get; set; }

        public int? routedLocationId { get; set; }
        [Display(Name = "Routed Location id")] public Location routedLocation { get; set; }
        public bool isDefaultRoute { get; set; }
        public int routeCost { get; set; }
    }
}