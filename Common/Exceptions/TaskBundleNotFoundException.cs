using System;
using Common.Models.Task;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Common.Exceptions
{
    public class TaskBundleNotFoundException : SmHttpControllerException
    {
        public override int HttpStatusCode { get; } = StatusCodes.Status404NotFound;
        public override RcsErrorCode ErrorCode { get; } = RcsErrorCode.TASK_BUNDLE_NOT_FOUND;
        [JsonProperty]
        public TaskBundleId Id { get; set; }

        public override string ToString()
        {
            return $"Unknown task bundle: {Id}";
        }
    }
}