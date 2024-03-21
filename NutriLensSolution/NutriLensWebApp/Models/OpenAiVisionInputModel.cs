using Newtonsoft.Json;

namespace NutriLensWebApp.Models
{
    public class OpenAiVisionInputModel
    {
        [JsonProperty("systemPrompt")]
        public string SystemPrompt { get; set; }
        [JsonProperty("userPrompt")]
        public string UserPrompt { get; set; }
        [JsonProperty("maxTokens")]
        public int? MaxTokens { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("base64")]
        public bool Base64 { get; set; }
    }
}
