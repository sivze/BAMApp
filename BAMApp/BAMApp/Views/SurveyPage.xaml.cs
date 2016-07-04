using BAMApp.Models;
using BAMApp.ViewModels;
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
        public SurveyPage(GooglePlaceItem gItem)
        {
            InitializeComponent();
            lblRating.Text = sliderRating.Value.ToString();

            SurveyViewModel vm = new SurveyViewModel(gItem);
            vm.Initialize(this);
            BindingContext = vm;
        }
        void OnValueChanged(object sender, EventArgs e)
        {
            var newStep = Math.Round(sliderRating.Value / stepValue);
            sliderRating.Value = (int) (newStep * stepValue);

            lblRating.Text = sliderRating.Value.ToString();
        }
      
    }
}
