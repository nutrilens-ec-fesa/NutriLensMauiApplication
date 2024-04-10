using Newtonsoft.Json;

namespace NutriLensWebApp.Models
{
    public class GeminiVisionInputModel
    {
        [JsonProperty("prompt")]
        public string Prompt { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
