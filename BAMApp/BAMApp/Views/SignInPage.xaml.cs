using BAMApp.Models;
using BAMApp.ViewModels;
using Microsoft.WindowsAzure.MobileServices;
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

            SignInViewModel vm = ViewModelLocator.SignInViewModel;
            vm.Initialize(this);
            BindingContext = vm;
        }
    }
}
