namespace NutriLensWebApp.Models
{
    public class OpenAiInputModel
    {
        public string SystemPrompt { get; set; }
        public string UserPrompt { get; set; }
        public double Temperature { get; set; }
        public int? MaxTokens { get; set; }
        public double? TopP { get; set; }
        public double? FrequencyPenalty { get; set; }
        public double? PresencePenalty { get; set; }
    }
}
