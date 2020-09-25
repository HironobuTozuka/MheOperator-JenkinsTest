using System;
using System.Collections.Generic;
using Common.Models.Task;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Common.Exceptions
{
    public class TaskBundleIdAlreadyExistsException : SmHttpControllerException
    {
        public override int HttpStatusCode { get; } = StatusCodes.Status409Conflict;
        public override RcsErrorCode ErrorCode { get; } = RcsErrorCode.TASK_BUNDLE_ALREADY_EXISTS;
        [JsonProperty]
        public TaskBundleId Id { get; set; }

        public override string ToString()
        {
            return $"Task bundle already exists: {Id}";
        }
    }
}