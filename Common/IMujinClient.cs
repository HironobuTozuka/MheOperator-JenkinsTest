using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public interface IMujinClient
    {
        public string GetSkuName(string barcode);
        public void CheckSkuBarcodes(List<string> barcodes);
    }
}
