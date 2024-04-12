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
    }
}
