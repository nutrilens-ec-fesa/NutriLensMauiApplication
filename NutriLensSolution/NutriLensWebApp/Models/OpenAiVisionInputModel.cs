namespace NutriLensWebApp.Models
{
    public class OpenAiVisionInputModel
    {
        public string SystemPrompt { get; set; }
        public string UserPrompt { get; set; }
        public int? MaxTokens { get; set; }
        public string Url { get; set; }
        public bool Base64 { get; set; }
    }
}
