using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMApp.ViewModels
{
    public class ViewModelLocator
    {
        private static SignInViewModel _signInViewModel = null;
        private static UserViewModel _userViewModel = null;
        private static HomeViewModel _homeViewModel = null;
        public static SignInViewModel SignInViewModel
        {
            get
            {
                if (_signInViewModel == null)
                {
                    _signInViewModel = new SignInViewModel();
                }
                return _signInViewModel;
            }
        }
        public static UserViewModel UserViewModel
        {
            get
            {
                if (_userViewModel == null)
                {
                    _userViewModel = new UserViewModel();
                }
                return _userViewModel;
            }
        }
        public static HomeViewModel HomeViewModel
        {
            get
            {
                if (_homeViewModel == null)
                {
                    _homeViewModel = new HomeViewModel();
                }
                return _homeViewModel;
            }
        }
    }
}
