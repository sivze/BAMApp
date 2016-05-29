using BAMApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BAMApp.Views
{
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();
            imgBG.Source = ImageSource.FromResource("BAMApp.Assets.Images.SignInPageBG.jpg");

            BindingContext = new SignInViewModel(Navigation);
        }
    }
}
