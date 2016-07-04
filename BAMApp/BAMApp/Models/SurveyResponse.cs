using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMApp.Models
{
    public class SurveyResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "surveyId")]
        public string SurveyId { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "isGreeted")]
        public bool IsGreeted { get; set; }

        [JsonProperty(PropertyName = "isStoreAppearanceOk")]
        public bool IsStoreAppearanceOk { get; set; }

        [JsonProperty(PropertyName = "serviceRating")]
        public int ServiceRating { get; set; }

        [JsonProperty(PropertyName = "purposeOfShopping")]
        public string PurposeOfShopping { get; set; }

        [JsonProperty(PropertyName = "isSatisfied")]
        public bool IsSatisfied { get; set; }
    }
}
