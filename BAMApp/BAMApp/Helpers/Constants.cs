﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMApp.Helpers
{
    public static class Constants
    {
        public static string MALE = "Male";
        public static string FEMALE = "Female";

        public static string DEFAULT_MALE_AVATAR = "http://www.wpclipart.com/signs_symbol/icons_oversized/male_user_icon.png";
        public static string DEFAULT_FEMALE_AVATAR = "http://www.clker.com/cliparts/b/1/f/a/1195445301811339265dagobert83_female_user_icon.svg.med.png";

        public static string AZURE_SERVICE_URL = "https://bamapp.azurewebsites.net";

        public static string GOOGLE_PLACES_API_KEY = "AIzaSyDSMFfbLPNG1RJDSWpfJisy4UrBK-zn0tI";
        public static string GOOGLE_PLACES_BASE_URL =
            "https://maps.googleapis.com/maps/api/place/nearbysearch/json?radius=150&types=grocery_or_supermarket|clothing_store|jewelry_store&key="+GOOGLE_PLACES_API_KEY+"&location={0}";
        public static string GOOGLE_PLACES_IMAGE_BASE_URL =
            "https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&key=" + GOOGLE_PLACES_API_KEY + "&photoreference={0}";
    }
}
