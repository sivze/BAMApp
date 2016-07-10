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
    public class FacebookService : IFacebookService
    {

        HttpClient client;
        public FacebookUser FBUser { get; private set; }
        public FacebookService()
        {
            client = new HttpClient();
        }

        public async Task<FacebookUser> GetUserInfo(string authToken)
        {
            FBUser = null;

            var uri = new Uri(string.Format(Helpers.Constants.FACEBOOK_API_BASE_URL, authToken));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    FBUser = JsonConvert.DeserializeObject<FacebookUser>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"ERROR {0}", ex.Message);
            }

            return FBUser;
        }
    }
}
