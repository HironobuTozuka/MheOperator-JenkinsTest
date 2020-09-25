using System;
using Common.Models.Task;
using JsonSubTypes;
using Newtonsoft.Json;

namespace MheOperator.StoreManagementApi.Models.TaskController
{
    [JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(PickTaskDataModel), TaskType.Pick)]
    [JsonSubtypes.KnownSubType(typeof(MoveTaskDataModel), TaskType.Move)]
    [JsonSubtypes.KnownSubType(typeof(DeliverTaskDataModel), TaskType.Deliver)]
    public abstract class TaskBaseDataModel
    {
        public virtual TaskType type { get; }
        public string taskId { get; set; }

        public abstract TaskBase toTaskBase();
    }
}