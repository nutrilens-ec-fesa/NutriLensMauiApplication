using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using CryptographyLibrary;
using PermissionsLibrary;
using NutriLensClassLibrary.Models;

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

            UserInfo userInfo = null;

            await Task.Run(() => userInfo = DaoHelperClass.CheckUserLogin(LoginModel));

            await EntitiesHelperClass.CloseLoading();

            if (userInfo != null)
            {
                AppDataHelperClass.SetUserInfo(userInfo);
                await _navigation.PushAsync(ViewServices.ResolvePage<IMainMenuPage>());
            }
            else
            {
                PasswordEntry = string.Empty;
                OnPropertyChanged(nameof(PasswordEntry));
                await ViewServices.PopUpManager.PopPersonalizedAsync("Usuário não encontrado", "Usuário e/ou senha incorretos, verifique e tente novamente", "OK");
            }
        }

        [RelayCommand]
        private async Task Register()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IRegistrationPage>());
        }

        [RelayCommand]
        private async Task Appearing()
        {
            // Tempo para a tela renderizar
            await Task.Run(() => Thread.Sleep(1000));

            ViewServices.PopUpManager.UpdateDeviceDisplay(DeviceDisplay.Current);

            // Chama o comando de navegar para a StartPage

            await HandlePermissions();

            // Se houver informações de usuário
            if (AppDataHelperClass.HasUserInfo)
            {
                // Vai direto para a tela inicial

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

#if ANDROID33_0_OR_GREATER

#else

            neededPermissions.Add(new(PermissionType.StorageWrite, requiredPermissionTitle, "Para salvar as fotos da sua alimentação, precisamos da permissão de escrita no seu dispositivo"));
            neededPermissions.Add(new(PermissionType.StorageRead, requiredPermissionTitle, "Para resgatar seus dados da aplicação, precisamos de acesso a leitura dos dados do dispositivo"));
#endif
            neededPermissions.Add(new(PermissionType.Photos, requiredPermissionTitle, "Para salvar e resgatar as fotos da sua alimentação, precisamos de acesso as fotos do dispositivo"));
            //neededPermissions.Add(new(PermissionType.Microfone, requiredPermissionTitle, "Para o teste de gravar vídeos, precisamos de acesso ao microfone"));

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
