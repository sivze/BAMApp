using BAMApp.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMApp.Interfaces
{
    public interface IAzureService
    {
        MobileServiceClient MobileService {get;}
        Task Initialize();
        
        //Generic
        Task<T> GetById<T>(string id);
        Task Add<T>(T entity);
        Task Update<T>(T entity);
        Task Delete<T>(T entity);
        //Task<ObservableCollection<T>> GetAll<T>();
        //Task<SocialLoginResult> GetUserData();
        //Task Pull<T>();
        //Task Sync();

        //Specific
        Task<BAMAppUser> GetUserByEmail(string email);
        Task<Store> GetStoreByStoreName(string storeName);
        Task<Survey> GetSurveyByStoreId(string storeId);
        Task<SurveyResponse> GetSurveyResponseBySurveyId(string surveyId);
        Task<ObservableCollection<SurveyResponse>> GetSurveyResponsesByUserId(string userId);

        void ReportError(Exception ex);
    }
}
