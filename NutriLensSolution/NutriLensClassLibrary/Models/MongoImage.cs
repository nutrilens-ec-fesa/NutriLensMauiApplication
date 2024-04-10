using MongoDB.Bson.Serialization.Attributes;

namespace NutriLensClassLibrary.Models
{
    public class MongoImage
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserIdentifier { get; set; }
        public byte[] ImageBytes { get; set; }
        [Obsolete]
        public string VisionRawResult { get; set; }
        public string HumanResult { get; set; }
        public string FileName { get; set; }
        public int? TotalItems { get; set; }
        [Obsolete]
        public int? TotalOkItems { get; set; }
        public DateTime DateTime { get; set; }
        public string GptRawResult { get; set; }
        public string GeminiRawResult { get; set; }
        public int? GptTotalOkItems { get; set; }
        public int? GeminiTotalOkItems { get; set; }
    }
}
