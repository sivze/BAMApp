using BAMApp.Interfaces;
using BAMApp.Models;
using BAMApp.Services;
using BAMApp.Views;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BAMApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Fields
        private string location;
        private string loadingMessage;
        private string image;
        private bool isBusy = false;
        private bool isSurveyVisible = false;

        private HomePage homePage;

        private ICommand logoutCommand;
        #endregion

        #region Properties
        public string Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                    OnPropertyChanged("Location");
                }
            }
        }
        public string Image
        {
            get { return image; }
            set
            {
                if (image != value)
                {
                    image = value;
                    OnPropertyChanged("Image");
                }
            }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }
        public bool IsSurveyVisible
        {
            get { return isSurveyVisible; }
            set
            {
                if (isSurveyVisible != value)
                {
                    isSurveyVisible = value;
                    OnPropertyChanged("IsSurveyVisible");
                }
            }
        }
        public string LoadingMessage
        {
            get { return loadingMessage; }
            set
            {
                if (loadingMessage != value)
                {
                    loadingMessage = value;
                    OnPropertyChanged("LoadingMessage");
                }
            }
        }
        public ICommand LogoutCommand
        {
            get
            {
                if (logoutCommand == null)
                {
                    logoutCommand = new BaseCommand(Logout);
                }

                return logoutCommand;
            }
        }
        #endregion 

        #region Mehods
        public void Initialize(HomePage page)
        {
            this.homePage = page;

            FindCurrentLocation();
        }

        public async void FindCurrentLocation()
        {
            await Task.Delay(2000); //to load after home screen is launched

            var geolocator = CrossGeolocator.Current;

            if (!geolocator.IsListening)
                await geolocator.StartListeningAsync(5000, 100);

            CrossGeolocator.Current.PositionChanged += (sender, e) => {

                IsBusy = true;

                LoadingMessage = "Getting Current Location" +
                    Environment.NewLine +
                    "and" +
                    Environment.NewLine +
                    "nearest store...";

                var position = e.Position;

                string coordinates = position.Latitude +","+ position.Longitude;

                UpdatePosition(coordinates);
            };

            if (!geolocator.IsGeolocationEnabled)
            {
                await homePage.DisplayAlert("Attention", "Location Settings Turned Off", "Turn On");
                //navigate to android location settings menu
            }
            
        }

        public async void UpdatePosition(string coordinates)
        {
            GooglePlaceItem item = await ServiceLocator.GooglePlacesService.GetPlacesAsync(coordinates);

            if (item != null)
            {
                IsSurveyVisible = true;
                Location = "You are at" +
                    Environment.NewLine +
                    item.name +
                    Environment.NewLine +

                    item.vicinity;

                if (item.photos != null && item.photos.Count > 0)
                    this.Image = string.Format(Helpers.Constants.GOOGLE_PLACES_IMAGE_BASE_URL, item.photos[0].photo_reference);
            }
            else
            {
                Location = "No stores nearby offer discounts "+coordinates;
                IsSurveyVisible = false;
            }

            IsBusy = false;
        }

        public async void Logout(object obj)
        {
            bool isLoggedOut = await ServiceLocator.AzureService.Logout();

            if (isLoggedOut)
            {
                homePage.Navigation.InsertPageBefore(new SignInPage(), homePage);
                await homePage.Navigation.PopAsync();
            }
            else
                await homePage.DisplayAlert("Error", "Logout failed!", "Try again");
        }
        #endregion
    }
}
