using NutriLensClassLibrary.Models;

namespace NutriLensWebApp.Interfaces
{
    public interface IOpenAiPrompt
    {
        public OpenAiPrompt GetLast();
        public void InsertNew(OpenAiPrompt openAiPrompt);
    }
}
