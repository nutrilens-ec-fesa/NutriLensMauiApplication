using NutriLensClassLibrary.Models;
using System.Text;

namespace NutriLensClassLibrary.Models
{
    /// <summary>
    /// Representa um modelo de refeição
    /// </summary>
    public class Meal
    {
        /// <summary>
        /// Data e hora da refeição
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Nome da refeição
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Itens alimentícios presentes na refeição
        /// </summary>
        public List<FoodItem> FoodItems { get; set; }

        /// <summary>
        /// Caminho da foto associada a refeição
        /// </summary>
        public string MealPicturePath { get; set; }

        public string DateInfo { get => DateTime.ToShortDateString(); }
        public string TimeInfo { get => DateTime.ToShortTimeString(); }
        public string FoodItemsCountInfo { get => FoodItems.Count.ToString(); }
        public string FoodItemsInfo
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach(FoodItem foodItem in FoodItems)
                {
                    if (foodItem.TacoFoodItem != null)
                        stringBuilder.Append($"   - {foodItem.TacoFoodItem.Nome} ({foodItem.Portion} g - {foodItem.KiloCalorieInfo}){Environment.NewLine}");
                    else
                        stringBuilder.Append($"   - {foodItem.Name} ({foodItem.Portion} g - {foodItem.KiloCalorieInfo}){Environment.NewLine}");
                }

                return stringBuilder.ToString();
            }
        }
        public string TotalKcalInfo
        {
            get => Math.Round(FoodItems.Select(x => x.KiloCalories).Sum(), 2).ToString() + " kcal";
        }
        public string TotalMassInfo
        {
            get => Math.Round(FoodItems.Select(x => double.Parse(x.Portion)).Sum(), 2).ToString() + " g";
        }
        
        public override string ToString()
        {
            if(FoodItems.Count > 1)
                return $"{DateTime} - {Name} - {FoodItems.Count} itens";
            else
                return $"{DateTime} - {Name} - {FoodItems.Count} item";
        }
    }
}
