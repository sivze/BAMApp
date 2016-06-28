﻿using BAMApp.Services;
using BAMApp.ViewModels;
using BAMApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BAMApp
{
    public partial class App : Application
    {
        public App()
        {
            // The root page of your application
            InitializeComponent();

            MainPage = new NavigationPage(new SplashScreen());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

    }
}
