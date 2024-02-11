using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NutriLensClassLibrary.Models
{
    public class MongoImage
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string FileName { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}
