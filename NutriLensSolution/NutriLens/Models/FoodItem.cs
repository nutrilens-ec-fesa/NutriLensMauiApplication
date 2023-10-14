using Amazon.Runtime.SharedInterfaces;
using MongoDB.Bson;
using Newtonsoft.Json;
using NutriLens.Entities;

namespace NutriLens.Models
{
    public class FoodItem
    {
        public BsonObjectId Id { get; set; }
        public string Name { get; set; }
        public string Portion { get; set; }
        public double KiloCalories { get; set; }

        [JsonIgnore]
        public string NamePlusPortion { get => $"{Name} - {Portion}"; }
        [JsonIgnore]
        public string NamePlusPortionPlusKcalInfo { get => $"{Name} - {Portion} - {KiloCalorieInfo}"; }
        [JsonIgnore]
        public double KiloJoules { get => KiloCalories * Constants.kcalToKJFactor; }
        [JsonIgnore]
        public string KiloCalorieInfo { get => $"{KiloCalories} {Constants.kcalUnit}"; }
        [JsonIgnore]
        public string KiloJoulesInfo { get => $"{KiloCalories * Constants.kcalToKJFactor} {Constants.kJUnit}"; }
        [JsonIgnore]
        public string GptQueryString { get => $"{Name}{(string.IsNullOrEmpty(Portion) ? string.Empty : $" - {Portion}")}"; }

        public override string ToString()
        {
            return $"{Name}{(string.IsNullOrEmpty(Portion) ? string.Empty : $" - {Portion}")} - {KiloCalorieInfo}";
        }
    }
}
