using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMApp.Models
{
    public class Coupon
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "couponName")]
        public string CouponName { get; set; }

        [JsonProperty(PropertyName = "couponImage")]
        public string CouponImage { get; set; }

        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "couponTerms")]
        public string CouponTerms { get; set; }

        [JsonProperty(PropertyName = "barCodeImage")]
        public string BarCodeImage { get; set; }
    }
}
