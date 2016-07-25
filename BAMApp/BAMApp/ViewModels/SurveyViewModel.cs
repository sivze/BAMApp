using BAMApp.Models;
using BAMApp.Services;
using BAMApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BAMApp.ViewModels
{
    public class SurveyViewModel : BaseViewModel
    {
        #region Fields
        private GooglePlaceItem gItem;
        private string shoppingPurpose;
        private SurveyPage surveyPage;
        private Store store;
        private Survey survey;

        private string title;
        private string question1;
        private string question2;
        private string question3;
        private string question4;
        private string question5;

        private bool isGreeted;
        private bool isStoreAppearanceOk;
        private int serviceRating = 5;
        private int purposeOfShoppingIndex=-1;
        private bool isSatisfied;
        private bool isSubmittable;

        private ICommand submitSurveyCommand;
        #endregion

        #region Properties
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged("Title");
                }
            }
        }
        public string Question1
        {
            get { return question1; }
            set
            {
                if (question1 != value)
                {
                    question1 = value;
                    OnPropertyChanged("Question1");
                }
            }
        }
        public string Question2
        {
            get { return question2; }
            set
            {
                if (question2 != value)
                {
                    question2 = value;
                    OnPropertyChanged("Question2");
                }
            }
        }
        public string Question3
        {
            get { return question3; }
            set
            {
                if (question3 != value)
                {
                    question3 = value;
                    OnPropertyChanged("Question3");
                }
            }
        }
        public string Question4
        {
            get { return question4; }
            set
            {
                if (question4 != value)
                {
                    question4 = value;
                    OnPropertyChanged("Question4");
                }
            }
        }
        public string Question5
        {
            get { return question5; }
            set
            {
                if (question5 != value)
                {
                    question5 = value;
                    OnPropertyChanged("Question5");
                }
            }
        }

        public bool IsGreeted
        {
            get { return isGreeted; }
            set
            {
                if (isGreeted != value)
                {
                    isGreeted = value;
                    OnPropertyChanged("IsGreeted");
                }
            }
        }
        public bool IsStoreAppearanceOk
        {
            get { return isStoreAppearanceOk; }
            set
            {
                if (isStoreAppearanceOk != value)
                {
                    isStoreAppearanceOk = value;
                    OnPropertyChanged("IsStoreAppearanceOk");
                }
            }
        }
        public int ServiceRating
        {
            get { return serviceRating; }
            set
            {
                if (serviceRating != value)
                {
                    serviceRating = value;
                    OnPropertyChanged("ServiceRating");
                }
            }
        }
        public int PurposeOfShoppingIndex
        {
            get { return purposeOfShoppingIndex; }
            set
            {
                if (purposeOfShoppingIndex != value)
                {
                    purposeOfShoppingIndex = value;
                    if (value == 0)
                        shoppingPurpose = "Grocery";
                    else if (value == 1)
                        shoppingPurpose = "Clothing";
                    else if (value == 2)
                        shoppingPurpose = "Jewellery";
                    else if (value == 2)
                        shoppingPurpose = "Window Shopping";

                    if (value != -1)
                        IsSubmittable = true;

                    OnPropertyChanged("PurposeOfShoppingIndex");
                }
            }
        }
        public bool IsSatisfied
        {
            get { return isSatisfied; }
            set
            {
                if (isSatisfied != value)
                {
                    isSatisfied = value;
                    OnPropertyChanged("IsSatisfied");
                }
            }
        }
        public bool IsSubmittable
        {
            get { return isSubmittable; }
            set
            {
                if (isSubmittable != value)
                {
                    isSubmittable = value;
                    OnPropertyChanged("IsSubmittable");
                }
            }
        }
        public ICommand SubmitSurveyCommand
        {
            get
            {
                if (submitSurveyCommand == null)
                {
                    submitSurveyCommand = new BaseCommand(SubmitSurvey);
                }

                return submitSurveyCommand;
            }
        }
        #endregion

        #region Constructor
        public SurveyViewModel(GooglePlaceItem gItem)
        {
            this.gItem = gItem;
        }
        #endregion

        #region Methods

        public async void Initialize(SurveyPage surveyPage)
        {
            this.surveyPage = surveyPage;
            this.Title = gItem.name + " Survey";
            IsSubmittable = false;
            await PrepareSurvey();
        }

        public async Task PrepareSurvey()
        {
            try
            {
                IsBusy = true;
                LoadingMessage = "Loading...";

                store = await ServiceLocator.AzureService.GetStoreByStoreName(gItem.name);
                survey = await ServiceLocator.AzureService.GetSurveyByStoreId(store.Id);

                Question1 = survey.Question1;
                Question2 = survey.Question2;
                Question3 = survey.Question3;
                Question4 = survey.Question4;
                Question5 = survey.Question5;

                IsBusy = false;
            }
            catch
            {
                IsBusy = false;
            }
        }

        public async void SubmitSurvey(object obj)
        {
            try
            {
                IsBusy = true;
                LoadingMessage = "Submitting...";

                SurveyResponse response = new SurveyResponse
                {
                    SurveyId = survey.Id,
                    CouponId = survey.CouponId,
                    UserId = Helpers.Settings.UserId,
                    IsGreeted = this.IsGreeted,
                    IsStoreAppearanceOk = this.IsStoreAppearanceOk,
                    ServiceRating = this.ServiceRating,
                    PurposeOfShopping = this.shoppingPurpose,
                    IsSatisfied = this.IsSatisfied,
                };

                await ServiceLocator.AzureService.Add(response);

                IsBusy = false;

                //assuming one survey per survey id
                if (await ServiceLocator.AzureService.GetSurveyResponseBySurveyId(response.SurveyId) != null)
                {
                    await surveyPage.DisplayAlert("Thank you for taking this survey!",
                                                   "Show the barcode while checking out",
                                                   "Ok");

                    surveyPage.Navigation.InsertPageBefore(new CouponsPage(), surveyPage);
                    await surveyPage.Navigation.PopAsync();
                }
                else
                    await surveyPage.DisplayAlert("Error", "Survey cannot be submitted", "Try again");
            }
            catch
            {
                IsBusy = false;
            }
        }
        #endregion
    }
}
