using BAMApp.ViewModels;
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

            UserViewModel vm = ViewModelLocator.UserViewModel;
            vm.Initialize(this);
            BindingContext = vm;
            
        }
    }
}
