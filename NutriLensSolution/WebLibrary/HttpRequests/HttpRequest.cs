namespace WebLibrary.HttpRequests
{
    /// <summary>
    /// Classe base para requisições HTTP
    /// </summary>
    public class HttpRequest
    {
        #region Properties

        /// <summary>
        /// Comando da requisição HTTP
        /// </summary>
        public HttpCommand HttpCommand { get; protected set; }

        /// <summary>
        /// Url base da requisição
        /// </summary>
        public string Url { get; protected set; }

        /// <summary>
        /// Controller de destino da requisição
        /// </summary>
        public string Controller { get; protected set; }

        /// <summary>
        /// Parâmetros adicionais da URL
        /// </summary>
        public string[] Parameters { get; protected set; }

        /// <summary>
        /// Token da requisição (deve ser preenchido em caso de rota autenticada)
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Configuração Secure Sockets Layer
        /// </summary>
        public bool Ssl { get; set; } = true;

        /// <summary>
        /// Timeout da requisição em segundos
        /// </summary>
        public int Timeout { get; set; } = 30;

        /// <summary>
        /// Flag que indica que a requisição tem como destido uma rota autenticada
        /// </summary>
        public bool AuthenticatedRequest { get => !string.IsNullOrEmpty(Token); }

        /// <summary>
        /// Retorna os parâmetros adicionais da URL no estilo "~/param1/param2/.../paramN"
        /// </summary>
        public string ParametersUrl
        {
            get
            {
                string parametersUrl = "/";

                foreach (string param in Parameters)
                {
                    parametersUrl += param + "/";
                }

                return parametersUrl;
            }
        }

        /// <summary>
        /// Indica a URL completa gerada pela URL base, controller e parâmetros
        /// </summary>
        public string FullUrl { get => $"{Url}/{Controller}{ParametersUrl}"; }

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor com URL base e controller
        /// </summary>
        /// <param name="url"></param>
        /// <param name="controller"></param>
        protected HttpRequest(string url, string controller)
        {
            Url = url;
            Controller = controller;
        }

        /// <summary>
        /// Contrutor com URL base, controller e parâmetros de URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="controller"></param>
        /// <param name="parameters"></param>
        protected HttpRequest(string url, string controller, params string[] parameters)
        {
            Url = url;
            Controller = controller;
            Parameters = parameters;
        }

        #endregion
    }
}