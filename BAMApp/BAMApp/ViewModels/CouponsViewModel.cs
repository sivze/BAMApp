using BAMApp.Models;
using BAMApp.Services;
using BAMApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BAMApp.ViewModels
{
    public class CouponsViewModel : BaseViewModel
    {
        #region Fields

        private string barcodeImage;
        private string couponName;
        private string couponTerms;
        private bool isBarcodeVisible;
        private Coupon selectedCoupon;

        private CouponsPage couponsPage;
        private ObservableCollection<Coupon> coupons;

        private ICommand closeBarCodeCommand;
        #endregion

        #region Properties
        public ObservableCollection<Coupon> Coupons
        {
            get { return coupons; }
            set
            {
                if (coupons != value)
                {
                    coupons = value;
                    OnPropertyChanged("Coupons");
                }
            }
        }
        public Coupon SelectedCoupon
        {
            get { return selectedCoupon; }
            set
            {
                if (selectedCoupon != value)
                {
                    selectedCoupon = value;
                    OnPropertyChanged("SelectedCoupon");
                }
            }
        }
        public string CouponName
        {
            get { return couponName; }
            set
            {
                if (couponName != value)
                {
                    couponName = value;
                    OnPropertyChanged("CouponName");
                }
            }
        }
        public string CouponTerms
        {
            get { return couponTerms; }
            set
            {
                if (couponTerms != value)
                {
                    couponTerms = value;
                    OnPropertyChanged("CouponTerms");
                }
            }
        }
        public string BarcodeImage
        {
            get { return barcodeImage; }
            set
            {
                if (barcodeImage != value)
                {
                    barcodeImage = value;
                    OnPropertyChanged("BarcodeImage");
                }
            }
        }
        public bool IsBarcodeVisible
        {
            get { return isBarcodeVisible; }
            set
            {
                if (isBarcodeVisible != value)
                {
                    isBarcodeVisible = value;
                    OnPropertyChanged("IsBarcodeVisible");
                }
            }
        }
        public ICommand CloseBarCodeCommand
        {
            get
            {
                if (closeBarCodeCommand == null)
                {
                    closeBarCodeCommand = new BaseCommand(CloseBarCode);
                }

                return closeBarCodeCommand;
            }
        }

        #endregion

        #region Methods
        public async void Initialize(CouponsPage page)
        {
            this.couponsPage = page;
            Coupons = new ObservableCollection<Coupon>();
            IsBarcodeVisible = false;
            await LoadCoupons();
        }

        public async Task LoadCoupons()
        {
            IsBusy = true;
            LoadingMessage = "Loading...";

            var surveyResponses = await ServiceLocator.AzureService.GetSurveyResponsesByUserId(Helpers.Settings.UserId);

            if (surveyResponses.Count > 0)
            {
                foreach (SurveyResponse s in surveyResponses)
                {
                    if (s.CouponId != null)
                    {
                        var coupon = await ServiceLocator.AzureService.GetById<Coupon>(s.CouponId);
                        coupon.CreatedDate = s.CreatedAt;
                        Coupons.Add(coupon);
                    }
                }
            }
            else
            {
                await couponsPage.DisplayAlert("Sorry", "No coupons available, please take a survey.", "ok");
            }

            IsBusy = false;
        }

        public void CouponSelected(Coupon item)
        {
            if (item != null)
            {
                CouponName = item.CouponName;
                BarcodeImage = item.BarCodeImage;
                CouponTerms = item.CouponTerms;
                IsBarcodeVisible = true;
            }
        }
        public void CloseBarCode(object obj)
        {
            IsBarcodeVisible = false;
            SelectedCoupon = null;
            CouponName = string.Empty;
            CouponTerms = string.Empty;
            BarcodeImage = string.Empty;
        }
        #endregion
    }
}
