using BAMApp.ViewModels;

using Xamarin.Forms;

namespace BAMApp.Views
{
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();

            SignInViewModel vm = ViewModelLocator.SignInViewModel;
            vm.Initialize(this);
            BindingContext = vm;
        }
    }
}
