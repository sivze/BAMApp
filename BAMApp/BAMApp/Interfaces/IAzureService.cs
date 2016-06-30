using BAMApp.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMApp.Services
{
    public interface IAzureService
    {
        MobileServiceClient MobileService {get;}
        Task Initialize();
        //Task<ObservableCollection<T>> GetAll<T>();
        Task<BAMAppUser> GetByEmail(string email);
        Task<BAMAppUser> GetById(string id);
        //Task<SocialLoginResult> GetUserData();
        //Task Pull<T>();
        Task Add(BAMAppUser user);
        Task Update(BAMAppUser user);
        Task Delete(string id);
        //Task Sync();
        Task<bool> Logout();
        void ReportError(Exception ex);
    }
}
