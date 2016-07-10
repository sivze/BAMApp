using BAMApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMApp.Services
{
    public class ServiceLocator
    {
        private static IAuthenticationService _authenticationService = null;
        private static IAzureService _azureService = null;
        private static IGooglePlacesService _googlePlacesService = null;
        private static IFacebookService _facebookService = null;
        public static IAuthenticationService AuthenticationService
        {
            get
            {
                if (_authenticationService == null)
                {
                    _authenticationService = new AuthenticationService();
                }
                return _authenticationService;
            }
        }
        public static IAzureService AzureService
        {
            get
            {
                if (_azureService == null)
                {
                    _azureService = new AzureService();
                }
                return _azureService;
            }
        }
        public static IGooglePlacesService GooglePlacesService
        {
            get
            {
                if (_googlePlacesService == null)
                {
                    _googlePlacesService = new GooglePlacesService();
                }
                return _googlePlacesService;
            }
        }
        public static IFacebookService FacebookService
        {
            get
            {
                if (_facebookService == null)
                {
                    _facebookService = new FacebookService();
                }
                return _facebookService;
            }
        }
    }
}
