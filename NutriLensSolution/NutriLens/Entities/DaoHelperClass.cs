using NutriLens.Models;
using static NutriLens.Entities.OpenAiEntity;

namespace NutriLens.Entities
{
    public static class DaoHelperClass
    {
        public static string GetNutritionalInfo(FoodItem foodItem)
        {
            OpenAiInputModel openAiInputModel = new()
            {
                SystemPrompt = "Be direct. Answer only the nutritional info in json format including units. Consider the average size/type.",
                UserPrompt = foodItem.ToString(),
                Temperature = 1,
                TopP = 1,
                FrequencyPenalty = 0,
                PresencePenalty = 0
            };

            OpenAiResponse openAiResponse = OpenAiQuery(OpenAiModel.Gpt3dot5Turbo, openAiInputModel);
            return openAiResponse.GetResponseMessage();
        }
    }
}
