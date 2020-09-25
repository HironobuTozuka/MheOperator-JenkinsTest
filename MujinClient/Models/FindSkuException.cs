using System;
using System.Collections.Generic;
using System.Text;

namespace MujinClient.Models
{
    class FindSkuException : Exception
    {
        public FindSkuException(string message,bool skuFound):base(message)
        {
            SkuFound = skuFound;
        }
        public bool SkuFound { get; set; }
    }
}
