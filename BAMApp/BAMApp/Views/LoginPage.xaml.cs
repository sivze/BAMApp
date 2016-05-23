using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BAMApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            imgBg.Source = ImageSource.FromResource("BAMApp.Assets.Images.LoginPageBG.jpg");
        }
    }
}
