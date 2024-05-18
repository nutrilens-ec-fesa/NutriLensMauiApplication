using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using NutriLensClassLibrary.Models;
using System.Text;
using Newtonsoft.Json;

namespace NutriLensClassLibrary.Models
{
    /// <summary>
    /// Representa um modelo de refeição
    /// </summary>
    public class Meal
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Id do usuário da refeição
        /// </summary>
        public string UserInfoId { get; set; }

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

        [JsonIgnore, BsonIgnore]
        public string DateInfo { get => DateTime.ToShortDateString(); }
        [JsonIgnore, BsonIgnore]
        public string TimeInfo { get => DateTime.ToShortTimeString(); }
        [JsonIgnore, BsonIgnore]
        public string FoodItemsCountInfo { get => FoodItems.Count.ToString(); }
        [JsonIgnore, BsonIgnore]
        public string FoodItemsInfo
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach(FoodItem foodItem in FoodItems)
                {
                    if (foodItem.TacoFoodItem != null)
                        stringBuilder.Append($"   - {foodItem.TacoFoodItem.Nome} ({foodItem.Portion} {foodItem.PortionUnit} - {foodItem.KiloCalorieInfo}){Environment.NewLine}");
                    else
                        stringBuilder.Append($"   - {foodItem.Name} ({foodItem.Portion} {foodItem.PortionUnit} - {foodItem.KiloCalorieInfo}){Environment.NewLine}");
                }

                return stringBuilder.ToString();
            }
        }
        [JsonIgnore, BsonIgnore]
        public string TotalKcalInfo
        {
            get => Math.Round(FoodItems.Select(x => x.KiloCalories).Sum(), 2).ToString() + " kcal";
        }
        [JsonIgnore, BsonIgnore]
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
