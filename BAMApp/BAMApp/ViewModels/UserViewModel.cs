using BAMApp.Helpers;
using BAMApp.Models;
using BAMApp.Services;
using BAMApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private bool isDisableControls = false;
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
        public bool IsDisableControls
        {
            get { return isDisableControls; }
            set
            {
                if (isDisableControls != value)
                {
                    isDisableControls = value;
                    OnPropertyChanged("IsFacebookUser");
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

            //disable controls if facebook user
            IsDisableControls = !Settings.IsFacebookUser;

            if (Settings.IsFacebookUser)
            {
                Avatar = Settings.Avatar;
                Name = Settings.Name;
                GenderIndex = Settings.Gender.ToLower() == Constants.MALE ? 0 : 1;
                Email = Settings.Email;
            }
            else
            {
                //BAMApp User
                IsBusy = true;
                LoadingMessage = "Loading...";

                BAMAppUser user = await ServiceLocator.AzureService.GetById<BAMAppUser>(Settings.UserId);
                Avatar = user.Avatar;
                Name = user.Name;
                GenderIndex = user.Gender.ToLower() == Constants.MALE ? 0 : 1;
                Email = user.Email;
                Birthday = user.Birthday;
                PhoneNumber = user.PhoneNumber;
                ZipCode = user.ZipCode;
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

            Regex regex = new Regex("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,20}$");

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
            else if (!regex.Match(password).Success)
            {
                await signUpPage.DisplayAlert("Error",
                    "Passwords should be 6 to 20 chars long and should be a combination of numbers, small and capital letters",
                    "Ok");
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
                user.Password = Password;
                await ServiceLocator.AzureService.Add(user);

                //if inserted successfully navigate
                BAMAppUser savedUser = await ServiceLocator.AzureService.GetUserByEmail(user.Email);
                if (savedUser != null)
                {
                    //save userID and user info to local settings
                    Settings.UserId = savedUser.Id;
                    Settings.Avatar = user.Avatar;
                    Settings.Name = user.Name;

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

            var originalUser = await ServiceLocator.AzureService.GetById<BAMAppUser>(Settings.UserId);
            user.Id = originalUser.Id;
            user.Password = originalUser.Password;
            

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