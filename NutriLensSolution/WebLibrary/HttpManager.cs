using ExceptionLibrary;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using WebLibrary.HttpRequests;

namespace WebLibrary;

/// <summary>
/// Enumerador dos comandos HTTP
/// </summary>
public enum HttpCommand
{
    Get,
    Post,
    Put,
    Delete
};

/// <summary>
/// Classe de requisições HTTP
/// </summary>
public static class HttpManager

{
    #region Public Methods

    /// <summary>
    /// Realiza uma requisição HTTP a partir de uma classe de requisição previamente
    /// instânciada e configurada
    /// </summary>
    /// <param name="httpRequest">Configuração da requisição HTTP</param>
    /// <param name="content">Content da requisição em string</param>
    /// <returns>Objeto da resposta da requisição HTTP</returns>
    /// <exception cref="HttpRequestException"></exception>
    public static HttpResponseMessage Request(HttpRequest httpRequest, out string content)
    {
        try
        {
            HttpResponseMessage response = HttpClientResponse(httpRequest);
            content = GetContentAsString(response);
            return response;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Falha na requisição Http", ex);
        }
    }

    /// <summary>
    /// Retorna a conversão do content de uma requisição HTTP para o tipo desejado
    /// </summary>
    /// <typeparam name="T">Tipo desejado para retorno</typeparam>
    /// <param name="content">Content de requisição HTTP em string</param>
    /// <returns>Item correspondente ao tipo passado como referência</returns>
    /// <exception cref="HttpContentParseException"></exception>
    public static T GetContent<T>(string content)
    {
        if (string.IsNullOrEmpty(content))
            throw new HttpContentParseException("Nenhum content a ser convertido.");

        if (typeof(T) == typeof(int))
        {
            if (int.TryParse(content, out int value))
                return (T)(object)value;
        }
        else if (typeof(T) == typeof(long))
        {
            if (long.TryParse(content, out long value))
                return (T)(object)value;
        }
        else if (typeof(T) == typeof(bool))
        {
            if (bool.TryParse(content, out bool value))
                return (T)(object)value;
        }
        else
            return JsonConvert.DeserializeObject<T>(content);

        throw new HttpContentParseException($"Falha na conversão do content para {typeof(T)}");
    }

    /// <summary>
    /// Realiza o download do arquivo no caminho especificado
    /// </summary>
    /// <param name="localPath">Caminho de destino do arquivo</param>
    /// <param name="remotePath">Caminho de origem do arquivo</param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException"></exception>
    public static async Task<bool> DownloadFileAsync(string localPath, string remotePath)
    {
        using HttpClient client = new();

        try
        {
            using HttpResponseMessage response = await client.GetAsync(remotePath);
            response.EnsureSuccessStatusCode();
            using Stream contentStream = await response.Content.ReadAsStreamAsync();
            using FileStream fileStream = File.Create(localPath);
            await contentStream.CopyToAsync(fileStream);
            return true;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Falha no download do arquivo '{localPath}'", ex);
        }
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Retorna a string do content do retorno de uma requisição HTTP
    /// </summary>
    /// <param name="resp">Response de uma requisição HTTP</param>
    /// <returns></returns>
    private static string GetContentAsString(HttpResponseMessage resp)
    {
        var t = Task.Run(() => resp.Content.ReadAsStringAsync());
        t.Wait();
        return t.Result;
    }

    /// <summary>
    /// Método que realiza a requisição HTTP a partir de um objeto de requisição HTTP previamente
    /// configurado
    /// </summary>
    /// <param name="httpRequest">Objeto de requisição HTTP</param>
    /// <returns></returns>
    private static HttpResponseMessage HttpClientResponse(HttpRequest httpRequest)
    {
        if (httpRequest.Ssl)
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        HttpClientHandler clientHandler = new()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        };

        HttpClient client = new(clientHandler)
        {
            Timeout = TimeSpan.FromSeconds(httpRequest.Timeout),
            BaseAddress = new Uri(httpRequest.Url)
        };

        if (httpRequest.AuthenticatedRequest)
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", httpRequest.Token);
        else
            client.DefaultRequestHeaders.Accept.Clear();

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        Task<HttpResponseMessage> task = httpRequest.HttpCommand switch
        {
            HttpCommand.Get => Task.Run(() => client.GetAsync(httpRequest.Controller + httpRequest.ParametersUrl)),
            HttpCommand.Post => Task.Run(() => client.PostAsync(httpRequest.Controller + httpRequest.ParametersUrl, new StringContent((httpRequest as PostRequest).JsonBody, Encoding.UTF8, "application/json"))),
            HttpCommand.Put => Task.Run(() => client.PutAsync(httpRequest.Controller + httpRequest.ParametersUrl, new StringContent((httpRequest as PutRequest).JsonBody, Encoding.UTF8, "application/json"))),
            HttpCommand.Delete => Task.Run(() => client.DeleteAsync(httpRequest.Controller + httpRequest.ParametersUrl)),
            _ => Task.Run(() => client.GetAsync(httpRequest.Url)),
        };

        task.Wait();
        return task.Result;
    }

    #endregion

}
