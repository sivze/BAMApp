﻿using BAMApp.Services;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BAMApp.Views
{
    public partial class SplashScreen : ContentPage
    {
        public SplashScreen()
        {
            InitializeComponent();

            IntializeAzureService();
        }

        private async void IntializeAzureService()
        {
            await ServiceLocator.AzureService.Initialize();

            await Task.Delay(2000);

            await AuthenticateUser();
        }

        private async Task AuthenticateUser()
        {
            if (Helpers.Settings.IsLoggedIn)
                Navigation.InsertPageBefore(new HomePage(), this);
            else
                Navigation.InsertPageBefore(new SignInPage(), this);

            await Navigation.PopAsync();
        }
    }
}
