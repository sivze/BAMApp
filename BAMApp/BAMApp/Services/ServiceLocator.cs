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
        private static IAzureService _azureService = null;
        private static IGooglePlacesService _googlePlacesService = null;
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
    }
}
