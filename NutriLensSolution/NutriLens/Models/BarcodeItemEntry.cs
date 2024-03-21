using MongoDB.Bson.Serialization.Attributes;
using NutriLens.Entities;
using NutriLensClassLibrary.Models;
using System.Text.Json.Serialization;

namespace NutriLens.Models
{
    public class BarcodeItemEntry : BarcodeItem
    {
        [JsonIgnore, BsonIgnore]
        public string UnitsPerPortionEntry
        {
            get => UnitsPerPortion.ToString("0.00");
            set { UnitsPerPortion = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string TotalCarbohydratesEntry
        {
            get => TotalCarbohydrates.ToString("0.00");
            set { TotalCarbohydrates = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string TotalSugarEntry
        {
            get => TotalSugar.ToString("0.00");
            set { TotalSugar = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string AddedSugarEntry
        {
            get => AddedSugar.ToString("0.00");
            set { AddedSugar = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string ProteinsEntry
        {
            get => Proteins.ToString("0.00");
            set { Proteins = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string TotalFatEntry
        {
            get => TotalFat.ToString("0.00");
            set { TotalFat = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string SaturatedFatEntry
        {
            get => SaturatedFat.ToString("0.00");
            set { SaturatedFat = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string TransFatEntry
        {
            get => TransFat.ToString("0.00");
            set { TransFat = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string DietaryFiberEntry
        {
            get => DietaryFiber.ToString("0.00");
            set { DietaryFiber = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string SodiumEntry
        {
            get => Sodium.ToString("0.00");
            set { Sodium = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        /// <summary>
        /// Quantidade a ser consumida
        /// </summary>
        [JsonIgnore, BsonIgnore]
        public double QuantityConsumption { get; set; }

        [JsonIgnore, BsonIgnore]
        public double TotalCaloriesConsumption { get => (QuantityConsumption * EnergeticValue) / UnitsPerPortion; }

        public override string ToString()
        {
            return base.ToString() + Environment.NewLine + Environment.NewLine + $"{QuantityConsumption} {PortionDefinition} - {TotalCaloriesConsumption}";
        }
    }
}
