using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics.Drawables;
using Microsoft.WindowsAzure.MobileServices;

namespace BAMApp.Droid
{
    [Activity(Label = "BAM", Icon = "@drawable/icon", 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            //azure init
            CurrentPlatform.Init();

            LoadApplication(new App());

            this.ActionBar.SetIcon(Android.Resource.Color.Transparent);
        }
    }
}

