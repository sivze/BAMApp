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
    public class SignInViewModel : BaseViewModel
    {
        #region Fields
        private string email;
        private string password;
        private INavigation navigation;

        private ICommand signInCommand;
        private ICommand signUpCommand;
        private ICommand signUpFBCommand;
        #endregion

        #region Properties
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
        public ICommand SignUpFBCommand
        {
            get
            {
                if (signUpFBCommand == null)
                {
                    signUpFBCommand = new BaseCommand(SignUpFB);
                }

                return signUpFBCommand;
            }
        }
        #endregion

        #region Constructor
        public SignInViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }
        #endregion

        #region Methods
        public async void SignIn(object obj)
        {
            await navigation.PushModalAsync(new HomePage());
        }
        public async void SignUp(object obj)
        {
            await navigation.PushModalAsync(new SignUpPage());
        }
        public async void SignUpFB(object obj)
        {
            //await navigation.PushModalAsync(new HomePage());
        }
        #endregion
    }
}
