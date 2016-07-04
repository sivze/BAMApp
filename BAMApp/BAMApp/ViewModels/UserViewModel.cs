using BAMApp.Helpers;
using BAMApp.Models;
using BAMApp.Services;
using BAMApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BAMApp.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        #region Fields
        private string name;
        private string email;
        private string password;
        private string confirmPassword;
        private string phoneNumber;
        private string zipCode;
        private int genderIndex = -1;
        private string gender;
        private string avatar;
        private DateTime birthday;

        private SignUpPage signUpPage;
        private UserProfilePage userProfilePage;

        private ICommand signUpCommand;
        private ICommand updateCommand;
        private ICommand deleteCommand;
        private ICommand stopNotificationsCommand;
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
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
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                if (confirmPassword != value)
                {
                    confirmPassword = value;
                    OnPropertyChanged("ConfirmPassword");
                }
            }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                if (phoneNumber != value)
                {
                    phoneNumber = value;
                    OnPropertyChanged("PhoneNumber");
                }
            }
        }
        public string ZipCode
        {
            get { return zipCode; }
            set
            {
                if (zipCode != value)
                {
                    zipCode = value;
                    OnPropertyChanged("ZipCode");
                }
            }
        }

        public int GenderIndex
        {
            get { return genderIndex; }
            set
            {
                if (genderIndex != value)
                {
                    genderIndex = value;

                    if (genderIndex == 0)
                        gender = Constants.MALE;
                    else if (genderIndex == 1)
                        gender = Constants.FEMALE;

                    OnPropertyChanged("GenderIndex");
                }
            }
        }
        public DateTime Birthday
        {
            get { return birthday; }
            set
            {
                if (birthday != value)
                {
                    birthday = value;
                    OnPropertyChanged("Birthday");
                }
            }
        }
        public string Avatar
        {
            get { return avatar; }
            set
            {
                if (avatar != value)
                {
                    avatar = value;
                    OnPropertyChanged("Avatar");
                }
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
        public ICommand UpdateCommand
        {
            get
            {
                if (updateCommand == null)
                {
                    updateCommand = new BaseCommand(Update);
                }

                return updateCommand;
            }
        }
        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new BaseCommand(Delete);
                }

                return deleteCommand;
            }
        }
        public ICommand StopNotificationsCommand
        {
            get
            {
                if (stopNotificationsCommand == null)
                {
                    stopNotificationsCommand = new BaseCommand(StopNotifications);
                }

                return stopNotificationsCommand;
            }
        }
        #endregion

        #region Methods
        public void Initialize(SignUpPage page)
        {
            this.signUpPage = page;

            Avatar = Name = Email = PhoneNumber = ZipCode = Password = ConfirmPassword = string.Empty;
            GenderIndex = -1;
            Birthday = DateTime.Now.AddYears(-18);
        }
        public async void Initialize(UserProfilePage page)
        {
            this.userProfilePage = page;

            //need change here after getting facebook user info
            if (string.IsNullOrEmpty(Settings.AuthToken)) 
            {
                IsBusy = true;
                LoadingMessage = "Loading...";

                BAMAppUser user = await ServiceLocator.AzureService.GetById<BAMAppUser>(Settings.UserId);
                Avatar = user.Avatar;
                Name = user.Name;
                GenderIndex = user.Gender == Constants.MALE ? 0 : 1;
                Email = user.Email;
                Birthday = user.Birthday;
                PhoneNumber = user.PhoneNumber;
                ZipCode = user.ZipCode;
            }
            else
            {
                await page.DisplayAlert("Sorry", "No access for Facebook User", "Go Back");
                await page.Navigation.PopAsync();
            }

            IsBusy = false;
        }
        public BAMAppUser CreateUserInstance()
        {
            return
                new BAMAppUser
                {
                    Name = name,
                    Email = email,
                    Password = password,
                    Avatar = gender.Equals(Constants.MALE) ? Constants.DEFAULT_MALE_AVATAR :
                                                             Constants.DEFAULT_FEMALE_AVATAR,
                    PhoneNumber = phoneNumber,
                    ZipCode = zipCode,
                    Gender = gender,
                    Birthday = birthday,
                    OS = Device.OS.ToString()
                };
        }
        public async void SignUp(object obj)
        {
            IsBusy = true;
            LoadingMessage = "Creating Account...";

            if (
                string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword) ||
                string.IsNullOrEmpty(gender) ||
                birthday == null
              )
            {
                await signUpPage.DisplayAlert("Error", "Fields with * cannot be empty", "Ok");
            }
            else if (!password.Equals(confirmPassword))
            {
                await signUpPage.DisplayAlert("Error", "Passwords doesn't match", "Ok");
            }
            else
            {
                
                //Check if email id already exists
                if (await ServiceLocator.AzureService.GetUserByEmail(email) != null)
                {
                    IsBusy = false;
                    await signUpPage.DisplayAlert("Sorry", "Email id already exists!", "Please sign in or enter different email id");
                    return;
                }

                //Create BAMPppUser Object
                BAMAppUser user = CreateUserInstance();

                await ServiceLocator.AzureService.Add(user);

                //if inserted successfully navigate
                BAMAppUser savedUser = await ServiceLocator.AzureService.GetUserByEmail(user.Email);
                if (savedUser != null)
                {
                    //save userID and user info to local settings
                    Settings.UserId = savedUser.Id;

                    //remove  sign up page from navigation stack
                    signUpPage.Navigation.InsertPageBefore(new HomePage(), signUpPage.Navigation.NavigationStack.First());
                    await signUpPage.Navigation.PopToRootAsync();
                }
            }

            IsBusy = false;
        }

        
        public async void Update(object obj)
        {
            IsBusy = true;
            LoadingMessage = "Updating...";

            BAMAppUser user = CreateUserInstance();
            user.Id = Settings.UserId;

            await ServiceLocator.AzureService.Update(user);
            Initialize(userProfilePage);

            IsBusy = false;
        }
        public async void Delete(object obj)
        {
            bool answer = await userProfilePage.DisplayAlert("Are you sure?", "All coupons will be lost", "ok", "cancel");
            if (answer)
            {
                IsBusy = true;
                LoadingMessage = "Deleting...";

                BAMAppUser user = await ServiceLocator.AzureService.GetById<BAMAppUser>(Settings.UserId);
                await ServiceLocator.AzureService.Delete(user);
                
                await ServiceLocator.AuthenticationService.LogoutAsync(
                ServiceLocator.AzureService.MobileService);

                userProfilePage.Navigation.InsertPageBefore(new SignInPage(), userProfilePage.Navigation.NavigationStack.First());
                await userProfilePage.Navigation.PopToRootAsync();
            }

            IsBusy = false;
        }
        public async void StopNotifications(object obj)
        {
        }
        #endregion
    }
}