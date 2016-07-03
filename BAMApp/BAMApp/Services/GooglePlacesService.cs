using BAMApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAMApp.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;

namespace BAMApp.Services
{
    public class GooglePlacesService : IGooglePlacesService
    {
        HttpClient client;
        public List<GooglePlaceItem> Places { get; private set; }
        public GooglePlacesService()
        {
            client = new HttpClient();
        }
        public async Task<GooglePlaceItem> GetPlacesAsync(string coordinates)
        {
            Places = new List<GooglePlaceItem>();

            var uri = new Uri(string.Format(Helpers.Constants.GOOGLE_PLACES_BASE_URL, coordinates));
            
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Places = JsonConvert.DeserializeObject<GooglePlacesResponse>(content).results;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }

            if (Places != null && Places.Count > 0)
                return Places[0];
            else
                return null;
        }
    }
}
