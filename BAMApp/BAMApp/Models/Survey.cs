using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMApp.Models
{
    public class Survey
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "couponId")]
        public string CouponId { get; set; }

        [JsonProperty(PropertyName = "storeId")]
        public string StoreId { get; set; }

        [JsonProperty(PropertyName = "adminId")]
        public string AdminId { get; set; }

        [JsonProperty(PropertyName = "question1")]
        public string Question1 { get; set; }

        [JsonProperty(PropertyName = "question2")]
        public string Question2 { get; set; }

        [JsonProperty(PropertyName = "question3")]
        public string Question3 { get; set; }

        [JsonProperty(PropertyName = "question4")]
        public string Question4 { get; set; }

        [JsonProperty(PropertyName = "question5")]
        public string Question5 { get; set; }
    }
}
