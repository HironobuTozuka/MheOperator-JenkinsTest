using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Common.Exceptions
{
    public class TotesNotReadyException : SmHttpControllerException
    {
        [JsonProperty]
        public List<string> Totes { get; set; }
        public override int HttpStatusCode { get; } = StatusCodes.Status400BadRequest;
        public override RcsErrorCode ErrorCode { get; } = RcsErrorCode.TOTE_NOT_READY;

        public override string ToString()
        {
            return $"Not ready totes {string.Join(", ", Totes)}";
        }
    }
}