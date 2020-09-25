using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class RobotPickRequestBundleModel
    {
        public RobotPickRequestModel pickRequest { get; set; }
        public RobotPickRequestModel preparationRequest { get; set; }
    }
    
}
