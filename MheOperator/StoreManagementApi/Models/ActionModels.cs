using System.Collections.Generic;
using Common.Models;
using Common.Models.Location;
using Newtonsoft.Json;

namespace MheOperator.StoreManagementApi.Models
{
    public class LedPostModel
    {
        public List<int> ledIds { get; set; }
    }

    public class GatePostModel
    {
        public ZoneId gateId { get; set; }
    }
}