namespace NutriLensClassLibrary.Models
{
    public class AiResult
    {
        public string GptResult { get; set; }
        public string GeminiResult { get; set; }

        public AiResult(string gptResult, string geminiResult)
        {
            GptResult = gptResult;
            GeminiResult = geminiResult;
        }
    }
}
