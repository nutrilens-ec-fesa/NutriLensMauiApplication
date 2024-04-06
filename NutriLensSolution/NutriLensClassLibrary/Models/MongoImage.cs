using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NutriLensClassLibrary.Models
{
    public class MongoImage
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserIdentifier { get; set; }
        public byte[] ImageBytes { get; set; }
        public string VisionRawResult { get; set; }
        public string HumanResult { get; set; }
        public string FileName { get; set; }
        public int? TotalItems { get; set; }
        public int? TotalOkItems { get; set; }
        public DateTime DateTime { get; set; }
    }
}
