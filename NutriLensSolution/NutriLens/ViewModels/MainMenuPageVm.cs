using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Services;
using NutriLens.ViewInterfaces;

namespace NutriLens.ViewModels
{
    public partial class MainMenuPageVM : ObservableObject
    {
        private INavigation _navigation;

        public MainMenuPageVM(INavigation navigation)
        {
            _navigation = navigation;
        }

        [RelayCommand]
        private async Task OpenFlyout()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IFlyoutPage>());
        }

        [RelayCommand]
        private async Task OpenCamera()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<ICameraPage>());
        }
    }
}
