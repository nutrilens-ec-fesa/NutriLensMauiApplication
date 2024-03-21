using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace NutriLensClassLibrary.Models
{
    public class OpenAiPrompt
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [JsonProperty("systemPrompt")]
        public string SystemPrompt { get; set; }
        [JsonProperty("userPrompt")]
        public string UserPrompt { get; set; }
        [JsonProperty("dateTime")]
        public DateTime DateTime { get; set; }
    }
}
