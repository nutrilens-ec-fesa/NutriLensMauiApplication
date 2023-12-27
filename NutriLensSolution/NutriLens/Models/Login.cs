using MongoDB.Bson.Serialization.Attributes;

namespace NutriLens.Models
{
    public class Login
    {
        [BsonId]
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
