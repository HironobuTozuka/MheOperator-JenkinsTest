using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MujinClient.Models
{
    public class SkuProperties
    {
        public string barcode { get; set; }
        public string name { get; set; }
        public float weight { get; set; }
        public float sizeX { get; set; }
        public float sizeY { get; set; }
        public float sizeZ { get; set; }
        public string mujinURI { get; set; }

    }
}
