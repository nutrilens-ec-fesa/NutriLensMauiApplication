using CommunityToolkit.Maui.Views;
using DictionaryLibrary;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PopupLibrary;

public enum MessageTypeEnum { info, attention, error }
public enum BtnResult { Btn1 = 1, Btn2, Btn3 }

public delegate Task DelegatePopupText(MessageTypeEnum type, string value);
public delegate Task<int> DelegatePopupTextTwoOptions(string title, string msg, string btn1, string btn2);
public delegate Task DelegatePopupFullyPersonalized(string title, string msg, string btn1, string btn2, string btn3);
public delegate Task<int> DelegatePopupOptions(string title, params string[] options);

public interface IPopUpManager
{
    /// <summary>
    /// Inicializa o PopUpManager já construído
    /// </summary>    
    /// <param name="language">Linguagem a ser utilizada nos textos básicos</param>
    public void Initialize(Languages language);
    /// <summary>
    /// Atualiza a linguagem dos textos básicos
    /// </summary>
    /// <param name="language">Linguagem a ser utilizada nos textos básicos</param>
    public void UpdateLanguage(Languages language);
    public void UpdateDeviceDisplay(IDeviceDisplay deviceDisplay);
    /// <summary>
    /// Mostra um pop-up do tipo informado, uma mensagem passada por parâmetro e um botão de OK. Utiliza Popup nativo 
    /// </summary>
    /// <param name="type">Tipo da Mensagem</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <returns></returns>
    public Task PopMessage(MessageTypeEnum type, string message);
    /// <summary>
    /// Mostra um pop-up com o título de "Informação" no idioma configurado, uma mensagem
    /// passada por parâmetro e um botão de OK. Utiliza Popup nativo 
    /// </summary>
    /// <param name="message">Mensagem do corpo do pop-up</param>   
    public Task PopInfoAsync(string message);
    /// <summary>
    /// Mostra um pop-up com o título de "Atenção" no idioma configurado, uma mensagem
    /// passada por parâmetro e um botão de OK. Utiliza Popup nativo 
    /// </summary>
    /// <param name="message">Mensagem do corpo do pop-up</param>    
    public Task PopAttentionAsync(string message);
    /// <summary>
    /// Mostra um pop-up com o título de "Erro" no idioma configurado, uma mensagem
    /// passada por parâmetro e um botão de OK. Utiliza Popup nativo 
    /// </summary>
    /// <param name="message">Mensagem do corpo do pop-up</param>  
    public Task PopErrorAsync(string message);
    /// <summary>
    /// Mostra um pop-up com um título e mensagem passados por parâmetro, botões de "Sim" e "Não"
    /// no idioma configurado. Utiliza Popup nativo 
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <returns>True para "Sim" e false para "Não"</returns>
    public Task<bool> PopYesOrNoAsync(string title, string message);
    /// <summary>
    /// Mostra um pop-up com um título e dispõe uma lista de opções de acordo 
    /// com os params string passados, possui botão de cancelar. Utiliza Popup nativo 
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="options">Opções a serem selecionadas</param>
    /// <returns>Índice do item escolhido, para "cancelar" ou clique fora retorna -1</returns>
    public Task<int> PopCancelableOptionsPromptAsync(string title, params string[] options);
    /// <summary>
    /// Mostra um pop-up com um título e dispõe uma lista de opções de acordo 
    /// com os params string passados, não possui botão de cancelar. Utiliza Popup nativo 
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="options">Opções a serem selecionadas</param>
    /// <returns>Índice do item escolhido, clique fora retorna -1</returns>
    public Task<int> PopUncancelableOptionsPromptAsync(string title, params string[] options);
    /// <summary>
    /// Mostra um pop-up com título e mensagem passados por parâmetro e disponibiliza uma entrada de texto livre. Utiliza Popup nativo 
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <returns>Texto digitado pelo usuário</returns>
    public Task<string> PopFreeInputAsync(string title, string message);
    /// <summary>
    /// Mostra um pop-up com título e mensagem passados por parâmetro e disponibiliza uma entrada de texto numérica. Utiliza Popup nativo 
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <returns>Texto digitado pelo usuário</returns>
    public Task<string> PopNumericInputAsync(string title, string message);
    /// <summary>
    /// Mostra um pop-up com título e mensagem passados por parâmetro e disponibiliza uma entrada de texto livre. Utiliza Popup nativo 
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <param name="placeHolder">Place holder para entrada de texto</param>
    /// <returns>Texto digitado pelo usuário</returns>
    public Task<string> PopFreeInputAsync(string title, string message, string placeHolder);
    /// <summary>
    /// Mostra um pop-up com título e mensagem passados por parâmetro e disponibiliza uma entrada de texto numérica. Utiliza Popup nativo 
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <param name="placeHolder">Place holder para entrada de texto</param>
    /// <returns>Texto digitado pelo usuário</returns>
    public Task<string> PopNumericInputAsync(string title, string message, string placeHolder);
    /// <summary>
    /// Mostra um pop-up com título e mensagem passados por parâmetro e disponibiliza uma entrada de texto livre.
    /// Realiza validação a partir de regex, mostrando mensagem de erro em caso de preenchimento incorreto.
    /// Permanece nesse fluxo até que a entrada seja válida, ou o usuário clique no botão "Cancelar". Utiliza Popup nativo 
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <param name="placeHolder">Place holder para entrada de texto</param>
    /// <param name="regex">Regex para validação da entrada do usuário</param>
    /// <returns>Texto digitado pelo usuário</returns>
    public Task<string> PopRegexInputAsync(string title, string message, string placeHolder, Regex regex);
    /// <summary>
    /// Mostra um pop-up com título e mensagem passados por parâmetro e disponibiliza uma entrada de texto livre. Utiliza Popup nativo 
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <param name="maxLength">Limite de caracteres da entrada de texto</param>
    /// <returns>Texto digitado pelo usuário</returns>
    public Task<string> PopFreeInputAsync(string title, string message, int maxLength);
    /// <summary>
    /// Mostra um pop-up com título e mensagem passados por parâmetro e disponibiliza uma entrada de texto numérica. Utiliza Popup nativo 
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <param name="maxLength">Limite de caracteres da entrada de texto</param>
    /// <returns>Texto digitado pelo usuário</returns>
    public Task<string> PopNumericInputAsync(string title, string message, int maxLength);
    /// <summary>
    /// Mostra um pop-up com título e mensagem passados por parâmetro e disponibiliza uma entrada de texto livre. Utiliza Popup nativo 
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <param name="maxLength">Limite de caracteres da entrada de texto</param>
    /// <param name="placeHolder">Place holder para entrada de texto</param>
    /// <returns>Texto digitado pelo usuário</returns>
    public Task<string> PopFreeInputAsync(string title, string message, int maxLength, string placeHolder);
    /// <summary>
    /// Mostra um pop-up com título e mensagem passados por parâmetro e disponibiliza uma entrada de texto numérica. Utiliza Popup nativo 
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <param name="maxLength">Limite de caracteres da entrada de texto</param>
    /// <param name="placeHolder">Place holder para entrada de texto</param>
    /// <returns>Texto digitado pelo usuário</returns>
    public Task<string> PopNumericInputAsync(string title, string message, int maxLength, string placeHolder);
    /// <summary>
    /// Mostra um pop-up com título e mensagem passados por parâmetro e disponibiliza uma entrada de texto livre.
    /// Realiza validação a partir de regex, mostrando mensagem de erro em caso de preenchimento incorreto.
    /// Permanece nesse fluxo até que a entrada seja válida, ou o usuário clique no botão "Cancelar". Utiliza Popup nativo 
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <param name="maxLength">Limite de caracteres da entrada de texto</param>
    /// <param name="placeHolder">Place holder para entrada de texto</param>
    /// <param name="regex">Regex para validação da entrada do usuário</param>
    /// <returns>Texto digitado pelo usuário</returns>
    public Task<string> PopRegexInputAsync(string title, string message, int maxLength, string placeHolder, Regex regex);
    /// <summary>
    /// Mostra um pop-up com um título e mensagem passados por parâmetro, botão personalizado. Utiliza Popup nativo    
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <param name="btn1">Texto do botão 1</param>    
    public Task PopPersonalizedAsync(string title, string message, string btn1);
    /// <summary>
    /// Mostra um pop-up com um título e mensagem passados por parâmetro, botões personalizados. Utiliza Popup nativo    
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <param name="btn1">Texto do botão 1</param>
    /// <param name="btn2">Texto do botão 2</param>
    /// <returns>Índice do botão pressionado</returns>
    public Task<int> PopPersonalizedAsync(string title, string message, string btn1, string btn2);
    /// <summary>
    /// Mostra um pop-up com um título, mensagem e 1 botão personalizado. Utiliza Popup personalizado
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="btn1"></param>
    /// <param name="btn2"></param>
    /// <param name="btn3"></param>
    /// <returns></returns>
    public Task<int> PopFullyPersonalizedAsync(string title, string message, string btn1);
    /// <summary>
    /// Mostra um pop-up com um título, mensagem e até 2 botões personalizados. Utiliza Popup personalizado
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="btn1"></param>
    /// <param name="btn2"></param>
    /// <returns></returns>
    public Task<int> PopFullyPersonalizedAsync(string title, string message, string btn1, string btn2);
    /// <summary>
    /// Mostra um pop-up com um título, mensagem e até 3 botões personalizados. Utiliza Popup personalizado
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="btn1"></param>
    /// <param name="btn2"></param>
    /// <param name="btn3"></param>
    /// <returns></returns>
    public Task<int> PopFullyPersonalizedAsync(string title, string message, string btn1, string btn2, string btn3);
    /// <summary>
    /// Mostra um pop-up com um título e mensagem passados por parâmetro, botões de "Sim" e "Não"
    /// no idioma configurado. Utiliza Popup personalizado 
    /// </summary>
    /// <param name="title">Título do pop-up</param>
    /// <param name="message">Mensagem do corpo do pop-up</param>
    /// <returns>True para "Sim" e false para "Não"</returns>
    public Task<bool> PopFullyPersonalizedYesOrNoAsync(string title, string message);
    public Task PopInDevelopment(MethodBase method);
}

public class PopUpManager : IPopUpManager
{
    private string _infoTitle;
    private string _attentionTitle;
    private string _errorTitle;
    private string _yes;
    private string _no;
    private string _cancel;
    private string _wrongInput;
    private string _developmentMessage;
    private string _ok = "OK";

    private IDeviceDisplay _deviceDisplay;

    /// <summary>
    /// Propriedade que mostra se as chamadas de PopUp utilizarão Dispatcher
    /// </summary>
    public bool DispatcherEnabled { get; set; } = true;

    /// <summary>
    /// Retorna a página atual no topo da pilha de navegação
    /// </summary>
    private Page ActualPage => Application.Current.MainPage; //_navigation.NavigationStack[_navigation.NavigationStack.Count - 1]; 

    /// <summary>
    /// Contrutor genérico para o uso no startup
    /// </summary>
    public PopUpManager()
    {

    }

    /// <summary>
    /// Construtor com o objeto de navegação e a linguagem de configuração do PopUpManager
    /// </summary>    
    /// <param name="language">Linguagem a ser utilizada nos textos básicos</param>
    public PopUpManager(Languages language)
    {
        Initialize(language);
    }

    public void Initialize(Languages language)//INavigation navigation, Languages language)
    {
        //navigation = navigation;
        UpdateLanguage(language);
    }

    public void UpdateLanguage(Languages language)
    {
        switch (language)
        {
            case Languages.Spanish:
                _infoTitle = "Información";
                _attentionTitle = "Atención";
                _yes = "Sí";
                _no = "No";
                _cancel = "Cancelar";
                _errorTitle = "Error";
                _wrongInput = "El valor introducido no tiene el formato correcto. Inténtalo de nuevo.";
                _developmentMessage = "Función en desarrollo, espera las próximas actualizaciones =)";
                break;
            case Languages.English:
                _infoTitle = "Information";
                _attentionTitle = "Attention";
                _yes = "Yes";
                _no = "No";
                _cancel = "Cancel";
                _errorTitle = "Error";
                _wrongInput = "The input value is not in the correct format. Try again.";
                _developmentMessage = "Function in development, please, wait for the next updates =)";
                break;
            default:
                _infoTitle = "Informação";
                _attentionTitle = "Atenção";
                _yes = "Sim";
                _no = "Não";
                _cancel = "Cancelar";
                _errorTitle = "Erro";
                _wrongInput = "O valor inserido não está no formato correto. Tente novamente.";
                _developmentMessage = "Função em desenvolvimento, por favor, aguarde as próximas atualizações =)";
                break;
        }
    }

    public void UpdateDeviceDisplay(IDeviceDisplay deviceDisplay)
    {
        _deviceDisplay = deviceDisplay;
    }

    public async Task PopMessage(MessageTypeEnum type, string message)
    {
        switch (type)
        {
            case MessageTypeEnum.info: await PopInfoAsync(message); break;
            case MessageTypeEnum.attention: await PopAttentionAsync(message); break;
            case MessageTypeEnum.error: await PopErrorAsync(message); break;
        }
    }

    public async Task PopInfoAsync(string message)
    {
        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                await ActualPage.DisplayAlert(_infoTitle, message, _ok);
            });
        }
        else
            await ActualPage.DisplayAlert(_infoTitle, message, _ok);
    }

    public async Task PopAttentionAsync(string message)
    {
        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                await ActualPage.DisplayAlert(_attentionTitle, message, _ok);
            });
        }
        else
            await ActualPage.DisplayAlert(_attentionTitle, message, _ok);
    }

    public async Task PopErrorAsync(string message)
    {
        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                await ActualPage.DisplayAlert(_errorTitle, message, _ok);
            });
        }
        else
            await ActualPage.DisplayAlert(_errorTitle, message, _ok);
    }

    public async Task<bool> PopYesOrNoAsync(string title, string message)
    {
        bool accepted = false;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                accepted = await ActualPage.DisplayAlert(title, message, _yes, _no);
            });
        }
        else
            accepted = await ActualPage.DisplayAlert(title, message, _yes, _no);

        return accepted;
    }

    public async Task<int> PopCancelableOptionsPromptAsync(string title, params string[] options)
    {
        string selectedOption = string.Empty;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                selectedOption = await ActualPage.DisplayActionSheet(title, _cancel, null, options);
            });
        }
        else
            selectedOption = await ActualPage.DisplayActionSheet(title, _cancel, null, options);

        if (selectedOption != null)
            return Array.IndexOf(options, selectedOption);
        else
            return -1;
    }

    public async Task<int> PopUncancelableOptionsPromptAsync(string title, params string[] options)
    {
        string selectedOption = string.Empty;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                selectedOption = await ActualPage.DisplayActionSheet(title, null, null, options);
            });
        }
        else
            selectedOption = await ActualPage.DisplayActionSheet(title, null, null, options);

        if (selectedOption != null)
            return Array.IndexOf(options, selectedOption);
        else
            return -1;
    }

    public async Task<string> PopFreeInputAsync(string title, string message)
    {
        string inputText = string.Empty;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel);
            });
        }
        else
            inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel);

        return inputText;
    }

    public async Task<string> PopNumericInputAsync(string title, string message)
    {
        string inputText = string.Empty;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, null, 5, Keyboard.Numeric);
            });
        }
        else
            inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, null, 5, Keyboard.Numeric);

        return inputText;
    }

    public async Task<string> PopFreeInputAsync(string title, string message, string placeHolder)
    {
        string inputText = string.Empty;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, placeHolder);
            });
        }
        else
            inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, placeHolder);

        return inputText;
    }

    public async Task<string> PopNumericInputAsync(string title, string message, string placeHolder)
    {
        string inputText = string.Empty;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, placeHolder, -1, Keyboard.Numeric);
            });
        }
        else
            inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, placeHolder, -1, Keyboard.Numeric);

        return inputText;
    }

    public async Task<string> PopRegexInputAsync(string title, string message, string placeHolder, Regex regex)
    {
        string result = null;

        if (!string.IsNullOrEmpty(message))
            message += Environment.NewLine;

        while (true)
        {
            if (DispatcherEnabled)
            {
                await Application.Current.Dispatcher.DispatchAsync(async () =>
                {
                    result = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, placeHolder, -1);
                });
            }
            else
                result = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, placeHolder, -1);

            if (result == null) // Entrada cancelada pelo usuário
                break;
            else if (regex.IsMatch(result))
                break;
            else
            {
                await PopAttentionAsync(_wrongInput);
                continue;
            }
        }

        return result;
    }

    public async Task<string> PopFreeInputAsync(string title, string message, int maxLength)
    {
        string inputText = string.Empty;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, null, maxLength);
            });
        }
        else
            inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, null, maxLength);

        return inputText;
    }

    public async Task<string> PopNumericInputAsync(string title, string message, int maxLength)
    {
        string inputText = string.Empty;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, null, maxLength, Keyboard.Numeric);
            });
        }
        else
            inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, null, maxLength, Keyboard.Numeric);

        return inputText;
    }

    public async Task<string> PopFreeInputAsync(string title, string message, int maxLength, string placeHolder)
    {
        string inputText = string.Empty;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, placeHolder, maxLength);
            });
        }
        else
            inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, placeHolder, maxLength);

        return inputText;
    }

    public async Task<string> PopNumericInputAsync(string title, string message, int maxLength, string placeHolder)
    {
        string inputText = string.Empty;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, placeHolder, maxLength, Keyboard.Numeric);
            });
        }
        else
            inputText = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, placeHolder, maxLength, Keyboard.Numeric);

        return inputText;
    }

    public async Task<string> PopRegexInputAsync(string title, string message, int maxLength, string placeHolder, Regex regex)
    {
        string result = null;

        if (!string.IsNullOrEmpty(message))
            message += Environment.NewLine;

        while (true)
        {
            if (DispatcherEnabled)
            {
                await Application.Current.Dispatcher.DispatchAsync(async () =>
                {
                    result = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, placeHolder, maxLength);
                });
            }
            else
                result = await ActualPage.DisplayPromptAsync(title, message, _ok, _cancel, placeHolder, maxLength);

            if (result == null) // Entrada cancelada pelo usuário
                break;
            else if (regex.IsMatch(result))
                break;
            else
            {
                await PopAttentionAsync(_wrongInput);
                continue;
            }
        }

        return result;
    }

    public async Task PopPersonalizedAsync(string title, string message, string btn1)
    {
        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                await ActualPage.DisplayAlert(title, message, btn1);
            });
        }
        else
            await ActualPage.DisplayAlert(title, message, btn1);
    }

    public async Task<int> PopPersonalizedAsync(string title, string message, string btn1, string btn2)
    {
        bool res = false;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                res = await ActualPage.DisplayAlert(title, message, btn1, btn2);
            });
        }
        else
            res = await ActualPage.DisplayAlert(title, message, btn1, btn2);

        return res ? 1 : 2;
    }

    public async Task<int> PopFullyPersonalizedAsync(string title, string message, string btn1)
    {
        int buttonQuantity = 0;

        if (!string.IsNullOrEmpty(btn1)) buttonQuantity++;

        CustomPopup customPopUp = new(_deviceDisplay, buttonQuantity);

        UpdatePersonalizedPopUpValues(customPopUp, title, message, btn1, string.Empty, string.Empty);

        int popResult = -1;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                popResult = (int)await ActualPage.ShowPopupAsync(customPopUp);
            });
        }
        else
            popResult = (int)await ActualPage.ShowPopupAsync(customPopUp);

        return popResult;
    }

    public async Task<int> PopFullyPersonalizedAsync(string title, string message, string btn1, string btn2)
    {
        int buttonQuantity = 0;

        if (!string.IsNullOrEmpty(btn1)) buttonQuantity++;
        if (!string.IsNullOrEmpty(btn2)) buttonQuantity++;

        CustomPopup customPopUp = new(_deviceDisplay, buttonQuantity);

        UpdatePersonalizedPopUpValues(customPopUp, title, message, btn1, btn2, string.Empty);

        int popResult = -1;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                popResult = (int)await ActualPage.ShowPopupAsync(customPopUp);
            });
        }
        else
            popResult = (int)await ActualPage.ShowPopupAsync(customPopUp);

        return popResult;
    }

    public async Task<int> PopFullyPersonalizedAsync(string title, string message, string btn1, string btn2, string btn3)
    {
        int buttonQuantity = 0;

        if (!string.IsNullOrEmpty(btn1)) buttonQuantity++;
        if (!string.IsNullOrEmpty(btn2)) buttonQuantity++;
        if (!string.IsNullOrEmpty(btn3)) buttonQuantity++;

        CustomPopup customPopUp = new(_deviceDisplay, buttonQuantity);

        UpdatePersonalizedPopUpValues(customPopUp, title, message, btn1, btn2, btn3);

        int popResult = -1;

        if (DispatcherEnabled)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                popResult = (int)await ActualPage.ShowPopupAsync(customPopUp);
            });
        }
        else
            popResult = (int)await ActualPage.ShowPopupAsync(customPopUp);

        return popResult;
    }

    public async Task PopInDevelopment(MethodBase method)
    {
        await PopPersonalizedAsync(method.Name, _developmentMessage, _ok);
    }

    public async Task<bool> PopFullyPersonalizedYesOrNoAsync(string title, string message)
    {
        int index = await PopFullyPersonalizedAsync(title, message, _yes, _no);
        return index == (int)BtnResult.Btn1;
    }

    /// <summary>
    /// Atualiza os valores do CustomPopUp de acordo com os dados passados
    /// </summary>
    /// <param name="customPopup">View CustomPopUp</param>
    /// <param name="title">Título do popup</param>
    /// <param name="message">Mensagem do popup</param>
    /// <param name="btn1">Texto do botão 1</param>
    /// <param name="btn2">Texto do botão 2</param>
    /// <param name="btn3">Texto do botão 3</param>
    private void UpdatePersonalizedPopUpValues(CustomPopup customPopup, string title, string message, string btn1, string btn2, string btn3)
    {
        foreach (IView child in ((customPopup.Content as Frame).Children.First() as StackBase).Children)
        {
            if (child is Label label)
            {
                switch (label.StyleId)
                {
                    case "lblTitle":
                        label.Text = title;
                        break;
                    case "lblMessage":
                        label.Text = message;
                        break;
                }
            }
            else if (child is Button button)
            {
                switch (button.StyleId)
                {
                    case "btn1":
                        button.Text = btn1;
                        break;
                    case "btn2":
                        button.Text = btn2;
                        break;
                    case "btn3":
                        button.Text = btn3;
                        break;
                }
            }
            else if (child is StackBase)
                UpdateStack(child as StackBase, title, message, btn1, btn2, btn3);
        }
    }

    /// <summary>
    /// Método recursivo que é chamado para atualizar os valores de dentro de Stacks
    /// </summary>
    /// <param name="stackBase">StackBase item</param>
    /// <param name="title">Título do popup</param>
    /// <param name="message">Mensagem do popup</param>
    /// <param name="btn1">Texto do botão 1</param>
    /// <param name="btn2">Texto do botão 2</param>
    /// <param name="btn3">Texto do botão 3</param>
    private void UpdateStack(StackBase stackBase, string title, string message, string btn1, string btn2, string btn3)
    {
        foreach (IView child in stackBase.Children)
        {
            if (child is Label label)
            {
                switch (label.StyleId)
                {
                    case "lblTitle":
                        label.Text = title;
                        break;
                    case "lblMessage":
                        label.Text = message;
                        break;
                }
            }
            else if (child is Button button)
            {
                switch (button.StyleId)
                {
                    case "btn1":
                        button.Text = btn1;
                        break;
                    case "btn2":
                        button.Text = btn2;
                        break;
                    case "btn3":
                        button.Text = btn3;
                        break;
                }
            }
            else if (child is StackBase)
                UpdateStack(child as StackBase, title, message, btn1, btn2, btn3);
        }
    }
}
