using System;
using System.Collections.Generic;
using System.Text;
using Common.Models;
using Common.Models.Location;
using Common.Models.Task;
using Newtonsoft.Json;

namespace StoreManagementClient.Models
{
    class TaskUpdateModel
    {
        public enum TaskStatus
        {
            COMPLETED,
            FAILED,
            CANCELLED
        };

        [JsonProperty("details")] public DetailsModel details { get; set; }
        [JsonProperty("task_id")] public string taskId { get; set; }
        [JsonProperty("task_status")] public TaskStatus taskStatus { get; set; }

        public class DetailsModel
        {
            [JsonProperty("failed")] public int failed { get; set; }
            [JsonProperty("picked")] public int picked { get; set; }
            [JsonProperty("fail_reason")] public FailReason? failReason { get; set; }
            [JsonProperty("fail_description")] public string failDescription { get; set; }
        }
    }
}