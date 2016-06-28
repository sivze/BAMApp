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
        private DateTime birthday;

        private SignUpPage signUpPage;

        private ICommand signUpCommand;
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
                    OnPropertyChanged("Zipcode");
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

                    OnPropertyChanged("Gender");
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
        #endregion

        #region Methods
        public void Initialize(SignUpPage page)
        {
            this.signUpPage = page;
        }
        public async void SignUp(object obj)
        {
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
                if (await ServiceLocator.AzureService.GetByEmail(email) != null)
                {
                    await signUpPage.DisplayAlert("Sorry", "Email id already exists!", "Please sign in or enter different email id");
                    return;
                }

                //Create BAMPppUser Object
                BAMAppUser user = new BAMAppUser
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

                await ServiceLocator.AzureService.Save(user);

                //if inserted successfully navigate
                BAMAppUser savedUser = await ServiceLocator.AzureService.GetByEmail(user.Email);
                if (savedUser != null)
                {
                    //save userID to local settings
                    Settings.UserId = savedUser.Id;

                    //remove  sign up page from navigation stack
                    signUpPage.Navigation.InsertPageBefore(new HomePage(), signUpPage.Navigation.NavigationStack.First());
                    await signUpPage.Navigation.PopToRootAsync();
                }
            }
            #endregion
        }
    }
}