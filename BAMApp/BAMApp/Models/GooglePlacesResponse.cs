using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMApp.Models
{
    public class GooglePlacesResponse
    {
        public List<object> html_attributions { get; set; }
        public List<GooglePlaceItem> results { get; set; }
        public string status { get; set; }
    }
}
