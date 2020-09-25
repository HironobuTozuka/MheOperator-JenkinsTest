using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RcsLogic.Models
{
    public interface IReturnToteHandler
    {
        public void ReturnTote(string toteId);
    }
}