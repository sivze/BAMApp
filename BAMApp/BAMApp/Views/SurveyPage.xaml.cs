using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BAMApp.Views
{
    public partial class SurveyPage : ContentPage
    {
        int stepValue = 1;
        public SurveyPage()
        {
            InitializeComponent();
            lblRating.Text = sliderRating.Value.ToString();
        }
        void OnValueChanged(object sender, EventArgs e)
        {
            var newStep = Math.Round(sliderRating.Value / stepValue);
            sliderRating.Value = (int) (newStep * stepValue);

            lblRating.Text = sliderRating.Value.ToString();
        }
        async void OnSubmitClicked(object sender, EventArgs e)
        {
            var accepted = await DisplayAlert("Thank you for taking this survey!",
                                               "You've earned 15% off on all Jewelry",
                                               "Ok", "Cancel");
            if(accepted)
            {
                await Navigation.PushModalAsync(new NavigationPage(new RedeemCouponPage()));
            }
        }
    }
}
