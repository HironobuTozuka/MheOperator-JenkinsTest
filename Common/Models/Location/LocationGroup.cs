using System.ComponentModel.DataAnnotations;

namespace Common.Models.Location
{
    public class LocationGroup
    {
        [Key] public int id { get; set; }
        [Required] [MaxLength(50)] public string name { get; set; }
    }
}