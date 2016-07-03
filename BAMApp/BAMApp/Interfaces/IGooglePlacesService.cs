using BAMApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMApp.Interfaces
{
    public interface IGooglePlacesService
    {
        Task<GooglePlaceItem> GetPlacesAsync(string cooordinates);
    }
}
