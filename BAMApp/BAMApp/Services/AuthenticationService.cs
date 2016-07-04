using BAMApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace BAMApp.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            var user = await DependencyService.Get<IAuthenticationService>().LoginAsync(
                      client,
                      provider);
            return user;
        }

        public async Task<bool> LogoutAsync(MobileServiceClient client)
        {
            try
            {
                return await DependencyService.Get<IAuthenticationService>().LogoutAsync(client);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void ClearCookies()
        {
            throw new NotImplementedException();
        }
    }
}
