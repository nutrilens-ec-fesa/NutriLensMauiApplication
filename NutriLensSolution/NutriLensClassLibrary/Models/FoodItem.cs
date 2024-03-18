using MongoDB.Bson;
using Newtonsoft.Json;
using NutriLensClassLibrary.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NutriLensClassLibrary.Models
{
    /// <summary>
    /// Representa um modelo de item alimentício
    /// </summary>
    public class FoodItem
    {
        #region Getters and Setters

        /// <summary>
        /// Id Bson para utilização no MongoDb
        /// </summary>
        public BsonObjectId Id { get; set; }

        /// <summary>
        /// Nome do item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Porção do item
        /// </summary>
        public string Portion { get; set; }

        /// <summary>
        /// Quantidade de kcal do item
        /// </summary>
        public double KiloCalories { get; set; }

        public TbcaItem TbcaFoodItem { get; set; }

        public TacoItem TacoFoodItem { get; set; }


        #endregion

        #region Getters only

        [JsonIgnore]
        public string NamePlusPortion { get => $"{Name}{(string.IsNullOrEmpty(Portion) ? string.Empty : $" - {Portion}")}"; }
        [JsonIgnore]
        public string NamePlusPortionPlusKcalInfo { get => $"{Name}{(string.IsNullOrEmpty(Portion) ? string.Empty : $" - {Portion}")} - {KiloCalorieInfo}"; }
        [JsonIgnore]
        public string NamePlusPortionPlusKjInfo { get => $"{Name}{(string.IsNullOrEmpty(Portion) ? string.Empty : $" - {Portion}")} - {KiloJoulesInfo}"; }
        [JsonIgnore]
        public double KiloJoules { get => KiloCalories * Constants.kcalToKJFactor; }
        [JsonIgnore]
        public string KiloCalorieInfo { get => $"{KiloCalories} {Constants.kcalUnit}"; }
        [JsonIgnore]
        public string KiloJoulesInfo { get => $"{KiloCalories * Constants.kcalToKJFactor} {Constants.kJUnit}"; }
        [JsonIgnore]
        public string GptQueryString { get => $"{Name}{(string.IsNullOrEmpty(Portion) ? string.Empty : $" - {Portion}")}"; }




        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"{Name}{(string.IsNullOrEmpty(Portion) ? string.Empty : $" - {Portion}")} - {KiloCalorieInfo}";
        }

        #endregion
    }
}
