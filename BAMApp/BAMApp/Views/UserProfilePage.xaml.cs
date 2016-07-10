using BAMApp.ViewModels;

using Xamarin.Forms;

namespace BAMApp.Views
{
    public partial class UserProfilePage : ContentPage
    {
        public UserProfilePage()
        {
            InitializeComponent();

            UserViewModel vm = ViewModelLocator.UserViewModel;
            vm.Initialize(this);
            BindingContext = vm;
        }
    }
}
