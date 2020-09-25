using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Common.Exceptions
{
    public class UnknownTotesException : SmHttpControllerException
    {
        [JsonProperty]
        public List<string> Totes { get; set; }
        public override RcsErrorCode ErrorCode { get; } = RcsErrorCode.TOTE_NOT_FOUND;
        public override int HttpStatusCode { get; } = StatusCodes.Status404NotFound;

        public override string ToString()
        {
            return $"Unknown totes {string.Join(", ", Totes)}";
        }
    }
}