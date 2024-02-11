using Newtonsoft.Json;

namespace NutriLens.Models
{
    public class RecognizedImageInfoModel
    {
        [JsonProperty("Item")]
        public string Item { get; set; }

        [JsonProperty("Quantidade")]
        public string Quantidade { get; set; }

    }
}
