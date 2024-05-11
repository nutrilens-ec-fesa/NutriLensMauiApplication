using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace NutriLensClassLibrary.Models
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

        /// <summary>
        /// Total de açucar
        /// </summary>
        public double TotalSugar { get; set; }

        /// <summary>
        /// Açucar adicionado
        /// </summary>
        public double AddedSugar { get; set; }

        /// <summary>
        /// Proteinas
        /// </summary>
        public double Proteins { get; set; }

        /// <summary>
        /// Gordura total
        /// </summary>
        public double TotalFat { get; set; }

        /// <summary>
        /// Gordura saturada
        /// </summary>
        public double SaturatedFat { get; set; }

        /// <summary>
        /// Gordura trans
        /// </summary>
        public double TransFat { get; set; }

        /// <summary>
        /// Fibra alimentar
        /// </summary>
        public double DietaryFiber { get; set; }

        /// <summary>
        /// Sódio
        /// </summary>
        public double Sodium { get; set; }

        public double Cholesterol { get; set; }

        public double Calcium { get; set; }

        public double Iron { get; set; }
    }
}
