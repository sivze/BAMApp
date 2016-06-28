using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BAMApp.Interfaces;
using BAMApp.Helpers;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using BAMApp.Droid.Helpers;

[assembly: Dependency(typeof(Authentication))]
namespace BAMApp.Droid.Helpers
{
    public class Authentication : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            try
            {
                Settings.LoginAttempts++;
                var user = await client.LoginAsync(Forms.Context, provider);
                Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Settings.UserId = user?.UserId ?? string.Empty;
                return user;
            }
            catch (Exception ex)
            {
                
            }

            return null;
        }

        public async Task<bool> LogoutAsync(MobileServiceClient client)
        {
            bool loggedOut = false;

            try
            {
                ClearCookies();

                await client.LogoutAsync();

                if(client.CurrentUser==null)
                    loggedOut = true;

            }
            catch (Exception ex)
            {
                loggedOut = false;
            }

            return loggedOut;
        }

        public void ClearCookies()
        {
            try
            {
                if ((int)global::Android.OS.Build.VERSION.SdkInt >= 21)
                    global::Android.Webkit.CookieManager.Instance.RemoveAllCookies(null);
            }
            catch (Exception ex)
            {
            }
        }
    }
}