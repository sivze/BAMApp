using BAMApp.Models;
using BAMApp.ViewModels;
using System;

using Xamarin.Forms;

namespace BAMApp.Views
{
    public partial class HomePage : MasterDetailPage
    {
        public HomePage()
        {
            InitializeComponent();

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
                Navigation.PushAsync((Page)Activator.CreateInstance(item.TargetType));
                
                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
