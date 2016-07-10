using BAMApp.Interfaces;
using BAMApp.Models;
using BAMApp.Services;
using BAMApp.Views;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            try
            {
                var geolocator = CrossGeolocator.Current;

                //Parameters
                // 5000 - The minimum time (in miliseconds) the system will wait until checking if the location changed
                // 100 - The minimum distance (in meters) traveled until you will be notified
                if (!geolocator.IsListening)
                {
                    if (!geolocator.IsGeolocationEnabled)
                    {
                        await homePage.DisplayAlert("Attention", "Location Settings Turned Off", "Turn On");
                        MessagingCenter.Send(homePage, Helpers.Constants.GPSEnableMessage);
                    }
                    else
                    {
                        IsBusy = true;
                        LoadingMessage = "Gettings current location";

                        var position = await CrossGeolocator.Current.GetPositionAsync(5000);
                        await UpdatePosition(position);
                    }
                    await geolocator.StartListeningAsync(5000, 100);
                }

                CrossGeolocator.Current.PositionChanged += async (sender, e) =>
                {
                    IsBusy = true;
                    LoadingMessage = "Gettings current location";

                    await UpdatePosition(e.Position);
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsBusy = false;
            }
        }

        public async Task UpdatePosition(Plugin.Geolocator.Abstractions.Position position)
        {
            try
            {
                LoadingMessage = "Gettings nearest store";
                string coordinates = position.Latitude + "," + position.Longitude;

                //hardcoded cordinates for demo purpose, change it to "coordinates"
                gItem = await ServiceLocator.GooglePlacesService.GetPlacesAsync("40.76875440000001,-111.8907201");

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
                    Location = "No stores nearby offer discounts " + coordinates;
                    IsSurveyVisible = false;
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsBusy = false;
            }
        }

        public async void TakeSurvey(object obj)
        {
            await homePage.Navigation.PushAsync(new SurveyPage(gItem));
        }
        public async void Logout(object obj)
        {
            IsBusy = true;
            LoadingMessage = "Logging out";

            Location = Image = string.Empty;
            IsSurveyVisible = false;

            if(CrossGeolocator.Current.IsListening)
                await CrossGeolocator.Current.StopListeningAsync();

            bool isLoggedOut = await ServiceLocator.AuthenticationService.LogoutAsync(
                ServiceLocator.AzureService.MobileService);

            if (isLoggedOut)
            {
                homePage.Navigation.InsertPageBefore(new SignInPage(), homePage);
                await homePage.Navigation.PopAsync();
            }
            else
                await homePage.DisplayAlert("Error", "Logout failed!", "Try again");

            IsBusy = false;
        }
        #endregion
    }
}
