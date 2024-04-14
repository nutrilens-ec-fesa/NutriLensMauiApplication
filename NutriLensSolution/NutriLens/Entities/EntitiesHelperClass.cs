using CommunityToolkit.Maui.Views;
using NutriLens.Views.Popups;
using NutriLensClassLibrary.Entities;
using NutriLensClassLibrary.Models;

namespace NutriLens.Entities
{
    public static class EntitiesHelperClass
    {
        private static LoadingPopup _loadingPopup;
        public static string[] AppFiles { get => Directory.GetFiles(FileSystem.AppDataDirectory); }
        public static int PicturesCount { get => GetAllPictures().Length; }
        public static bool HasTempPicture { get => !string.IsNullOrEmpty(GetTempPicture()); }
        public static string[] GetAllPictures()
        {
            return AppFiles.Where(x => x.EndsWith(".png") && !x.Contains("Temp")).ToArray();
        }
        public static string GetTempPicture()
        {
            return AppFiles.FirstOrDefault(x => x.EndsWith(".png") && x.Contains("Temp"));
        }
        public static void DeleteTempPictures()
        {
            string[] localTempPictures = AppFiles.Where(x => x.EndsWith(".png") && x.Contains("Temp")).ToArray();

            foreach (string localPicture in localTempPictures)
            {
                File.Delete(localPicture);
            }
        }
        public static async Task ShowLoading(string message)
        {
            _loadingPopup = new LoadingPopup(message);
            await Application.Current.MainPage.ShowPopupAsync(_loadingPopup);
        }
        public static async Task CloseLoading()
        {
            if (_loadingPopup != null)
                await _loadingPopup.CloseAsync();
        }
        
        /// <summary>
        /// Retorna um double do gasto energético total das atividades físicas da lista
        /// </summary>
        /// <returns></returns>
        public static double TotalEnergeticConsumption(List<PhysicalActivity> physicalActivities)
        {
            double totalCalories;

            try
            {
                totalCalories = physicalActivities
                    .Sum(physicalActivity => physicalActivity.Calories);
            }
            catch
            {
                totalCalories = 0;
            }

            return AppConfigHelperClass.EnergeticUnit switch
            {
                EnergeticUnit.kcal => totalCalories,
                EnergeticUnit.kJ => totalCalories * Constants.kcalToKJFactor,
                _ => default
            };
        }

        public static async Task<List<FoodItem>> GetAiAnalysisByMongoImageId(string mongoImageId)
        {
            EntitiesHelperClass.ShowLoading("Realizando análise dos alimentos...");

            string gptResult = string.Empty;
            string geminiResult = string.Empty;

            #region Analise usando modelos

            TaskCompletionSource<bool> tcsGpt = new TaskCompletionSource<bool>();

            Task.Run(() =>
            {
                try
                {
                    gptResult = DaoHelperClass.GetOpenAiFoodVisionAnalisysByImageId(mongoImageId);
                    tcsGpt.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcsGpt.SetResult(false);
                }

            });

            TaskCompletionSource<bool> tcsGemini = new TaskCompletionSource<bool>();

            Task.Run(() =>
            {
                try
                {
                    geminiResult = DaoHelperClass.GetGeminiAiFoodVisionAnalisysByImageId(mongoImageId);
                    tcsGemini.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcsGemini.SetResult(false);
                }
            });

            // Aguarda as consultas terminarem
            do
            {
                await Task.Delay(1000);

                Console.WriteLine("GPT: " + tcsGpt.Task.IsCompleted);
                Console.WriteLine("Gemini: " + tcsGemini.Task.IsCompleted);

            } while (!tcsGpt.Task.IsCompleted || !tcsGemini.Task.IsCompleted);

            #endregion

            List<SimpleFoodItem> gptSimpleFoodItems = JsonToFoodItemsParser.Parse(gptResult);
            List<SimpleFoodItem> geminiSimpleFoodItems = JsonToFoodItemsParser.Parse(geminiResult);

            List<FoodItem> gptFoodItems = null;
            List<FoodItem> geminiFoodItems = null;

            try
            {
                AppDataHelperClass.GetStringTacoItemsBySimpleFoodItems(gptSimpleFoodItems, out gptFoodItems);
            }
            catch (Exception ex)
            {
                //await EntitiesHelperClass.CloseLoading();
                //await ViewServices.PopUpManager.PopErrorAsync($"Houve algum problema com a análise da foto dos alimentos. {Environment.NewLine} Retorno da API: {gptResult}. {Environment.NewLine}" + ExceptionManager.ExceptionMessage(ex));
                //return;
            }

            try
            {
                AppDataHelperClass.GetStringTacoItemsBySimpleFoodItems(geminiSimpleFoodItems, out geminiFoodItems);
            }
            catch (Exception ex)
            {

            }

            await EntitiesHelperClass.CloseLoading();

            bool gptOk = gptFoodItems != null && gptFoodItems.Count > 0;
            bool geminiOk = geminiFoodItems != null && geminiFoodItems.Count > 0;

            Console.WriteLine("ChatGPT: " + (gptOk ? "OK" : "Falha"));
            Console.WriteLine("Gemini: " + (geminiOk ? "OK" : "Falha"));

            if (gptOk)
                return gptFoodItems;
            else if (geminiOk)
                return geminiFoodItems;
            else
                return null;
        }

        public static async Task<List<FoodItem>> GetAiAnalysisByMealDescription(string mealDescription)
        {
            EntitiesHelperClass.ShowLoading("Identificando os alimentos...");

            string gptResult = string.Empty;
            string geminiResult = string.Empty;

            #region Analise usando modelos

            TaskCompletionSource<bool> tcsGpt = new TaskCompletionSource<bool>();

            Task.Run(() =>
            {
                try
                {
                    gptResult = DaoHelperClass.GetOpenAiFoodItemsJsonByMealDescription(mealDescription);
                    tcsGpt.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcsGpt.SetResult(false);
                }

            });

            TaskCompletionSource<bool> tcsGemini = new TaskCompletionSource<bool>();

            Task.Run(() =>
            {
                try
                {
                    geminiResult = DaoHelperClass.GetGeminiFoodItemsJsonByMealDescription(mealDescription);
                    tcsGemini.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcsGemini.SetResult(false);
                }
            });

            // Aguarda as consultas terminarem
            do
            {
                await Task.Delay(1000);

                Console.WriteLine("GPT: " + tcsGpt.Task.IsCompleted);
                Console.WriteLine("Gemini: " + tcsGemini.Task.IsCompleted);

            } while (!tcsGpt.Task.IsCompleted || !tcsGemini.Task.IsCompleted);

            #endregion

            List<SimpleFoodItem> gptSimpleFoodItems = JsonToFoodItemsParser.Parse(gptResult);
            List<SimpleFoodItem> geminiSimpleFoodItems = JsonToFoodItemsParser.Parse(geminiResult);

            List<FoodItem> gptFoodItems = null;
            List<FoodItem> geminiFoodItems = null;

            try
            {
                AppDataHelperClass.GetStringTacoItemsBySimpleFoodItems(gptSimpleFoodItems, out gptFoodItems);
            }
            catch (Exception ex)
            {
                //await EntitiesHelperClass.CloseLoading();
                //await ViewServices.PopUpManager.PopErrorAsync($"Houve algum problema com a análise da foto dos alimentos. {Environment.NewLine} Retorno da API: {gptResult}. {Environment.NewLine}" + ExceptionManager.ExceptionMessage(ex));
                //return;
            }

            try
            {
                AppDataHelperClass.GetStringTacoItemsBySimpleFoodItems(geminiSimpleFoodItems, out geminiFoodItems);
            }
            catch (Exception ex)
            {

            }

            await EntitiesHelperClass.CloseLoading();

            bool gptOk = gptFoodItems != null && gptFoodItems.Count > 0;
            bool geminiOk = geminiFoodItems != null && geminiFoodItems.Count > 0;

            Console.WriteLine("ChatGPT: " + (gptOk ? "OK" : "Falha"));
            Console.WriteLine("Gemini: " + (geminiOk ? "OK" : "Falha"));

            if (gptOk)
                return gptFoodItems;
            else if (geminiOk)
                return geminiFoodItems;
            else
                return null;
        }
    }
}
