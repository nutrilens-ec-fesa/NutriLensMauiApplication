using MongoDB.Bson;

namespace NutriLensClassLibrary.Models
{
    public class MongoImage
    {
        public BsonObjectId Id { get; set; }
        public string FileName { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}
