using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CryptographyLibrary;
using ExceptionLibrary;
using NutriLens.Entities;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using NutriLens.Views.Popups;
using NutriLensClassLibrary.Models;
using PermissionsLibrary;

namespace NutriLens.ViewModels
{

    public partial class LoginPageVm : ObservableObject
    {
        private INavigation _navigation;

        public Login LoginModel { get; set; }
        public string PasswordEntry { get; set; }

        public LoginPageVm(INavigation navigation)
        {
            _navigation = navigation;
            LoginModel = new Login();
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrEmpty(LoginModel.Email))
            {
                await ViewServices.PopUpManager.PopPersonalizedAsync("E-mail não informado", "Você não informou o e-mail, por favor, informe um e-mail para login.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(PasswordEntry))
            {
                await ViewServices.PopUpManager.PopPersonalizedAsync("Senha não informada", "Você não informou a senha, por favor, informe a senha para login.", "OK");
                return;
            }

            LoginModel.Password = CryptographyManager.GetHashPassword(PasswordEntry);

            EntitiesHelperClass.ShowLoading("Verificando usuário...");

            try
            {
                await Task.Run(() => DaoHelperClass.Login(LoginModel));
            }
            catch (Exception ex)
            {
                await EntitiesHelperClass.CloseLoading();
                PasswordEntry = string.Empty;
                OnPropertyChanged(nameof(PasswordEntry));
                await ViewServices.PopUpManager.PopPersonalizedAsync("Falha ao logar", ExceptionManager.ExceptionMessage(ex) , "OK");
                return;
            }

            await EntitiesHelperClass.CloseLoading();

            AppDataHelperClass.SetUserInfo(DaoHelperClass.GetUserInfoByAuthenticatedUser());
            await _navigation.PushAsync(ViewServices.ResolvePage<IMainMenuPage>());

        }

        [RelayCommand]
        private async Task Register()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IRegistrationPage>());
        }

        [RelayCommand]
        private async Task Appearing()
        {
            EntitiesHelperClass.ShowLoading("    Carregando    ");

            // Tempo para a tela renderizar
            await Task.Run(() => Thread.Sleep(1000));

            ViewServices.PopUpManager.UpdateDeviceDisplay(DeviceDisplay.Current);

            await HandlePermissions();


            bool hasUserInfo = false;

            await Task.Run(() => hasUserInfo = AppDataHelperClass.HasUserInfo);

            await EntitiesHelperClass.CloseLoading();

            // Se houver informações de usuário
            if (hasUserInfo)
            {
                EntitiesHelperClass.ShowLoading("Realizando login automático");

                await Task.Run(() => DaoHelperClass.LoginWithUserInfoId(AppDataHelperClass.UserInfo.Id));

                await EntitiesHelperClass.CloseLoading();

                EntitiesHelperClass.ShowLoading("Verificando termos de uso");

                bool termsAccepted = false;

                await Task.Run(() => termsAccepted = DaoHelperClass.GetTermsAcceptedByAuthenticatedUser());

                await EntitiesHelperClass.CloseLoading();

                if (!termsAccepted)
                {
                    TermsOfUsePopup termsOfUsePopup = new TermsOfUsePopup();
                    await Application.Current.MainPage.ShowPopupAsync(termsOfUsePopup);
                }

                // Chama a StartPage 
                // Retira a LaunchPage da Stack, para não permitir que o usuário possa acessá-la via botão voltar           
                _navigation.InsertPageBefore(ViewServices.ResolvePage<IMainMenuPage>(), _navigation.NavigationStack.Last());
                await _navigation.PopAsync();
            }
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
            neededPermissions.Add(new(PermissionType.Microfone, requiredPermissionTitle, "Para realizar análise dos alimentos por voz, precisamos de acesso ao microfone do dispositivo."));

#if ANDROID33_0_OR_GREATER

#else
            neededPermissions.Add(new(PermissionType.StorageWrite, requiredPermissionTitle, "Para salvar as fotos da sua alimentação, precisamos da permissão de escrita no seu dispositivo"));
            neededPermissions.Add(new(PermissionType.StorageRead, requiredPermissionTitle, "Para resgatar seus dados da aplicação, precisamos de acesso a leitura dos dados do dispositivo"));
#endif
            neededPermissions.Add(new(PermissionType.Photos, requiredPermissionTitle, "Para salvar e resgatar as fotos da sua alimentação, precisamos de acesso as fotos do dispositivo."));
            neededPermissions.Add(new(PermissionType.Microfone, requiredPermissionTitle, "Para realizar análise dos alimentos por voz, precisamos de acesso ao microfone do dispositivo."));

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
}
