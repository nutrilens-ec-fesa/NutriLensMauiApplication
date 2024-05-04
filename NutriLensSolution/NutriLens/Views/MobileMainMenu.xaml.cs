using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;
using PopupLibrary;
using PermissionsLibrary;
using NutriLens.Services;
using Plugin.Maui.Audio;

namespace NutriLens.Views;

public partial class MobileMainMenu : ContentPage, IMainMenuPage
{
	public MobileMainMenu()
	{
		InitializeComponent();
        BindingContext = new MainMenuPageVM(Navigation);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Tempo para a tela renderizar
        await Task.Run(() => Thread.Sleep(1000));

        ViewServices.PopUpManager.UpdateDeviceDisplay(DeviceDisplay.Current);
    }

    private void DoughnutSeries_BindingContextChanged(object sender, EventArgs e)
    {

    }
}