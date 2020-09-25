using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Location;
using Common.Models.Tote;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Common.Exceptions
{
    public class TotesLocationDisabledException : SmHttpControllerException
    {
        public override int HttpStatusCode { get; } = StatusCodes.Status400BadRequest;
        public override RcsErrorCode ErrorCode { get; } = RcsErrorCode.TOTE_LOCATION_DISABLED;
        [JsonProperty]
        public List<Tote> Totes { get;  }
        [JsonProperty]
        public List<Location> Locations { get;  }

        public TotesLocationDisabledException(List<Tote> totes)
        {
            Totes = totes;
            Locations = totes.Select(tote => tote.location).ToList();
        }


        public override string ToString()
        {
            return $"Totes: {string.Join(",", Totes)} are on disabled locations: {string.Join(",", Locations)}";
        }
    }
}