using GenerativeAI.Models;
using GenerativeAI.Types;
using NutriLensClassLibrary.Models;

namespace NutriLensWebApp.Entities
{
    public static class GeminiAiEntity
    {
        private static string _apiKey;

        public static void SetApiKey(string apiKey)
        {
            _apiKey = apiKey;
        }

        public static async Task<string> GeminiAiQuery(string prompt, byte[] imageBytes)
        {
            // Implementação do Gemini
            Part imagePart = new Part()
            {
                InlineData = new GenerativeContentBlob()
                {
                    MimeType = "image/png",
                    Data = Convert.ToBase64String(imageBytes)
                }
            };

            Part textPart = new Part()
            {
                Text = prompt
            };

            Part[] parts = new[] { textPart, imagePart };

            string apiKey = _apiKey;

            GeminiProVision visionModel = new GeminiProVision(apiKey);


            EnhancedGenerateContentResponse result = await visionModel.GenerateContentAsync(parts);
            return result.Text();
        }

        public static async Task<string> GeminiAiQuery(string prompt)
        {
            //// Implementação do Gemini
            //Part imagePart = new Part()
            //{
            //    InlineData = new GenerativeContentBlob()
            //    {
            //        MimeType = "text",
            //        Data = prompt
            //    }
            //};

            Part textPart = new Part()
            {
                Text = prompt
            };

            Part[] parts = new[] { textPart };

            string apiKey = _apiKey;

            GeminiProModel model = new GeminiProModel(apiKey);

            EnhancedGenerateContentResponse result = await model.GenerateContentAsync(parts);

            return result.Text();
        }
    }
}
