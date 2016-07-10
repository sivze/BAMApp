using BAMApp.Helpers;
using BAMApp.Interfaces;
using BAMApp.Models;
using BAMApp.Services;
using BAMApp.Views;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BAMApp.ViewModels
{
    public class SignInViewModel : BaseViewModel
    {
        #region Fields
        private string email;
        private string password;

        private SignInPage signInPage;

        private ICommand signInCommand;
        private ICommand signUpCommand;
        //private ICommand signUpFBCommand;
        #endregion

        #region Properties
        public static Action<string> PostSuccessFacebookAction { get; set; }
        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged("Email");
                }
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged("Password");
                }
            }
        }
        
        public ICommand SignInCommand
        {
            get
            {
                if (signInCommand == null)
                {
                    signInCommand = new BaseCommand(SignIn);
                }

                return signInCommand;
            }
        }
        public ICommand SignUpCommand
        {
            get
            {
                if (signUpCommand == null)
                {
                    signUpCommand = new BaseCommand(SignUp);
                }

                return signUpCommand;
            }
        }
        //public ICommand SignUpFBCommand
        //{
        //    get
        //    {
        //        if (signUpFBCommand == null)
        //        {
        //            signUpFBCommand = new BaseCommand(SignUpFB);
        //        }

        //        return signUpFBCommand;
        //    }
        //}
        #endregion

        #region Constructor
        public SignInViewModel()
        {
            PostSuccessFacebookAction = async token =>
            {
                
                Settings.AuthToken = token;

                LoadingMessage = "Signing In";
                IsBusy = true;

                var fbUser = await ServiceLocator.FacebookService.GetUserInfo(token);

                //the following are stored in settings to retireve when user opens user profile page
                Settings.UserId = fbUser.id;
                Settings.Name = fbUser.name;
                Settings.Avatar = fbUser.picture.data.url;
                Settings.Gender = fbUser.gender;
                Settings.Email = fbUser.email;

                await TakeToHomePage();

                IsBusy = false;
            };
        }
        #endregion

        #region Methods
        public void Initialize(SignInPage signInPage)
        {
            this.signInPage = signInPage;
            Email = Password = string.Empty;

        }

        public async void SignIn(object obj)
        {
            try
            {
                LoadingMessage = "Signing In";
                IsBusy = true;

                //check whether the user exists in Azure table 
                //and get user id and save in local settings for single-sign on
                if (email != null && email.Contains("@"))
                {
                    BAMAppUser user = await ServiceLocator.AzureService.GetUserByEmail(email);
                    if (user != null)
                    {
                        if (user.Password == password)
                        {
                            Settings.UserId = user.Id;
                            //for home page master page labels (name, avatar) the following are stored in settings
                            Settings.Avatar = user.Avatar;
                            Settings.Name = user.Name;
                            await TakeToHomePage();
                        }
                        else
                            await signInPage.DisplayAlert("Sorry", "Invalid password!", "Try again");
                    }
                    else
                        await signInPage.DisplayAlert("Sorry", "Invalid email id!", "Try again");
                }
                else
                    await signInPage.DisplayAlert("Sorry", "Invalid email id!", "Enter correct email id");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO!" + ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async void SignUp(object obj)
        {
            await signInPage.Navigation.PushAsync(new SignUpPage());
        }
        //public async void SignUpFB(object obj)
        //{
        //    if (IsBusy)
        //        return;

        //    //Xamarin Auth
            

        //    //Xamarin Auth

        //    try
        //    {
        //        LoadingMessage = "Signing In";
        //        IsBusy = true;

        //        var user = await ServiceLocator.AuthenticationService.LoginAsync(
        //               ServiceLocator.AzureService.MobileService,
        //               MobileServiceAuthenticationProvider.Facebook);

        //        if (Settings.IsLoggedIn)
        //            await TakeToHomePage();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("OH NO!" + ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        public async Task TakeToHomePage()
        {
            signInPage.Navigation.InsertPageBefore(new HomePage(), signInPage);
            await signInPage.Navigation.PopAsync();
        }

        #endregion
    }

}
