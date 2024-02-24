using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NutriLensClassLibrary.Models
{
    public class OpenAiPrompt
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string SystemPrompt { get; set; }
        public string UserPrompt { get; set; }
        public DateTime DateTime { get; set; }
    }
}
