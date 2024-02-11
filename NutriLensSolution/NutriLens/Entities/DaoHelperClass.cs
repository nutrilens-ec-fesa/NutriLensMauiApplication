using ExceptionLibrary;
using MongoDB.Bson;
using MongoDB.Driver;
using NutriLens.Models;
using NutriLens.Services;
using NutriLensClassLibrary.Models;

namespace NutriLens.Entities
{
    public static class DaoHelperClass
    {
        private const string _connectionUri = "mongodb://nutrilens:nutri0821900lens@ac-k5cpl72-shard-00-00.6xjkrgj.mongodb.net:27017,ac-k5cpl72-shard-00-01.6xjkrgj.mongodb.net:27017,ac-k5cpl72-shard-00-02.6xjkrgj.mongodb.net:27017/?ssl=true&replicaSet=atlas-xyph1r-shard-0&authSource=admin&retryWrites=true&w=majority";

        public static string MongoDbPingTest()
        {

            MongoClientSettings settings = MongoClientSettings.FromConnectionString(_connectionUri);

            MongoClient client = new MongoClient(settings);
            try
            {
                BsonDocument result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                return "Pinged your deployment. You successfully connected to MongoDB!";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public static string MongoDbInsertTest()
        {
            var dbName = "NutriLensDtb";
            var collectionName = "TestCollection";

            IMongoClient client;

            IMongoCollection<FoodItem> collection;

            try
            {
                client = new MongoClient(_connectionUri);
            }
            catch (Exception ex)
            {
                return "There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {ex.Message}";
            }

            collection = client
                .GetDatabase(dbName)
                .GetCollection<FoodItem>(collectionName);

            try
            {
                collection.InsertOne(new FoodItem
                {
                    Name = "Pão de queijo - CEFSA rock version",
                    Portion = "uma unidade"
                });

                return "Inserção realizada com sucesso!";
            }
            catch (Exception e)
            {
                return $"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}";
            }
        }

        public static string MongoDbReadTest()
        {
            var dbName = "NutriLensDtb";
            var collectionName = "TestCollection";

            IMongoClient client;

            IMongoCollection<FoodItem> collection;

            try
            {
                client = new MongoClient(_connectionUri);
            }
            catch (Exception ex)
            {
                return "There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {ex.Message}";
            }

            collection = client
                .GetDatabase(dbName)
                .GetCollection<FoodItem>(collectionName);

            try
            {
                var allDocs = collection.Find(Builders<FoodItem>.Filter.Empty)
                .ToList();

                string returnString = string.Empty;

                foreach (FoodItem foodItem in allDocs)
                {
                    returnString += foodItem.ToString() + Environment.NewLine;
                }

                return returnString;
            }
            catch (Exception e)
            {
                return $"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}";
            }
        }

        public static string UploadImage(string imagePath)
        {
            var dbName = "NutriLensDtb";
            var collectionName = "TestCollection";

            IMongoClient client;

            IMongoCollection<MongoImage> collection;

            try
            {
                client = new MongoClient(_connectionUri);
            }
            catch (Exception ex)
            {
                return "There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {ex.Message}";
            }

            collection = client
                .GetDatabase(dbName)
                .GetCollection<MongoImage>(collectionName);

            try
            {
                collection.InsertOne(new MongoImage
                {
                    FileName = Path.GetFileName(imagePath),
                    ImageBytes = File.ReadAllBytes(imagePath)
                });

                return "Inserção realizada com sucesso!";
            }
            catch (Exception e)
            {
                return $"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}";
            }
        }

        public static string DownloadImages(string downloadDirectory)
        {
            var dbName = "NutriLensDtb";
            var collectionName = "TestCollection";

            IMongoClient client;

            IMongoCollection<MongoImage> collection;

            try
            {
                client = new MongoClient(_connectionUri);
            }
            catch (Exception ex)
            {
                return "There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {ex.Message}";
            }

            collection = client
                .GetDatabase(dbName)
                .GetCollection<MongoImage>(collectionName);

            try
            {
                var allDocs = collection.Find(Builders<MongoImage>.Filter.Empty)
                .ToList();

                string returnString = string.Empty;

                foreach (MongoImage mongoImage in allDocs)
                {
                    if (!string.IsNullOrEmpty(mongoImage.FileName) && !File.Exists(Path.Combine(downloadDirectory, mongoImage.FileName)))
                    {
                        File.WriteAllBytes(Path.Combine(downloadDirectory, mongoImage.FileName), mongoImage.ImageBytes);
                    }
                }

                return returnString;
            }
            catch (Exception e)
            {
                return $"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}";
            }
        }

        public static bool InsertBarCodeItem(BarcodeItem barcodeItem)
        {
            var dbName = "NutriLensDtb";
            var collectionName = "BarCodeProductsCollection";

            IMongoClient client;

            IMongoCollection<BarcodeItem> collection;

            try
            {
                client = new MongoClient(_connectionUri);
            }
            catch (Exception ex)
            {
                ViewServices.PopUpManager.PopErrorAsync("There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {ex.Message}");

                return false;
            }

            collection = client
                .GetDatabase(dbName)
                .GetCollection<BarcodeItem>(collectionName);

            try
            {
                collection.InsertOne(barcodeItem);
                return true;
            }
            catch (Exception e)
            {
                ViewServices.PopUpManager.PopErrorAsync($"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}");
                return false;
            }
        }

        public static bool UpdateBarCodeItem(BarcodeItem barcodeItem)
        {
            var dbName = "NutriLensDtb";
            var collectionName = "BarCodeProductsCollection";

            IMongoClient client;

            IMongoCollection<BarcodeItem> collection;

            try
            {
                client = new MongoClient(_connectionUri);
            }
            catch (Exception ex)
            {
                ViewServices.PopUpManager.PopErrorAsync("There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {ex.Message}");

                return false;
            }

            collection = client
                .GetDatabase(dbName)
                .GetCollection<BarcodeItem>(collectionName);

            try
            {
                collection.ReplaceOne(doc => doc.Barcode == barcodeItem.Barcode, barcodeItem);
                return true;
            }
            catch (Exception e)
            {
                ViewServices.PopUpManager.PopErrorAsync($"Something went wrong trying to update the new documents." +
                    $" Message: {e.Message}");
                return false;
            }
        }

        public static BarcodeItemEntry GetBarCodeItem(string barcode)
        {
            var dbName = "NutriLensDtb";
            var collectionName = "BarCodeProductsCollection";

            IMongoClient client;

            IMongoCollection<BarcodeItemEntry> collection;

            try
            {
                client = new MongoClient(_connectionUri);
            }
            catch (Exception ex)
            {
                ViewServices.PopUpManager.PopErrorAsync("There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {ex.Message}");

                return null;
            }

            collection = client
                .GetDatabase(dbName)
                .GetCollection<BarcodeItemEntry>(collectionName);

            try
            {
                List<BarcodeItemEntry> allDocs = collection.Find(Builders<BarcodeItemEntry>.Filter.Eq(x => x.Barcode, barcode))
                .ToList();

                if (allDocs.Count > 0)
                    return allDocs[0];
                else
                    return null;
            }
            catch (Exception e)
            {
                ViewServices.PopUpManager.PopErrorAsync($"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}");

                return null;
            }
        }

        public static List<BarcodeItem> GetAllBarCodeItems()
        {
            var dbName = "NutriLensDtb";
            var collectionName = "BarCodeProductsCollection";

            IMongoClient client;

            IMongoCollection<BarcodeItem> collection;

            try
            {
                client = new MongoClient(_connectionUri);
            }
            catch (Exception ex)
            {
                ViewServices.PopUpManager.PopErrorAsync("There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {ex.Message}");

                return null;
            }

            collection = client
                .GetDatabase(dbName)
                .GetCollection<BarcodeItem>(collectionName);

            try
            {
                List<BarcodeItem> allDocs = collection.Find(Builders<BarcodeItem>.Filter.Empty).ToList();

                if (allDocs.Count > 0)
                    return allDocs;
                else
                    return null;
            }
            catch (Exception e)
            {
                ViewServices.PopUpManager.PopErrorAsync($"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}");

                return null;
            }
        }

        public static string GetNutritionalInfo(FoodItem foodItem)
        {
            //OpenAiInputModel openAiInputModel = new()
            //{
            //    SystemPrompt = "Be direct. Answer only the nutritional info in json format. Always with units. Consider the average size/type.",
            //    UserPrompt = foodItem.GptQueryString,
            //    Temperature = 1,
            //    TopP = 1,
            //    FrequencyPenalty = 0,
            //    PresencePenalty = 0
            //};

            //OpenAiResponse openAiResponse = OpenAiQuery(OpenAiModel.Gpt3dot5Turbo, openAiInputModel);
            //return openAiResponse.GetResponseMessage();
            return string.Empty;
        }

        public static string GetFoodVisionAnalisysByUrl(string url)
        {
            //OpenAiVisionInputModel inputModel = new OpenAiVisionInputModel
            //{
            //    UserPrompt = "Analise essa foto.",
            //    SystemPrompt = "Você identifica itens alimentícios de uma refeição, trazendo seu nome e a quantidade em gramas ou ml, em itens. Eu sei que as quantidades são aproximadas, portanto, não é necessário escrever aproximadamente ou coisa do tipo. Você é objetivo e não retorna mais nada além do solicitado.",
            //    MaxTokens = 300,
            //    Base64 = false,
            //    Url = url
            //};

            //OpenAiResponse response = OpenAiQuery(OpenAiModel.Gpt4VisionPreview, inputModel);

            //return response.GetResponseMessage();
            return string.Empty;
        }

        public static string GetFoodVisionAnalisysByLocalPath(string localPath)
        {
            //byte[] imageBytes = File.ReadAllBytes(localPath);
            //string base64Image = Convert.ToBase64String(imageBytes);

            //OpenAiVisionInputModel inputModel = new OpenAiVisionInputModel
            //{
            //    UserPrompt = "Analise essa foto.",
            //    SystemPrompt = "Você identifica itens alimentícios de uma refeição, trazendo seu nome e a quantidade em gramas ou ml, em itens. Eu sei que as quantidades são aproximadas, portanto, não é necessário escrever aproximadamente ou coisa do tipo. Você é objetivo e não retorna mais nada além do solicitado.",
            //    MaxTokens = 300,
            //    Base64 = false,
            //    Url = $"data:image/jpeg;base64,{base64Image}"
            //};

            //OpenAiResponse response = OpenAiQuery(OpenAiModel.Gpt4VisionPreview, inputModel);

            //return response.GetResponseMessage();
            return string.Empty;
        }

        public static UserInfo InsertNewLoginModel(Login login)
        {
            var dbName = "NutriLensDtb";
            var collectionName = "LoginCollection";

            IMongoClient client;

            IMongoCollection<Login> collection;

            try
            {
                client = new MongoClient(_connectionUri);
            }
            catch (Exception ex)
            {
                ViewServices.PopUpManager.PopErrorAsync("There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {ex.Message}");

                return null;
            }

            collection = client
                .GetDatabase(dbName)
                .GetCollection<Login>(collectionName);

            List<Login> allDocs;

            try
            {
                var filterBuilder = Builders<Login>.Filter;

                allDocs = collection.Find(filterBuilder.Eq(x => x.Email, login.Email)).ToList();
            }
            catch (Exception e)
            {
                ViewServices.PopUpManager.PopErrorAsync($"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}");

                return null;
            }

            if (allDocs.Count > 0)
                throw new EmailAlreadyRegisteredException("E-mail já registrado! Verifique e tente novamente.");

            try
            {
                collection.InsertOne(login);
                return InsertNewUserInfoByLogin(login);
            }
            catch (Exception e)
            {
                ViewServices.PopUpManager.PopErrorAsync($"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}");
                return null;
            }
        }

        public static UserInfo InsertNewUserInfoByLogin(Login login)
        {
            var dbName = "NutriLensDtb";
            var collectionName = "UserInfoCollection";

            IMongoClient client;

            IMongoCollection<UserInfo> collection;

            try
            {
                client = new MongoClient(_connectionUri);
            }
            catch (Exception ex)
            {
                ViewServices.PopUpManager.PopErrorAsync("There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {ex.Message}");

                return null;
            }

            collection = client
                .GetDatabase(dbName)
                .GetCollection<UserInfo>(collectionName);

            List<UserInfo> allDocs;
            string userInfoIdentifier = login.UserInfoIdentifier;

            try
            {
                var filterBuilder = Builders<UserInfo>.Filter;

                allDocs = collection.Find(filterBuilder.Eq(x => x.Id, userInfoIdentifier)).ToList();
            }
            catch (Exception e)
            {
                ViewServices.PopUpManager.PopErrorAsync($"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}");

                return null;
            }

            if (allDocs.Count > 0)
                throw new EmailAlreadyRegisteredException("Informações de usuário já registradas! Verifique e tente novamente.");

            try
            {
                UserInfo newUserInfo = new() { Id = userInfoIdentifier };
                collection.InsertOne(newUserInfo);
                return newUserInfo;
            }
            catch (Exception e)
            {
                ViewServices.PopUpManager.PopErrorAsync($"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}");
                return null;
            }
        }

        public static UserInfo GetUserInfoByLogin(Login login)
        {
            var dbName = "NutriLensDtb";
            var collectionName = "UserInfoCollection";

            IMongoClient client;

            IMongoCollection<UserInfo> collection;

            try
            {
                client = new MongoClient(_connectionUri);
            }
            catch (Exception ex)
            {
                ViewServices.PopUpManager.PopErrorAsync("There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {ex.Message}");

                return null;
            }

            collection = client
                .GetDatabase(dbName)
                .GetCollection<UserInfo>(collectionName);

            List<UserInfo> allDocs;
            string userInfoIdentifier = login.UserInfoIdentifier;

            try
            {
                var filterBuilder = Builders<UserInfo>.Filter;

                allDocs = collection.Find(filterBuilder.Eq(x => x.Id, userInfoIdentifier)).ToList();
            }
            catch (Exception e)
            {
                ViewServices.PopUpManager.PopErrorAsync($"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}");

                return null;
            }

            if (allDocs.Count > 0)
                return allDocs[0];
            else
                throw new Exception("Informações de usuário não encontradas");
        }

        public static UserInfo CheckUserLogin(Login login)
        {
            var dbName = "NutriLensDtb";
            var collectionName = "LoginCollection";

            IMongoClient client;

            IMongoCollection<Login> collection;

            try
            {
                client = new MongoClient(_connectionUri);
            }
            catch (Exception ex)
            {
                ViewServices.PopUpManager.PopErrorAsync("There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {ex.Message}");

                return null;
            }

            collection = client
                .GetDatabase(dbName)
                .GetCollection<Login>(collectionName);

            try
            {
                var filterBuilder = Builders<Login>.Filter;

                List<Login> allDocs = collection.Find(filterBuilder.Eq(x => x.Email, login.Email) & filterBuilder.Eq(x => x.Password, login.Password))
                .ToList();

                return GetUserInfoByLogin(allDocs[0]);

            }
            catch (Exception e)
            {
                ViewServices.PopUpManager.PopErrorAsync($"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}");

                return null;
            }
        }

        public static bool UpdateUserInfo(UserInfo userInfo)
        {
            var dbName = "NutriLensDtb";
            var collectionName = "UserInfoCollection";

            IMongoClient client;

            IMongoCollection<UserInfo> collection;

            try
            {
                client = new MongoClient(_connectionUri);
            }
            catch (Exception ex)
            {
                ViewServices.PopUpManager.PopErrorAsync("There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {ex.Message}");

                return false;
            }

            collection = client
                .GetDatabase(dbName)
                .GetCollection<UserInfo>(collectionName);

            try
            {
                collection.ReplaceOne(doc => doc.Id == userInfo.Id, userInfo);
                return true;
            }
            catch (Exception e)
            {
                ViewServices.PopUpManager.PopErrorAsync($"Something went wrong trying to update the new documents." +
                    $" Message: {e.Message}");
                return false;
            }
        }

        public static List<TbcaItem> GetTbcaItemsList()
        {
            var dbName = "NutriLensDtb";
            var collectionName = "TbcaFoodItemsCollection";

            IMongoClient client;

            IMongoCollection<TbcaItem> collection;

            try
            {
                client = new MongoClient(_connectionUri);
            }
            catch (Exception ex)
            {
                ViewServices.PopUpManager.PopErrorAsync("There was a problem connecting to your " +
                    "Atlas cluster. Check that the URI includes a valid " +
                    "username and password, and that your IP address is " +
                    $"in the Access List. Message: {ex.Message}");

                return null;
            }

            collection = client
                .GetDatabase(dbName)
                .GetCollection<TbcaItem>(collectionName);

            try
            {
                List<TbcaItem> allDocs = collection.Find(Builders<TbcaItem>.Filter.Empty).ToList();

                if (allDocs.Count > 0)
                    return allDocs;
                else
                    return null;
            }
            catch (Exception e)
            {
                ViewServices.PopUpManager.PopErrorAsync($"Something went wrong trying to insert the new documents." +
                    $" Message: {e.Message}");

                return null;
            }
        }
    }
}
