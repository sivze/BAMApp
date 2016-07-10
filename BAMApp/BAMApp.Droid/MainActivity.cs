using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics.Drawables;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using BAMApp.Helpers;
using BAMApp.Views;
using Android.Content;
using Xamarin.Facebook;
using Java.Security;
using Xamarin.Facebook.AppEvents;
using Xamarin.Forms.Platform.Android;

namespace BAMApp.Droid
{
    [Activity(Label = "BAM", Icon = "@drawable/icon", 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity
    {
        public static ICallbackManager CallbackManager = CallbackManagerFactory.Create();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);

            //Facebook init
            FacebookSdk.SdkInitialize(this.ApplicationContext);

            CurrentPlatform.Init();

            LoadApplication(new App());

            this.ActionBar.SetIcon(Android.Resource.Color.Transparent);

            //Messages
            MessagingCenter.Subscribe<HomePage>(
                this,
                Constants.GPSEnableMessage,
                GPSEnableMessage);
        }

        private void GPSEnableMessage(HomePage sender)
        {
            var intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
            StartActivity(intent);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            CallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}

