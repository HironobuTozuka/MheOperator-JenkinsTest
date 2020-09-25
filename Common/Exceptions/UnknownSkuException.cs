using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Common.Exceptions
{
    public class UnknownSkuException : SmHttpControllerException
    {
        [JsonProperty]
        public List<string> SkuIds { get; set; }
        public override int HttpStatusCode { get; } = StatusCodes.Status404NotFound;
        public override RcsErrorCode ErrorCode { get; } = RcsErrorCode.SKU_NOT_FOUND;

        public override string ToString()
        {
            return $"Unknown sku {string.Join(", ", SkuIds)}";
        }
    }
}