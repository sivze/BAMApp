using BAMApp.Helpers;
using BAMApp.Interfaces;
using BAMApp.iOS.Helpers;
using Foundation;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Authentication))]
namespace BAMApp.iOS.Helpers
{
    public class Authentication : IAuthenticationService
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            try
            {
                var window = UIKit.UIApplication.SharedApplication.KeyWindow;
                var root = window.RootViewController;
                if (root != null)
                {
                    var current = root;
                    while (current.PresentedViewController != null)
                    {
                        current = current.PresentedViewController;
                    }


                    Settings.LoginAttempts++;

                    var user = await client.LoginAsync(current, provider);

                    Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                    Settings.UserId = user?.UserId ?? string.Empty;

                    return user;
                }
            }
            catch (Exception e)
            {
                
            }

            return null;
        }

        public void ClearCookies()
        {
            var store = NSHttpCookieStorage.SharedStorage;
            var cookies = store.Cookies;

            foreach (var c in cookies)
            {
                store.DeleteCookie(c);
            }
        }

        public Task<bool> LogoutAsync(MobileServiceClient client)
        {
            throw new NotImplementedException();
        }
    }
}
