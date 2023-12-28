using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using CryptographyLibrary;

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

            bool validUser = false;

            await Task.Run(() => validUser = DaoHelperClass.CheckUserLogin(LoginModel));

            await EntitiesHelperClass.CloseLoading();

            if (validUser)
                await _navigation.PushAsync(ViewServices.ResolvePage<IMainMenuPage>());
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
            // Se houver informações de usuário
            if (AppDataHelperClass.HasUserInfo)
            {
                // Vai direto para a tela inicial
                await _navigation.PushAsync(ViewServices.ResolvePage<IMainMenuPage>());
            }
        }
    }
}
