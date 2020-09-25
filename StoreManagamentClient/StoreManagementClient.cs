using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Common;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StoreManagementClient.Models;

namespace StoreManagementClient
{
    public class StoreManagementClient : IStoreManagementClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StoreManagementClient> _logger;

        public StoreManagementClient(ILoggerFactory loggerFactory, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _logger = loggerFactory.CreateLogger<StoreManagementClient>();
            _logger.LogInformation("Store management client created, address: {0}", _httpClient.BaseAddress.ToString());
        }

        public void ReportTaskState(TaskBase task, int? picked = null, int? failed = null, FailReason? failReason = null, string failDescription = null)
        {
            TaskUpdateModel.DetailsModel details = null;
            if (picked != null && failed != null)
            {
                details = new TaskUpdateModel.DetailsModel()
                {
                    picked = (int) picked,
                    failed = (int) failed,
                    failReason = failReason,
                    failDescription = failDescription
                };
            }

            var taskUpdateModel = new TaskUpdateModel()
            {
                details = details,
                taskId = task.taskId.ToString(),
                taskStatus = TaskStatusForSm(task)
            };

            SendToSm(taskUpdateModel, "/api/internal/task:update");
        }

        public void ToteNotification(Tote tote, Location scanLocation, ToteRotation toteRotation, ToteStatus toteStatus)
        {
            var toteNotificationModel = new ToteNotificationModel()
            {
                location = scanLocation?.zone?.id,
                toteHeight = tote.type.toteHeight,
                toteId = tote.toteBarcode,
                totePartitioning = tote.type.totePartitioning,
                toteRotation = toteRotation,
                toteStatus = toteStatus
            };

            SendToSm(toteNotificationModel, "/api/internal/tote:notification");
        }

        public ToteData GetToteDetails(string toteBarcode)
        {
            var getTask = GetFromSm("tote_id=" + toteBarcode, "/api/internal/tote:technical_details", typeof(ToteDataModel));
            getTask.Wait();
            var toteData = ((ToteDataModel) getTask.Result).ToToteData();
            _logger.LogInformation("Recieved tote data: {0}", toteData.ToString());
            return toteData;
        }

        private async Task<object> GetFromSm(string request, string url, Type responceType)
        {
            var response = await _httpClient.GetAsync(url + "?" + request);
            _logger.LogInformation("Sent tote details request {1}", url + "?" + request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Unable to retrieve from Store Management from: " + url + " status:" +
                                    response.StatusCode);
            var stringResponce = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(stringResponce, responceType, new StringEnumConverter());
        }

        private void SendToSm(Object objectToSend, string url)
        {
            var serializeObject = JsonConvert.SerializeObject(objectToSend, new StringEnumConverter());
            var body = serializeObject;
            _logger.LogInformation("Sent state update {0}", body);
            var content = new StringContent(body);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            Send(url, content, body);
        }

        private async Task<string> Send(string url, StringContent content, string body)
        {
            try
            {
                var response = await _httpClient.PostAsync(url, content);
                var responseContent = response.Content.ReadAsStringAsync();
                await responseContent;
                _logger.LogInformation("State update status code: {0}, response: {1}", response.StatusCode,
                    responseContent.Result);
                return responseContent.Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to send task update to store management {0}", body);
                return null;
            }
        }

        private static TaskUpdateModel.TaskStatus TaskStatusForSm(TaskBase task)
        {
            var taskStatus = task.taskStatus switch
            {
                RcsTaskStatus.Complete => TaskUpdateModel.TaskStatus.COMPLETED,
                RcsTaskStatus.Cancelled => TaskUpdateModel.TaskStatus.CANCELLED,
                _ => TaskUpdateModel.TaskStatus.FAILED
            };

            return taskStatus;
        }
    }
}