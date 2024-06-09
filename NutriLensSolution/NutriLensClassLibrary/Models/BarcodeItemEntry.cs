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
            get => Math.Round(UnitsPerPortion, 2).ToString();
            set { UnitsPerPortion = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string TotalCarbohydratesEntry
        {
            get => Math.Round(TotalCarbohydrates, 2).ToString();
            set { TotalCarbohydrates = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string TotalSugarEntry
        {
            get => Math.Round(TotalSugar, 2).ToString();
            set { TotalSugar = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string AddedSugarEntry
        {
            get => Math.Round(AddedSugar, 2).ToString();
            set { AddedSugar = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string ProteinsEntry
        {
            get => Math.Round(Proteins, 2).ToString();
            set { Proteins = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string TotalFatEntry
        {
            get => Math.Round(TotalFat, 2).ToString();
            set { TotalFat = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string SaturatedFatEntry
        {
            get => Math.Round(SaturatedFat, 2).ToString();
            set { SaturatedFat = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string TransFatEntry
        {
            get => Math.Round(TransFat, 2).ToString();
            set { TransFat = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string DietaryFiberEntry
        {
            get => Math.Round(DietaryFiber, 2).ToString();
            set { DietaryFiber = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string SodiumEntry
        {
            get => Math.Round(Sodium, 2).ToString();
            set { Sodium = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string CholesterolEntry
        {
            get => Math.Round(Cholesterol, 2).ToString();
            set { Cholesterol = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string CalciumEntry
        {
            get => Math.Round(Calcium, 2).ToString();
            set { Calcium = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string IronEntry
        {
            get => Math.Round(Iron, 2).ToString();
            set { Iron = StringFunctions.ParseDoubleValue(value); }
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
        public double CholesterolConsumption { get => (QuantityConsumption * Cholesterol) / UnitsPerPortion; }

        [JsonIgnore, BsonIgnore]
        public double CalciumConsumption { get => (QuantityConsumption * Calcium) / UnitsPerPortion; }

        [JsonIgnore, BsonIgnore]
        public double IronConsumption { get => (QuantityConsumption * Iron) / UnitsPerPortion; }

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
