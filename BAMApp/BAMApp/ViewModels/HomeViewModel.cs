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
        private string image;
        private bool isSurveyVisible = false;

        private GooglePlaceItem gItem;
        private HomePage homePage;

        private ICommand takeSurveyCommand;
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
        public ICommand TakeSurveyCommand
        {
            get
            {
                if (takeSurveyCommand == null)
                {
                    takeSurveyCommand = new BaseCommand(TakeSurvey);
                }

                return takeSurveyCommand;
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
            await Task.Delay(1000); //to load after home screen is launched

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
            gItem = await ServiceLocator.GooglePlacesService.GetPlacesAsync("40.759471,-111.875634");

            if (gItem != null)
            {
                IsSurveyVisible = true;
                Location = "You are at" +
                    Environment.NewLine +
                    gItem.name +
                    Environment.NewLine +

                    gItem.vicinity;

                if (gItem.photos != null && gItem.photos.Count > 0)
                    this.Image = string.Format(Helpers.Constants.GOOGLE_PLACES_IMAGE_BASE_URL, gItem.photos[0].photo_reference);
            }
            else
            {
                Location = "No stores nearby offer discounts "+coordinates;
                IsSurveyVisible = false;
            }

            IsBusy = false;
        }

        public async void TakeSurvey(object obj)
        {
            await homePage.Navigation.PushAsync(new SurveyPage(gItem));
        }
        public async void Logout(object obj)
        {
            bool isLoggedOut = await ServiceLocator.AuthenticationService.LogoutAsync(
                ServiceLocator.AzureService.MobileService);

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
