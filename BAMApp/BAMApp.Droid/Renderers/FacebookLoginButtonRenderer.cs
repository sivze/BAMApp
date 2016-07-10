using System;
using Android.App;
using Android.Content;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Object = Java.Lang.Object;
using View = Android.Views.View;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Facebook.Share;
using BAMApp.Droid.Callbacks;
using BAMApp.Views;
using BAMApp.Droid.Renderers;
using BAMApp.ViewModels;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRenderer))]
namespace BAMApp.Droid.Renderers
{

	public class FacebookLoginButtonRenderer : ViewRenderer<Button, LoginButton>
    {
        private static Activity _activity;

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            _activity = this.Context as MainActivity;

			var loginButton = new LoginButton (this.Context);

				var facebookCallback = new FacebookCallback<LoginResult>
                {

				HandleSuccess = shareResult => 
                {
                    Action<string> local = SignInViewModel.PostSuccessFacebookAction;
                    if (local != null)
                    {
                        local(shareResult.AccessToken.Token);
                    }
                }
			,
			HandleCancel = () => {
				Console.WriteLine ("HelloFacebook: Canceled");
			},
			HandleError = shareError => {
				Console.WriteLine ("HelloFacebook: Error: {0}", shareError);
				}
			};

			loginButton.RegisterCallback (MainActivity.CallbackManager, facebookCallback);
			base.SetNativeControl (loginButton);
        }
    }
}