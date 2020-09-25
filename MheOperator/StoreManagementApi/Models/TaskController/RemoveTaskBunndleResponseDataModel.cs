using System.Collections.Generic;

namespace MheOperator.StoreManagementApi.Models.TaskController
{
    /// <summary>
    /// Provides response for task bundle remove
    /// </summary>
    public class RemoveTaskBundleResponseDataModel
    {
        /// <summary>
        /// List of removed tasks from task bundle
        /// </summary>
        public List<string> CanceledTaskIds { get; set; }
    }
}