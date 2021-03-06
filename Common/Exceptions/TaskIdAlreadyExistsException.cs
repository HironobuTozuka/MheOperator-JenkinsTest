﻿using System;
using System.Collections.Generic;
using Common.Models.Task;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Common.Exceptions
{
    public class TaskIdAlreadyExistsException : SmHttpControllerException
    {
        public override int HttpStatusCode { get; } = StatusCodes.Status409Conflict;
        public override RcsErrorCode ErrorCode { get; } = RcsErrorCode.TASK_ID_ALREADY_EXISTS;
        [JsonProperty]
        public List<TaskId> Tasks { get; set; }

        public override string ToString()
        {
            return $"Tasks already exist: {string.Join(", ",Tasks)}";
        }
    }
}