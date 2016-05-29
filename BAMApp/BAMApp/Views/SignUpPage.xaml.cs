using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BAMApp.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();

            List<string> genderItems = new List<string>();
            genderItems.Add("Male");
            genderItems.Add("Female");
            
        }
    }
}
