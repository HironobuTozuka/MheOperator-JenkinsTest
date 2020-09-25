using Microsoft.AspNetCore.Http;
using Common.Models.Location;
using Common.Models.Tote;
using Newtonsoft.Json;

namespace Common.Exceptions
{
    public class NoLocationForToteInZoneException : SmHttpControllerException
    {
        public override int HttpStatusCode { get; } = StatusCodes.Status404NotFound;
        public override RcsErrorCode ErrorCode { get; } = RcsErrorCode.NO_LOCATION_FOR_TOTE_IN_ZONE;
        [JsonProperty]
        public ZoneId ZoneId { get; }
        [JsonProperty]
        public Tote Tote { get;  }
        
        public NoLocationForToteInZoneException(Tote tote, ZoneId zoneId)
        {
            ZoneId = zoneId;
            Tote = tote;
        }


        public override string ToString()
        {
            return $"No location was found for Tote: {Tote.toteBarcode} in zone: {ZoneId}";
        }

        
    }
}