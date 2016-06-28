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
    public partial class HomePage : MasterDetailPage
    {
        public HomePage()
        {
            InitializeComponent();

            //Detail = 
            //    new NavigationPage(new SurveyListPage())
            //{
            //    BarBackgroundColor = (Color)Application.Current.Resources["ThemeColor"],
            //    BarTextColor = (Color)Application.Current.Resources["ThemeTextColor"]
            //};
            masterPage.ListView.ItemSelected += OnItemSelected;

            HomeViewModel vm = ViewModelLocator.HomeViewModel;
            vm.Initialize(this);
            BindingContext = vm;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType))
                {
                    BarBackgroundColor = (Color)Application.Current.Resources["ThemeColor"],
                    BarTextColor = (Color)Application.Current.Resources["ThemeTextColor"],
                };

                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
