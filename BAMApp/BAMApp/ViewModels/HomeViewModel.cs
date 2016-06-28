using BAMApp.Interfaces;
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
    public class HomeViewModel : BaseViewModel
    {
        #region Fields
        private HomePage homePage;

        private ICommand logoutCommand;
        #endregion

        #region Properties
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
