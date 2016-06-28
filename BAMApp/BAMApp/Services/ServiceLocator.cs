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
    }
}
