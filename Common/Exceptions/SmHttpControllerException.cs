using System;
using Newtonsoft.Json;

namespace Common.Exceptions
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class SmHttpControllerException : Exception
    {
        [JsonIgnore]
        public abstract int HttpStatusCode { get; }
        [JsonProperty]
        public abstract RcsErrorCode ErrorCode { get; }
    }
}