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

        // Chama o comando de navegar para a StartPage
        await HandlePermissions();
    }

    /// <summary>
    /// Gerencia/solicita permiss�es do android
    /// </summary>
    /// <returns></returns>
    private async Task HandlePermissions()
    {
#if ANDROID || IOS
        List<PermissionItem> neededPermissions = new List<PermissionItem>();

        string requiredPermissionTitle = "Permiss�o necess�ria";

        neededPermissions.Add(new(PermissionType.C�mera, requiredPermissionTitle, "Para utilizar a vis�o computacional do NutriLens, precisamos de acesso a c�mera durante a execu��o do aplicativo."));
        neededPermissions.Add(new(PermissionType.StorageWrite, requiredPermissionTitle, "Para salvar as fotos da sua alimenta��o, precisamos da permiss�o de escrita no seu dispositivo"));
        neededPermissions.Add(new(PermissionType.StorageRead, requiredPermissionTitle, "Para resgatar seus dados da aplica��o, precisamos de acesso a leitura dos dados do dispositivo"));
        neededPermissions.Add(new(PermissionType.Photos, requiredPermissionTitle, "Para salvar e resgatar as fotos da sua alimenta��o, precisamos de acesso as fotos do dispositivo")); 
        //neededPermissions.Add(new(PermissionType.Microfone, requiredPermissionTitle, "Para o teste de gravar v�deos, precisamos de acesso ao microfone"));

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