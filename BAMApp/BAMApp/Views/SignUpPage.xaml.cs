using BAMApp.ViewModels;

using Xamarin.Forms;

namespace BAMApp.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();

            UserViewModel vm = ViewModelLocator.UserViewModel;
            vm.Initialize(this);
            BindingContext = vm;
            
        }
    }
}
