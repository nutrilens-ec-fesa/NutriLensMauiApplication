using MongoDB.Bson.Serialization.Attributes;
using NutriLensClassLibrary.Models;
using StringLibrary;
using System.Text.Json.Serialization;

namespace NutriLensClassLibrary.Models
{
    public class BarcodeItemEntry : BarcodeItem
    {
        [JsonIgnore, BsonIgnore]
        public string UnitsPerPortionEntry
        {
            get => UnitsPerPortion.ToString("0.00");
            set { UnitsPerPortion = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string TotalCarbohydratesEntry
        {
            get => TotalCarbohydrates.ToString("0.00");
            set { TotalCarbohydrates = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string TotalSugarEntry
        {
            get => TotalSugar.ToString("0.00");
            set { TotalSugar = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string AddedSugarEntry
        {
            get => AddedSugar.ToString("0.00");
            set { AddedSugar = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string ProteinsEntry
        {
            get => Proteins.ToString("0.00");
            set { Proteins = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string TotalFatEntry
        {
            get => TotalFat.ToString("0.00");
            set { TotalFat = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string SaturatedFatEntry
        {
            get => SaturatedFat.ToString("0.00");
            set { SaturatedFat = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string TransFatEntry
        {
            get => TransFat.ToString("0.00");
            set { TransFat = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string DietaryFiberEntry
        {
            get => DietaryFiber.ToString("0.00");
            set { DietaryFiber = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string SodiumEntry
        {
            get => Sodium.ToString("0.00");
            set { Sodium = StringFunctions.ParseDoubleValue(value); }
        }

        /// <summary>
        /// Quantidade a ser consumida
        /// </summary>
        [JsonIgnore, BsonIgnore]
        public double QuantityConsumption { get; set; }

        [JsonIgnore, BsonIgnore]
        public double TotalCaloriesConsumption { get => (QuantityConsumption * EnergeticValue) / UnitsPerPortion; }

        [JsonIgnore, BsonIgnore]
        public double TotalCarbohydratesConsumption { get => (QuantityConsumption * TotalCarbohydrates) / UnitsPerPortion;  }

        [JsonIgnore, BsonIgnore]
        public double TotalSugarConsumption { get => (QuantityConsumption * TotalSugar) / UnitsPerPortion; }

        [JsonIgnore, BsonIgnore]
        public double AddedSugarConsumption { get => (QuantityConsumption * AddedSugar) / UnitsPerPortion; }

        [JsonIgnore, BsonIgnore]
        public double ProteinsConsumption { get => (QuantityConsumption * Proteins) / UnitsPerPortion; }

        [JsonIgnore, BsonIgnore]
        public double TotalFatConsumption { get => (QuantityConsumption * TotalFat) / UnitsPerPortion; }

        [JsonIgnore, BsonIgnore]
        public double SaturatedFatConsumption { get => (QuantityConsumption * SaturatedFat) / UnitsPerPortion; }

        [JsonIgnore, BsonIgnore]
        public double TransFatConsumption { get => (QuantityConsumption * TransFat) / UnitsPerPortion; }

        [JsonIgnore, BsonIgnore]
        public double DietaryFiberConsumption { get => (QuantityConsumption * DietaryFiber) / UnitsPerPortion; }

        [JsonIgnore, BsonIgnore]
        public double SodiumConsumption { get => (QuantityConsumption * Sodium) / UnitsPerPortion; }

        [JsonIgnore, BsonIgnore]
        public string BarCodeEntryInfo
        {
            get
            {
                string barcodeEntryInfo;

                barcodeEntryInfo = ProductName + Environment.NewLine;
                barcodeEntryInfo += $"Qde: {QuantityConsumption} {PortionDefinition} - {TotalCaloriesConsumption} kcal";
                return barcodeEntryInfo;
            }
        }

        public override string ToString()
        {
            return base.ToString() + Environment.NewLine + Environment.NewLine + $"{QuantityConsumption} {PortionDefinition} - {TotalCaloriesConsumption}";
        }
    }
}
