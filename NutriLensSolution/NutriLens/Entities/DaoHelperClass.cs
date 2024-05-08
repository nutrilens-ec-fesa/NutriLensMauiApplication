using ExceptionLibrary;
using Newtonsoft.Json;
using NutriLensClassLibrary.Models;
using System.Net;
using WebLibrary;
using WebLibrary.HttpRequests;

namespace NutriLens.Entities
{
    public static class DaoHelperClass
    {
        #region Login

        /// <summary>
        /// Efetua o login na API do NutriLens e retorna o token de acesso aos recursos
        /// </summary>
        /// <param name="login">Objeto de login</param>
        /// <returns></returns>
        public static void Login(Login login)
        {
            PostRequest httpRequest = new(UriAndPaths.ApiUrl, "Auth/v1/Login")
            {
                Body = login,
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (resp.IsSuccessStatusCode)
                AppDataHelperClass.SetNutriLensApiToken(content);
            else
                throw new UnsuccessfullRequestException(content);
        }

        /// <summary>
        /// Efetua o login na API do NutriLens e retorna o token de acesso aos recursos
        /// </summary>
        /// <param name="userInfoId">ID de informações do usuário</param>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static void LoginWithUserInfoId(string userInfoId)
        {
            PostRequest httpRequest = new(UriAndPaths.ApiUrl, "Auth/v1/LoginByUserInfoId")
            {
                Body = userInfoId,
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (resp.IsSuccessStatusCode)
                AppDataHelperClass.SetNutriLensApiToken(content);
            else
                throw new UnsuccessfullRequestException(content);
        }

        /// <summary>
        /// Insere um novo login na base de dados
        /// </summary>
        /// <param name="login">Objeto de login</param>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static void InsertNewLoginModel(Login login)
        {
            PostRequest httpRequest = new(UriAndPaths.ApiUrl, "User/v1/InsertNewUser")
            {
                Body = login,
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
        }

        #endregion

        #region User Info

        /// <summary>
        /// Obtém informações de usuário a partir do usuário autenticado na aplicação
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static UserInfo GetUserInfoByAuthenticatedUser()
        {
            GetRequest httpRequest = new(UriAndPaths.ApiUrl, "User/v1/GetUserInfo")
            {
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return HttpManager.GetContent<UserInfo>(content);
        }

        /// <summary>
        /// Atualiza as informações de usuário na base de dados
        /// </summary>
        /// <param name="userInfo">Informações de usuário a serem atualizadas</param>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static void UpdateUserInfo(UserInfo userInfo)
        {
            PutRequest httpRequest = new(UriAndPaths.ApiUrl, "User/v1/UpdateUserInfo")
            {
                Body = userInfo,
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
        }

        /// <summary>
        /// Verifica na base de dados se o usuário aceitou os termos de uso da aplicação
        /// </summary>
        /// <returns>True, caso aceitou. False, caso contrário</returns>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static bool GetTermsAcceptedByAuthenticatedUser()
        {
            GetRequest httpRequest = new(UriAndPaths.ApiUrl, "User/v1/GetTermsAccepted")
            {
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return HttpManager.GetContent<bool>(content);
        }

        public static void UpdateTermsAcceptedByAuthenticatedUser(bool termsAccepted)
        {
            PutRequest httpRequest = new(UriAndPaths.ApiUrl, "User/v1/UpdateTermsAccepted", termsAccepted.ToString())
            {
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return;
        }

        #endregion

        #region Open AI

        /// <summary>
        /// Realiza análise dos alimentos presentes em uma imagem
        /// </summary>
        /// <param name="localPath">Caminho local da imagem</param>
        /// <returns></returns>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static string GetFoodVisionAnalisysByLocalPath(string localPath)
        {
            byte[] imageBytes = File.ReadAllBytes(localPath);
            string base64Image = Convert.ToBase64String(imageBytes);

            PostRequest httpRequest = new(UriAndPaths.ApiUrl, "Ai/v1/DetectFoodByBase64Image")
            {
                Body = base64Image,
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return content.Replace("\"", string.Empty).Replace("\\n", Environment.NewLine);
        }

        /// <summary>
        /// Realiza análise dos alimentos presentes em uma imagem
        /// </summary>
        /// <param name="imageId">Identificador da imagem na base de dados</param>
        /// <returns></returns>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static string GetOpenAiFoodVisionAnalisysByImageId(string imageId)
        {
            PostRequest httpRequest = new(UriAndPaths.ApiUrl, "Ai/v1/DetectFoodByMongoImageId", imageId)
            {
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return content.Replace("\"", string.Empty).Replace("\\n", Environment.NewLine);
        }

        /// <summary>
        /// Obtém o último prompt da OpenAi registrado
        /// </summary>
        /// <returns></returns>
        public static OpenAiPrompt GetOpenAiPrompt()
        {
            GetRequest httpRequest = new(UriAndPaths.ApiUrl, "Ai/v1/GetActualGpt4VisionPrompt")
            {
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return JsonConvert.DeserializeObject<OpenAiPrompt>(content);
        }

        /// <summary>
        /// Posta na base de dados o novo prompt para detecção de alimentos
        /// </summary>
        /// <param name="openAiPrompt"></param>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static void PostNewOpenAiPrompt(OpenAiPrompt openAiPrompt)
        {
            PostRequest httpRequest = new(UriAndPaths.ApiUrl, "Ai/v1/InsertNewGpt4VisionPrompt")
            {
                Body = openAiPrompt,
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return;
        }

        /// <summary>
        /// A partir de uma string descrevendo os alimentos, retorna um json composto por
        /// Item e Quantidade de cada um dos itens mencionados
        /// </summary>
        /// <param name="mealDescription">string de descrição dos itens da refeição</param>
        /// <returns></returns>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static string GetOpenAiFoodItemsJsonByMealDescription(string mealDescription)
        {
            PostRequest httpRequest = new(UriAndPaths.ApiUrl, "Ai/v1/GetFoodItemsJsonByMealDescription")
            {
                Token = AppDataHelperClass.NutriLensApiToken,
                Body = new StringObject { Value = mealDescription }
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return content.Replace("\"", string.Empty).Replace("\\n", Environment.NewLine);
        }

        #endregion

        #region Gemini


        /// <summary>
        /// Realiza análise dos alimentos presentes em uma imagem
        /// </summary>
        /// <param name="imageId">Identificador da imagem na base de dados</param>
        /// <returns></returns>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static string GetGeminiAiFoodVisionAnalisysByImageId(string imageId)
        {
            PostRequest httpRequest = new(UriAndPaths.ApiUrl, "Ai/v1/DetectFoodByMongoImageId/gemini", imageId)
            {
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return content.Replace("\"", string.Empty).Replace("\\n", Environment.NewLine);
        }

        /// <summary>
        /// A partir de uma string descrevendo os alimentos, retorna um json composto por
        /// Item e Quantidade de cada um dos itens mencionados
        /// </summary>
        /// <param name="mealDescription">string de descrição dos itens da refeição</param>
        /// <returns></returns>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static string GetGeminiFoodItemsJsonByMealDescription(string mealDescription)
        {
            PostRequest httpRequest = new(UriAndPaths.ApiUrl, "Ai/v1/GetFoodItemsJsonByMealDescription/gemini")
            {
                Token = AppDataHelperClass.NutriLensApiToken,
                Body = new StringObject { Value = mealDescription }
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return content.Replace("\"", string.Empty).Replace("\\n", Environment.NewLine);
        }

        #endregion

        #region Both

        /// <summary>
        /// Realiza análise dos alimentos presentes em uma imagem, utilizando GPT e Gemini
        /// </summary>
        /// <param name="imageId">Identificador da imagem na base de dados</param>
        /// <returns></returns>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static AiResult GetFoodVisionAnalisysByImageId(string imageId)
        {
            PostRequest httpRequest = new(UriAndPaths.ApiUrl, "Ai/v2/DetectFoodByMongoImageId", imageId)
            {
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return HttpManager.GetContent<AiResult>(content);
        }

        /// <summary>
        /// A partir de uma string descrevendo os alimentos, retorna um json composto por
        /// Item e Quantidade de cada um dos itens mencionados, utilizando GPT e Gemini
        /// </summary>
        /// <param name="mealDescription">string de descrição dos itens da refeição</param>
        /// <returns></returns>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static AiResult GetFoodItemsJsonByMealDescription(string mealDescription)
        {
            PostRequest httpRequest = new(UriAndPaths.ApiUrl, "Ai/v2/GetFoodItemsJsonByMealDescription")
            {
                Token = AppDataHelperClass.NutriLensApiToken,
                Body = new StringObject { Value = mealDescription }
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return HttpManager.GetContent<AiResult>(content);
        }

        #endregion

        #region Image

        /// <summary>
        /// Realiza upload de imagem a partir de caminho de imagem capturada pela aplicação
        /// </summary>
        /// <param name="imagePath">Caminho da imagem</param>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static string UploadImage(string imagePath)
        {
            MongoImage image = new()
            {
                FileName = Path.GetFileName(imagePath),
                ImageBytes = File.ReadAllBytes(imagePath),
            };

            PostRequest httpRequest = new(UriAndPaths.ApiUrl, "Image/v1/UploadImage")
            {
                Body = image,
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return content.Replace("\"", string.Empty);
        }

        /// <summary>
        /// Realiza o download das imagens registradas na base de dados
        /// </summary>
        /// <param name="downloadDirectory">Diretório a ser armazenada as imagens</param>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static void DownloadImages(string downloadDirectory, out List<MongoImage> mongoImages)
        {
            GetRequest httpRequest = new(UriAndPaths.ApiUrl, "Image/v1/GetAllImages")
            {
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);

            mongoImages = HttpManager.GetContent<List<MongoImage>>(content);

            foreach (MongoImage mongoImage in mongoImages)
            {
                if (!string.IsNullOrEmpty(mongoImage.FileName) && !File.Exists(Path.Combine(downloadDirectory, mongoImage.FileName)))
                    File.WriteAllBytes(Path.Combine(downloadDirectory, mongoImage.FileName), mongoImage.ImageBytes);
            }
        }

        /// <summary>
        /// Realiza o download das imagens registradas na base de dados vinculadas ao usuário logado
        /// </summary>
        /// <param name="downloadDirectory">Diretório a ser armazenada as imagens</param>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static void DownloadUserImages(string downloadDirectory, out List<MongoImage> mongoImages)
        {
            GetRequest httpRequest = new(UriAndPaths.ApiUrl, "Image/v1/GetAllImagesByAuthUser")
            {
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);

            mongoImages = HttpManager.GetContent<List<MongoImage>>(content);

            foreach (MongoImage mongoImage in mongoImages)
            {
                if (!string.IsNullOrEmpty(mongoImage.FileName) && !File.Exists(Path.Combine(downloadDirectory, mongoImage.FileName)))
                    File.WriteAllBytes(Path.Combine(downloadDirectory, mongoImage.FileName), mongoImage.ImageBytes);
            }
        }

        /// <summary>
        /// Deleta uma imagem da base
        /// </summary>
        /// <param name="mongoImage">Imagem a ser deletada</param>
        public static void DeleteImage(MongoImage mongoImage)
        {
            DeleteRequest httpRequest = new(UriAndPaths.ApiUrl, "Image/v1/DeleteImageById", mongoImage.Id)
            {
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
        }

        #endregion

        #region Barcode

        /// <summary>
        /// Insere um novo item de código de barras na base
        /// </summary>
        /// <param name="barcodeItem">Item de código de barras</param>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static void InsertBarCodeItem(BarcodeItem barcodeItem)
        {
            PostRequest httpRequest = new(UriAndPaths.ApiUrl, "BarcodeItems/v1/InsertNew")
            {
                Body = barcodeItem,
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
        }

        /// <summary>
        /// Atualiza um item de código de barras
        /// </summary>
        /// <param name="barcodeItem">Item de código de barras</param>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static void UpdateBarCodeItem(BarcodeItem barcodeItem)
        {
            PutRequest httpRequest = new(UriAndPaths.ApiUrl, "BarcodeItems/v1/Update")
            {
                Body = barcodeItem,
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
        }

        /// <summary>
        /// Obtém um item de código de barras da base a partir do código de barras
        /// </summary>
        /// <param name="barcode">Código de barras</param>
        /// <returns></returns>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static BarcodeItemEntry GetBarCodeItem(string barcode)
        {
            GetRequest httpRequest = new(UriAndPaths.ApiUrl, "BarcodeItems/v1/GetByBarcode", barcode)
            {
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (resp.IsSuccessStatusCode)
                return HttpManager.GetContent<BarcodeItemEntry>(content);
            else if (resp.StatusCode == HttpStatusCode.NotFound)
                return null;
            else
                throw new UnsuccessfullRequestException(content);
        }

        /// <summary>
        /// Obtém a lista de todos os itens de código de barras registrados na base
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static List<BarcodeItemEntry> GetAllBarCodeItems()
        {
            GetRequest httpRequest = new(UriAndPaths.ApiUrl, "BarcodeItems/v1/GetList")
            {
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return HttpManager.GetContent<List<BarcodeItemEntry>>(content);
        }

        #endregion

        #region TBCA

        /// <summary>
        /// Obtém a lista de todos os itens da TBCA
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static List<TbcaItem> GetTbcaItemsList()
        {
            GetRequest httpRequest = new(UriAndPaths.ApiUrl, "Tbca/v1/GetList")
            {
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return HttpManager.GetContent<List<TbcaItem>>(content);
        }

        #endregion

        #region TACO

        /// <summary>
        /// Obtém a lista de todos os itens da TACO
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UnsuccessfullRequestException"></exception>
        public static List<TacoItem> GetTacoItemsList()
        {
            GetRequest httpRequest = new(UriAndPaths.ApiUrl, "Taco/v1/GetList")
            {
                Token = AppDataHelperClass.NutriLensApiToken
            };

            HttpResponseMessage resp = HttpManager.Request(httpRequest, out string content);

            if (!resp.IsSuccessStatusCode)
                throw new UnsuccessfullRequestException(content);
            else
                return HttpManager.GetContent<List<TacoItem>>(content);
        }

        #endregion
    }
}
