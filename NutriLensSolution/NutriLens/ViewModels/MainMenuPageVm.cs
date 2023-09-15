using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using PopupLibrary;
using System.Reflection;

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

        [RelayCommand]
        private async Task OpenBarCode()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IBarCodePage>());
        }

        [RelayCommand]
        private async Task OpenManualInput()
        {
            await ViewServices.PopUpManager.PopInDevelopment(MethodBase.GetCurrentMethod());
        }
    }
}
