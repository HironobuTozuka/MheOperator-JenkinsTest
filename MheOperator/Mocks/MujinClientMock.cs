using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Tests
{
    public class MujinClientMock : Common.IMujinClient
    {

        private ILogger _logger;
        public MujinClientMock(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MujinClientMock>();
            _logger.LogInformation("Created MujinClientMock");
        }
        
        public string GetSkuName(string barcode)
        {
            return "SkuName";
        }
        public void CheckSkuBarcodes(List<string> barcodes)
        {
        }
    }
}
