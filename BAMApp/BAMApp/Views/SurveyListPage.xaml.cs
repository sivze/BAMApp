﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BAMApp.Views
{
    public partial class SurveyListPage : ContentPage
    {
        public SurveyListPage()
        {
            InitializeComponent();

        }

        void OnTakeSurveyClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new NavigationPage(new SurveyPage()));
        }
    }
}
