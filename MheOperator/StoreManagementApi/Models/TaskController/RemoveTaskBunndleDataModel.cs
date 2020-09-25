namespace MheOperator.StoreManagementApi.Models.TaskController
{
    /// <summary>
    /// Specifies task bundle to remove
    /// </summary>
    public class CancelTaskBundleDataModel
    {
        /// <summary>
        /// Id of task bundle to remove
        /// </summary>
        public string TaskBundleId { get; set; }
    }
}