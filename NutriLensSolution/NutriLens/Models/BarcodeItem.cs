using MongoDB.Bson.Serialization.Attributes;
using NutriLens.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NutriLens.Models
{
    public class BarcodeItem
    {
        /// <summary>
        /// Código de barras do produts
        /// </summary>
        [BsonId]
        public string Barcode { get; set; }

        /// <summary>
        /// Nome do produto
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Quantas unidades são a base para a porção (1 fatia, 2 biscoitos)
        /// </summary>
        public double UnitsPerPortion { get; set; }

        [JsonIgnore, BsonIgnore]
        public string UnitsPerPortionEntry
        {
            get => UnitsPerPortion.ToString("0.00");
            set { UnitsPerPortion = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        /// <summary>
        /// Porção base, em gramas (g) ou mililitros (ml)
        /// </summary>
        public double BasePortion { get; set; }

        /// <summary>
        /// Nome do tipo de porção (biscoitos, fatias)
        /// </summary>
        public string PortionDefinition { get; set; }

        /// <summary>
        /// Valor energético em kcal
        /// </summary>
        public double EnergeticValue { get; set; }

        /// <summary>
        /// Total de carboidratos
        /// </summary>
        public double TotalCarbohydrates { get; set; }

        [JsonIgnore, BsonIgnore]
        public string TotalCarbohydratesEntry
        {
            get => TotalCarbohydrates.ToString("0.00");
            set { TotalCarbohydrates = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        /// <summary>
        /// Total de açucar
        /// </summary>
        public double TotalSugar { get; set; }

        [JsonIgnore, BsonIgnore]
        public string TotalSugarEntry
        {
            get => TotalSugar.ToString("0.00");
            set { TotalSugar = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        /// <summary>
        /// Açucar adicionado
        /// </summary>
        public double AddedSugar { get; set; }

        [JsonIgnore, BsonIgnore]
        public string AddedSugarEntry
        {
            get => AddedSugar.ToString("0.00");
            set { AddedSugar = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        /// <summary>
        /// Proteinas
        /// </summary>
        public double Proteins { get; set; }

        [JsonIgnore, BsonIgnore]
        public string ProteinsEntry
        {
            get => Proteins.ToString("0.00");
            set { Proteins = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        /// <summary>
        /// Gordura total
        /// </summary>
        public double TotalFat { get; set; }

        [JsonIgnore, BsonIgnore]
        public string TotalFatEntry
        {
            get => TotalFat.ToString("0.00");
            set { TotalFat = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        /// <summary>
        /// Gordura saturada
        /// </summary>
        public double SaturatedFat { get; set; }

        [JsonIgnore, BsonIgnore]
        public string SaturatedFatEntry
        {
            get => SaturatedFat.ToString("0.00");
            set { SaturatedFat = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        /// <summary>
        /// Gordura trans
        /// </summary>
        public double TransFat { get; set; }

        [JsonIgnore, BsonIgnore]
        public string TransFatEntry
        {
            get => TransFat.ToString("0.00");
            set { TransFat = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        /// <summary>
        /// Fibra alimentar
        /// </summary>
        public double DietaryFiber { get; set; }

        [JsonIgnore, BsonIgnore]
        public string DietaryFiberEntry
        {
            get => DietaryFiber.ToString("0.00");
            set { DietaryFiber = EntitiesHelperClass.ParseDoubleValue(value); }
        }

        /// <summary>
        /// Sódio
        /// </summary>
        public double Sodium { get; set; }

        [JsonIgnore, BsonIgnore]
        public string SodiumEntry
        {
            get => Sodium.ToString("0.00");
            set { Sodium = EntitiesHelperClass.ParseDoubleValue(value); }
        }
    }
}
