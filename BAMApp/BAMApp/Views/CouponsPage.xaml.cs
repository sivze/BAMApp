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
    public partial class CouponsPage : ContentPage
    {
        CouponsViewModel vm;
        public CouponsPage()
        {
            InitializeComponent();

            vm = new CouponsViewModel();
            vm.Initialize(this);
            BindingContext = vm;
        }
        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            vm.CouponSelected((Coupon)e.SelectedItem);
        }
    }
}
