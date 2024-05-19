using CommunityToolkit.Maui.Views;
using NutriLens.Services;
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
            try
            {
                if (_loadingPopup != null)
                    await _loadingPopup.CloseAsync();
            }
            catch
            {

            }
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
            ShowLoading("Realizando análise dos alimentos...");

            AiResult aiResult = null; 

            await Task.Run(() => aiResult = DaoHelperClass.GetFoodVisionAnalisysByImageId(mongoImageId));

            await ViewServices.PopUpManager.PopInfoAsync(aiResult.GptResult);
            await ViewServices.PopUpManager.PopInfoAsync(aiResult.GeminiResult);

            List<SimpleFoodItem> gptSimpleFoodItems = JsonToFoodItemsParser.Parse(aiResult.GptResult);
            List<SimpleFoodItem> geminiSimpleFoodItems = JsonToFoodItemsParser.Parse(aiResult.GeminiResult);

            List<FoodItem> gptFoodItems = null;
            List<FoodItem> geminiFoodItems = null;

            try
            {
                if(gptSimpleFoodItems != null)
                    AppDataHelperClass.GetStringTacoItemsBySimpleFoodItemsV2(gptSimpleFoodItems, out gptFoodItems);
            }
            catch (Exception ex)
            {

            }

            try
            {
                if (geminiSimpleFoodItems != null)
                    AppDataHelperClass.GetStringTacoItemsBySimpleFoodItemsV2(geminiSimpleFoodItems, out geminiFoodItems);
            }
            catch (Exception ex)
            {

            }

            await CloseLoading();

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
            ShowLoading("Identificando os alimentos...");

            AiResult aiResult = null;

            await Task.Run(() => aiResult = DaoHelperClass.GetFoodItemsJsonByMealDescription(mealDescription));

            List<SimpleFoodItem> gptSimpleFoodItems = JsonToFoodItemsParser.Parse(aiResult.GptResult);
            List<SimpleFoodItem> geminiSimpleFoodItems = JsonToFoodItemsParser.Parse(aiResult.GeminiResult);

            List<FoodItem> gptFoodItems = null;
            List<FoodItem> geminiFoodItems = null;

            try
            {
                AppDataHelperClass.GetStringTacoItemsBySimpleFoodItemsV2(gptSimpleFoodItems, out gptFoodItems);
            }
            catch (Exception ex)
            {

            }

            try
            {
                AppDataHelperClass.GetStringTacoItemsBySimpleFoodItemsV2(geminiSimpleFoodItems, out geminiFoodItems);
            }
            catch (Exception ex)
            {

            }

            await CloseLoading();

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
