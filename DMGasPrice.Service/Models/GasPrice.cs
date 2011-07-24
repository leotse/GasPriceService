using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DMGasPrice.Service.Models
{
    public class GasPrice
    {
        // gas price related fields
        public string CityName { get; set; }
        public double Price { get; set; }
        public double PriceChange { get; set; }

        // meta data
        [ScriptIgnore]
        public int Key { get; set; }
        [ScriptIgnore]
        public DateTime For { get; set; }

        // calculated fields
        public string Message
        {
            get
            {
                return "For " + For.ToString("D");
            }
        }
    }
}