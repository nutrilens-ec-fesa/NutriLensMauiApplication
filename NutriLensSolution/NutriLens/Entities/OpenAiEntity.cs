using Newtonsoft.Json;
using NutriLens.Models;
using WebLibrary;
using WebLibrary.HttpRequests;

namespace NutriLens.Entities
{

    public static class OpenAiEntity
    {
        private static readonly string _routeApiChatCompletions = "https://api.openai.com/v1/chat/completions";
        private static readonly string _apiKey = coletar no excel do NutriLens;
        private static readonly string _gpt3dot5Turbo = "gpt-3.5-turbo";

        public enum OpenAiModel
        {
            Gpt3dot5Turbo = 1
        }

        public class Message
        {
            [JsonProperty("role")]
            public string Role { get; set; }
            [JsonProperty("content")]
            public string Content { get; set; }
        }

        public class Choice
        {
            [JsonProperty("index")]
            public int Index { get; set; }
            [JsonProperty("message")]
            public Message Message { get; set; }
            [JsonProperty("finish_reason")]
            public string FinishReason { get; set; }
        }

        public class Usage
        {
            [JsonProperty("prompt_tokens")]
            public int PromptTokens { get; set; }
            [JsonProperty("completion_tokens")]
            public int CompletionTokens { get; set; }
            [JsonProperty("total_tokens")]
            public int TotalTokens { get; set; }
        }

        public class Request
        {
            [JsonProperty("model")]
            public string Model { get; set; }
            [JsonProperty("messages")]
            public List<Message> Messages { get; set; }
            [JsonProperty("temperature")]
            public double Temperature { get; set; }
            [JsonProperty("max_tokens")]
            public int? MaxTokens { get; set; }
            [JsonProperty("top_p")]
            public double? TopP { get; set; }
            [JsonProperty("frequency_penalty")]
            public double? FrequencyPenalty { get; set; }
            [JsonProperty("presence_penalty")]
            public double? PresencePenalty { get; set; }
        }

        public class OpenAiResponse
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("object")]
            public string Object { get; set; }
            [JsonProperty("created")]
            public long Created { get; set; }
            [JsonProperty("model")]
            public string Model { get; set; }
            [JsonProperty("choices")]
            public List<Choice> Choices { get; set; }
            [JsonProperty("usage")]
            public Usage Usage { get; set; }

            public string GetResponseMessage()
            {
                return Choices.First().Message.Content;
            }
        }

        private static string GetOpenAiModelKey(OpenAiModel openAiModel)
        {
            switch (openAiModel)
            {
                case OpenAiModel.Gpt3dot5Turbo:
                    return _gpt3dot5Turbo;
                default:
                    throw new NotImplementedException();
            }
        }

        public static OpenAiResponse OpenAiQuery(OpenAiModel openAiModel, double temperature, string input)
        {
            Request request = new Request
            {
                Model = GetOpenAiModelKey(openAiModel),
                Messages = new List<Message>() { new Message() { Role = "user", Content = input } },
                Temperature = temperature
            };

            return OpenAiRequest(request);
        }

        public static OpenAiResponse OpenAiQuery(OpenAiModel openAiModel, double temperature, string input, string systemInstructions)
        {
            Request request = new Request
            {
                Model = GetOpenAiModelKey(openAiModel),
                Messages = new List<Message>() { new Message() { Role = "user", Content = input }, new Message() { Role = "system", Content = systemInstructions } },
                Temperature = temperature
            };

            return OpenAiRequest(request);
        }

        public static OpenAiResponse OpenAiQuery(OpenAiModel openAiModel, OpenAiInputModel inputModel)
        {
            Request request = new Request
            {
                Model = GetOpenAiModelKey(openAiModel),
                Messages = new List<Message>() { new Message() { Role = "user", Content = inputModel.UserPrompt } },// new Message() { Role = "system", Content = systemInstructions } },
                Temperature = inputModel.Temperature,
                MaxTokens = inputModel.MaxTokens,
                TopP = inputModel.TopP,
                FrequencyPenalty = inputModel.FrequencyPenalty,
                PresencePenalty = inputModel.PresencePenalty
            };

            if (!string.IsNullOrEmpty(inputModel.SystemPrompt))
                request.Messages.Insert(0, new Message() { Role = "system", Content = inputModel.SystemPrompt });


            return OpenAiRequest(request);
        }

        private static OpenAiResponse OpenAiRequest(Request request)
        {
            PostRequest httpRequest = new(_routeApiChatCompletions, string.Empty)
            {
                Body = request,
                Token = _apiKey
            };

            HttpResponseMessage resp;
            string content;

            try
            {
                resp = HttpManager.Request(httpRequest, out content);
            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Houve algum problema na consulta OpenAI", ex);
            }

            try
            {
                if (resp.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<OpenAiResponse>(content);
                else
                    throw new HttpRequestException("Falha na requisição OpenAI: " + content);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Houve algum problema na conversão do objeto de retorno da OpenAI", ex);
            }
        }
    }
}
