using CommunityToolkit.Maui.Views;
using NutriLens.Entities;
using NutriLens.Services;
using NutriLensClassLibrary.Models;
using ExceptionLibrary;

namespace NutriLens.Views.Popups;

public partial class TermsOfUsePopup : Popup
{
    private bool _alreadyAccepted;

    public bool TermsAccepted { get => switchTermsAcceptance.IsToggled; }

    public TermsOfUsePopup()
    {
        InitializeComponent();
        _alreadyAccepted = false;
    }

    public TermsOfUsePopup(bool alreadyAccepted)
    {
        InitializeComponent();
        _alreadyAccepted = alreadyAccepted;
        switchTermsAcceptance.IsToggled = _alreadyAccepted;
    }

    private async void btnContinue_Clicked(object sender, EventArgs e)
    {
        if (!TermsAccepted)
        {
            if (_alreadyAccepted)
            {
                if (await ViewServices.PopUpManager.PopYesOrNoAsync("Atenção", "Deseja realmente revogar o aceite dos Termos de Uso e Privacidade? Lembrando que só é possível utilizar a aplicação ao aceitar os termos e continuar!"))
                {
                    EntitiesHelperClass.ShowLoading("Registrando revogação dos Termos de Uso e Privacidade.");

                    await Task.Run(() => DaoHelperClass.UpdateTermsAcceptedByAuthenticatedUser(false));

                    EntitiesHelperClass.CloseLoading();

                    EntitiesHelperClass.ShowLoading("Aceite dos termos revogado! Fechando aplicação.");

                    await Task.Delay(3000);

                    Application.Current.Quit();
                }
            }
            else
            {
                ViewServices.PopUpManager.PopInfoAsync("É necessário aceitar os Termos de Uso e Privacidade para continuar.");
                return;
            }
        }
        else
        {
            if (!_alreadyAccepted)
            {
                try
                {
                    EntitiesHelperClass.ShowLoading("Registrando aceite dos Termos de Uso e Privacidade.");

                    await Task.Run(() => DaoHelperClass.UpdateTermsAcceptedByAuthenticatedUser(true));

                    EntitiesHelperClass.CloseLoading();

                    await CloseAsync();
                }
                catch (Exception ex)
                {
                    await EntitiesHelperClass.CloseLoading();
                    await ViewServices.PopUpManager.PopErrorAsync("Houve alguma falha para registrar o aceite dos Termos de Uso e Privacidade, tente novamente mais tarde." + ExceptionManager.ExceptionMessage(ex));
                }
            }
            else
                await CloseAsync();
        }
    }

    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        if (!_alreadyAccepted)
        {
            if (await ViewServices.PopUpManager.PopYesOrNoAsync("Atenção", "Deseja realmente cancelar? Lembrando que só é possível utilizar a aplicação ao aceitar os termos e continuar!"))
            {
                EntitiesHelperClass.ShowLoading("Termos de uso não aceitos, fechando a aplicação...");

                await Task.Delay(3000);

                Application.Current.Quit();
            }
        }
        else
            await CloseAsync();
    }

    private async void termsOfUseTapped(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new(UriAndPaths.termsOfUsePath);
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            await ViewServices.PopUpManager.PopErrorAsync("Houve algum erro para abrir os termos de uso e privacidade. " + ExceptionManager.ExceptionMessage(ex));
        }
    }
}