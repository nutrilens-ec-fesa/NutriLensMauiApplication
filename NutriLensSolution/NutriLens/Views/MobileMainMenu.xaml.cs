using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;
using PopupLibrary;
using PermissionsLibrary;
using NutriLens.Services;

namespace NutriLens.Views;

public partial class MobileMainMenu : ContentPage, IMainMenuPage
{
	public MobileMainMenu()
	{
		InitializeComponent();
        BindingContext = new MainMenuPageVM(Navigation);
    }

    /// <summary>
    /// Chama a StartPage no evento de OnAppearing
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Tempo para a tela renderizar
        await Task.Run(() => Thread.Sleep(1000));

        ViewServices.PopUpManager.UpdateDeviceDisplay(DeviceDisplay.Current);

        // Chama o comando de navegar para a StartPage
        await HandlePermissions();
    }

    /// <summary>
    /// Gerencia/solicita permissões do android
    /// </summary>
    /// <returns></returns>
    private async Task HandlePermissions()
    {
#if ANDROID || IOS
        List<PermissionItem> neededPermissions = new List<PermissionItem>();

        string requiredPermissionTitle = "Permissão necessária";

        neededPermissions.Add(new(PermissionType.Câmera, requiredPermissionTitle, "Para utilizar a visão computacional do NutriLens, precisamos de acesso a câmera durante a execução do aplicativo."));

        ViewServices.PermissionManager.SetNeededPermissions(neededPermissions);
        ViewServices.PermissionManager.EventInfoMessage += PermissionManager_EventInfoMessage;
        await ViewServices.PermissionManager.CheckAndRequestPermissionsStatus();
#endif
    }

    /// <summary>
    /// Delegate para o PermissionManager conseguir utilizar popups
    /// </summary>
    /// <param name="title"></param>
    /// <param name="msg"></param>
    /// <param name="btn1"></param>
    /// <param name="btn2"></param>
    /// <param name="btn3"></param>
    /// <returns></returns>
    private async Task PermissionManager_EventInfoMessage(string title, string msg, string btn1, string btn2, string btn3)
    {
        await ViewServices.PopUpManager.PopFullyPersonalizedAsync(title, msg, btn1, btn2, btn3);
    }
}