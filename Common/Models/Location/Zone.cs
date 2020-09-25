using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.Location
{
    public class Zone
    {
        [Key] [MaxLength(255)] public string id { get; set; }
        public ZoneId zoneId => new ZoneId(id);
        [MaxLength(255)] public TemperatureRegime temperatureRegime { get; set; }
        [MaxLength(255)] public LocationFunction function { get; set; }
        [MaxLength(15)] public string plcGateId { get; set; }
    }
}