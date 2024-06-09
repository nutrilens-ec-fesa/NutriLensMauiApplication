using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CryptographyLibrary;
using ExceptionLibrary;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLensClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriLens.ViewModels
{
    public partial class RegistrationPageVm : ObservableObject
    {
        public INavigation _navigation;

        public string NameEntry { get; set; }
        public string EmailEntry { get; set; }
        public string EmailConfirmEntry { get; set; }
        public string PasswordEntry { get; set; }
        public string PasswordConfirmEntry { get; set; }

        public RegistrationPageVm(INavigation navigation)
        {
            _navigation = navigation;
        }

        [RelayCommand]
        public async Task Register()
        {
            if (string.IsNullOrEmpty(NameEntry))
            {
                await ViewServices.PopUpManager.PopPersonalizedAsync("Nome não informado", "Você não seu nome, por favor, informe um nome.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(EmailEntry))
            {
                await ViewServices.PopUpManager.PopPersonalizedAsync("E-mail não informado", "Você não informou um e-mail, por favor, informe um e-mail.", "OK");
                return;
            }

            if (EmailEntry != EmailConfirmEntry)
            {
                await ViewServices.PopUpManager.PopPersonalizedAsync("E-mails não conferem", "E-mails informados não conferem, verifique e tente novamente.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(PasswordEntry))
            {
                await ViewServices.PopUpManager.PopPersonalizedAsync("Senha não informada", "Você não informou uma senha, verifique e tente novamente.", "OK");
                return;
            }

            if (PasswordEntry != PasswordConfirmEntry)
            {
                await ViewServices.PopUpManager.PopPersonalizedAsync("Senhas não conferem", "Senhas informadas não conferem, verifique e tente novamente.", "OK");
                return;
            }

            Login newLogin = new()
            {
                Name = NameEntry,
                Email = EmailEntry,
                Password = CryptographyManager.GetHashPassword(PasswordEntry)
            };

            EntitiesHelperClass.ShowLoading("Criando novo usuário.");

            try
            {
                await Task.Run(() => DaoHelperClass.InsertNewLoginModel(newLogin));
            }
            catch (Exception ex)
            {
                await EntitiesHelperClass.CloseLoading();
                await ViewServices.PopUpManager.PopErrorAsync("Houve algum problema para inserir o novo usuário tente novamente mais tarde. " + ExceptionManager.ExceptionMessage(ex));
                return;
            }

            await EntitiesHelperClass.CloseLoading();

            await ViewServices.PopUpManager.PopPersonalizedAsync("Usuário criado", "O usuário foi criado com sucesso!", "OK");

            await _navigation.PopAsync();
        }

        [RelayCommand]
        public async Task GoBack()
        {
            if (await ViewServices.PopUpManager.PopYesOrNoAsync("Registrar usuário", "Deseja realmente cancelar o registro de novo usuário e voltar para a tela de login?"))
                await _navigation.PopAsync();
        }
    }
}
