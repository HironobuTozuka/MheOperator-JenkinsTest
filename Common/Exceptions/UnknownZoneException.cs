using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Common.Exceptions
{
    public class UnknownZoneException : SmHttpControllerException
    {
        [JsonProperty]
        public List<string> Zones { get; set; }
        public override RcsErrorCode ErrorCode { get; } = RcsErrorCode.ZONE_NOT_FOUND;
        public override int HttpStatusCode { get; } = StatusCodes.Status404NotFound;

        public override string ToString()
        {
            return $"Unknown zones: {string.Join(",", Zones)}";
        }
    }
}