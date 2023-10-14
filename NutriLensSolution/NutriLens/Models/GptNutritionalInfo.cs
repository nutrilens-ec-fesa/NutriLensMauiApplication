using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NutriLens.Models
{
    public partial class GptNutritionalInfo
    {
        [JsonProperty("calories")]
        public string CaloriesRaw { get; set; }

        [JsonProperty("carbohydrates")]
        public string CarbohydratesRaw { get; set; }

        [JsonProperty("cholesterol")]
        public string CholesterolRaw { get; set; }

        [JsonProperty("protein")]
        public string ProteinRaw { get; set; }

        [JsonProperty("fat")]
        public string FatRaw { get; set; }

        [JsonProperty("saturated_fat")]
        public string SaturatedFatRaw { get; set; }

        [JsonProperty("sodium")]
        public string SodiumRaw { get; set; }

        [JsonProperty("sugar")]
        public string SugarRaw { get; set; }

        [JsonProperty("total_fat")]
        public string TotalFatRaw { get; set; }

        [JsonProperty("fiber")]
        public string FiberRaw { get; set; }

        public double CaloriesValue { get => double.Parse(ValueRegex().Match(CaloriesRaw).Value); }

        [GeneratedRegex("[\\d]*([\\.,][\\d]+)?")]
        private static partial Regex ValueRegex();
    }
}
