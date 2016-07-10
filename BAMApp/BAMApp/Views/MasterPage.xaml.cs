using BAMApp.Models;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BAMApp.Views
{
    public partial class MasterPage : ContentPage
    {
        public ListView ListView
        {
            get { return listView; }
        }
        
        public MasterPage()
        {
            InitializeComponent();

            lblName.Text = Helpers.Settings.Name;
            imgAvatar.Source = Helpers.Settings.Avatar;

            var masterPageItems = new List<MasterPageItem>();

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Coupons",
                IconSource = "coupon.png",
                TargetType = typeof(CouponsPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Account",
                IconSource = "userIcon.png",
                TargetType = typeof(UserProfilePage)
            });

            listView.ItemsSource = masterPageItems;

        }
    }
}
