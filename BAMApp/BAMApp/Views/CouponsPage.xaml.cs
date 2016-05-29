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
        public CouponsPage()
        {
            InitializeComponent();

            List<string> couponList = new List<string>();
            couponList.Add("Forever 21 - 15% off all Jewellery");
            couponList.Add("Down East Basics - BOGO free sale items");
            couponList.Add("American eagle - 20% off shoes");

            listView.ItemsSource = couponList;
        }
        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushModalAsync(new NavigationPage(new RedeemCouponPage()));
        }
    }
}
