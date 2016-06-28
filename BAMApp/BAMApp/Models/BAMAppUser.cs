using BAMApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BAMApp.Models
{
    public class BAMAppUser
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get; set; }

        [JsonProperty(PropertyName = "phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "zipcode")]
        public string ZipCode { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "birthday")]
        public DateTime Birthday { get; set; }

        [JsonProperty(PropertyName = "os")]
        public string OS { get; set; }

        //public BAMAppUser CreateInstance(
        //    string name,
        //    string email,
        //    string password,
        //    string avatar,
        //    string phoneNumber,
        //    string zipCode,
        //    string gender,
        //    DateTime birthday)
        //{
        //    Name = name;
        //    Email = email;
        //    Password = password;
        //    Avatar = avatar;
        //    PhoneNumber = phoneNumber;
        //    ZipCode = zipCode;
        //    Gender = gender;
        //    Birthday = birthday;
        //    OS = Device.OS.ToString();
        //    return this;
        //}
    }
}
