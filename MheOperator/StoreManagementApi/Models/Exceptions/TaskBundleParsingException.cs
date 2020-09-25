using System;
using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MheOperator.StoreManagementApi.Models.Exceptions
{
    public class TaskBundleParsingException : SmHttpControllerException
    {
        [JsonProperty]
        public override string Message { get; }

        public TaskBundleParsingException(string message)
        {
            this.Message = message;
        }

        public override string ToString()
        {
            return $"Error was thrown: {Message}";
        }

        public override int HttpStatusCode { get; } = StatusCodes.Status400BadRequest;
        public override RcsErrorCode ErrorCode { get; } = RcsErrorCode.PASING_ERROR;
    }
}