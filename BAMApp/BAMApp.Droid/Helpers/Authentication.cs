using System;

using BAMApp.Interfaces;
using BAMApp.Helpers;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using BAMApp.Droid.Helpers;
using Xamarin.Facebook.Login;

[assembly: Dependency(typeof(Authentication))]
namespace BAMApp.Droid.Helpers
{
    public class Authentication : IAuthenticationService
    {
        //public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
        //{
        //    try
        //    {
        //        Settings.LoginAttempts++;

        //        //Native FB login
        //        //this.Con

        //        var user = await client.LoginAsync(Forms.Context, provider);
        //        Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
        //        Settings.UserId = user?.UserId ?? string.Empty;
        //        return user;
        //    }
        //    catch (Exception ex)
        //    {
                
        //    }

        //    return null;
        //}

        public async Task<bool> LogoutAsync(MobileServiceClient client)
        {
            bool loggedOut = false;

            try
            {
                ClearCookies();

                //await client.LogoutAsync();

                //facebook logout
                LoginManager.Instance.LogOut();

                Settings.UserId = string.Empty;
                Settings.AuthToken = string.Empty;
                Settings.Avatar = Constants.DEFAULT_MALE_AVATAR;
                Settings.Name = Constants.UNKNOWN;

                if (client.CurrentUser==null)
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