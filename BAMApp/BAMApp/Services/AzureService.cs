using BAMApp.Helpers;
using BAMApp.Interfaces;
using BAMApp.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static BAMApp.Views.SignInPage;

namespace BAMApp.Services
{
    public class AzureService : IAzureService
    {
        private MobileServiceClient mobileService;

        public MobileServiceClient MobileService
        {
            get
            {
                return mobileService;
            }
        }

        private IMobileServiceTable<BAMAppUser> bamAppUserTable;

        public AzureService()
        {
            //Todo - why authHandler is used?
            var handler = new AuthHandler();

            mobileService = new MobileServiceClient(Constants.AZURE_SERVICE_URL, handler);

            //Create our client
            handler.Client = mobileService;
            
        }

        public async Task Initialize()
        {
            try
            {
                if (!mobileService.SyncContext.IsInitialized)
                {

                    if (!string.IsNullOrWhiteSpace(Settings.AuthToken) && !string.IsNullOrWhiteSpace(Settings.UserId))
                    {
                        mobileService.CurrentUser = new MobileServiceUser(Settings.UserId);
                        mobileService.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;
                    }

                    //setup our local sqlite store and intialize our table
                    //const string dbPath = "bamAppLocal.db";
                    //var store = new MobileServiceSQLiteStore(dbPath);
                    //store.DefineTable<BAMAppUser>();
                    //await mobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

                    //Get our sync table that will call out to azure
                    //bamAppUserTable = mobileService.GetSyncTable<BAMAppUser>();

                    bamAppUserTable = mobileService.GetTable<BAMAppUser>();
                    //pull the table from Azure
                    //await Pull<BAMAppUser>();
                }
            }
            catch(Exception ex)
            {
                ReportError(ex);
            }

        }

        public async Task Add<T>(T entity)
        {
            try
            {
                //await MobileService.GetTable<TodoItem>().InsertAsync(user1);
                //if ((object)entity is BAMAppUser)
                //    await bamAppUserTable.InsertAsync((BAMAppUser)(object)entity);
                //else if ((object)entity is SurveyResponse)
                //{
                //    //get survey response table and insert
                //}

                var table = MobileService.GetTable<T>();
                await table.InsertAsync(entity);

                //Synchronize user
                //await Sync();

            }
            catch (Exception ex)
            {
                ReportError(ex);
            }
        }
        public async Task Update<T>(T entity)
        {
            try
            {

                var table = MobileService.GetTable<T>();
                await table.UpdateAsync(entity);

                //if ((object)entity is BAMAppUser)
                //    await bamAppUserTable.UpdateAsync((BAMAppUser)(object)entity);
                //else if ((object)entity is SurveyResponse)
                //{
                //    //get survey response table and update
                //}
            }
            catch (Exception ex)
            {
                ReportError(ex);
            }
        }
        public async Task Delete<T>(T entity)
        {
            try
            {
                var table = MobileService.GetTable<T>();
                await table.DeleteAsync(entity);
                //if (T.GetType()  is BAMAppUser)
                //{
                //    BAMAppUser user = await GetById(id, type);

                //    await bamAppUserTable.DeleteAsync(user);
                //}
            }
            catch (Exception ex)
            {
                ReportError(ex);
            }
        }

        public async Task<T> GetById<T>(string id)
        {
            try
            {
                var table = MobileService.GetTable<T>();

                return await table.LookupAsync(id);

                //if ((object)type is BAMAppUser)
                //{
                //    return await bamAppUserTable.LookupAsync(id); ;
                //}
                //return null;
            }
            catch (Exception ex)
            {
                ReportError(ex);
                return default(T);
            }
        }

        public async Task<BAMAppUser> GetUserByEmail(string email)
        {
            try
            {
                List<BAMAppUser> bamAppUser = await bamAppUserTable.Where(
                    user => user.Email == email).ToListAsync();

                if (bamAppUser.Count > 0)
                    return bamAppUser[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                ReportError(ex);
                return null;
            }
        }

        public async Task<Store> GetStoreByStoreName(string storeName)
        {
            try
            {
                var table = MobileService.GetTable<Store>();

                List<Store> stores = await table.Where(
                    store => store.Name == storeName).ToListAsync();

                if (stores.Count > 0)
                    return stores[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                ReportError(ex);
                return null;
            }
        }

        public async Task<Survey> GetSurveyByStoreId(string storeId)
        {
            try
            {
                var table = MobileService.GetTable<Survey>();

                List<Survey> surveys = await table.Where(
                    survey => survey.StoreId == storeId).ToListAsync();

                if (surveys.Count > 0)
                    return surveys[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                ReportError(ex);
                return null;
            }
        }
        public async Task<SurveyResponse> GetSurveyRespnceBySurveyId(string surveyId)
        {
            try
            {
                var table = MobileService.GetTable<SurveyResponse>();

                List<SurveyResponse> surveyResponses = await table.Where(
                    surveyResponse => surveyResponse.SurveyId == surveyId).ToListAsync();

                if (surveyResponses.Count > 0)
                    return surveyResponses[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                ReportError(ex);
                return null;
            }
        }
        //public async Task<ObservableCollection<T>> GetAll<T>()
        //{
        //    ObservableCollection<T> theCollection = new ObservableCollection<T>();

        //    try
        //    {
        //        //var theTable = MobileService.GetTable<T>();
        //        var theTable = MobileService.GetSyncTable<T>();
        //        theCollection = await theTable.ToCollectionAsync<T>();
        //    }
        //    catch (Exception ex)
        //    {
        //        theCollection = null;
        //        ReportError(ex);
        //    }

        //    return theCollection;
        //    return null;
        //}


        //public async Task Pull<T>()
        //{
        //    try
        //    {
        //        await bamAppUserTable.PullAsync("allUsers", bamAppUserTable.CreateQuery());
        //    }
        //    catch (Exception ex)
        //    {
        //        ReportError(ex);
        //    }
        //}


        //public async Task Sync()
        //{
        //    ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

        //    try
        //    {
        //        //pull down all latest changes and then push current coffees up
        //        //await bamAppUserTable.PullAsync("user", bamAppUserTable.CreateQuery());
        //        //await Pull<BAMAppUser>();
        //        //await mobileService.SyncContext.PushAsync();
        //    }
        //    catch (MobileServicePushFailedException exc)
        //    {
        //        if (exc.PushResult != null)
        //        {
        //            syncErrors = exc.PushResult.Errors;
        //        }
        //    }

        //    // Simple error/conflict handling. A real application would handle the various errors like network conditions,
        //    // server conflicts and others via the IMobileServiceSyncHandler.
        //    if (syncErrors != null)
        //    {
        //        foreach (var error in syncErrors)
        //        {
        //            if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
        //            {
        //                //Update failed, reverting to server's copy.
        //                await error.CancelAndUpdateItemAsync(error.Result);
        //            }
        //            else
        //            {
        //                // Discard local change.
        //                await error.CancelAndDiscardItemAsync();
        //            }

        //            Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
        //        }
        //    }
        //}

        //public async Task<SocialLoginResult> GetUserData()
        //{

        //    return await mobileService.InvokeApiAsync<SocialLoginResult>("getextrauserinfo", HttpMethod.Get, null);
        //}

        public void ReportError(Exception ex)
        {
            throw new NotImplementedException();
        }

       
    }

    //for getting facebook user info
    public class SocialLoginResult
    {
        public Message Message { get; set; }
    }

    public class Message
    {
        public string SocialId
        {
            get { return string.IsNullOrEmpty(Sub) ? Id : Sub; }
        }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Sub { get; set; }
        public string Id { get; set; }
    }
}
