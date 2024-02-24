using DnsClient;
using ExceptionLibrary;
using MongoDB.Driver;
using NutriLensClassLibrary.Models;
using NutriLensWebApp.Entities;
using NutriLensWebApp.Interfaces;

namespace NutriLensWebApp.Repositories
{
    public class OpenAiPromptRepository : IOpenAiPrompt
    {
        public OpenAiPrompt GetLast()
        {
            try
            {
                FilterDefinitionBuilder<OpenAiPrompt> filterBuilder = Builders<OpenAiPrompt>.Filter;

                return AppMongoDbContext.OpenAiPrompt
                    .Find(filterBuilder.Empty)
                    .SortByDescending(x => x.DateTime)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para obter o último prompt da OpenAi", ex);
            }
        }

        public void InsertNew(OpenAiPrompt openAiPrompt)
        {
            openAiPrompt.DateTime = DateTime.Now;

            try
            {
                AppMongoDbContext.OpenAiPrompt.InsertOne(openAiPrompt);
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para inserir o novo registro de prompt OpenAi", ex);
            }
        }
    }
}
