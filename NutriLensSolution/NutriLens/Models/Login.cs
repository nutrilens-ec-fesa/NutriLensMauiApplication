using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static Android.Renderscripts.ScriptGroup;

namespace NutriLens.Models
{
    public class Login
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [JsonIgnore, BsonIgnore]
        public string UserInfoIdentifier
        {
            get
            {
                string userInfoIdentifier = string.Empty;
                int biggerLength = Id.Length > Email.Length ? Id.Length : Email.Length;

                for(int i = 0; i < biggerLength; i++)
                {
                    if(i < Id.Length)
                        userInfoIdentifier += Id[i];
                    
                    if(i < Email.Length)
                        userInfoIdentifier += Email[i];
                }

                char[] charArray = userInfoIdentifier.ToCharArray();
                Array.Reverse(charArray);
                string reversedUserInfoIdentifier = new string(charArray);
                return CryptographyLibrary.CryptographyManager.EncryptData(reversedUserInfoIdentifier);
            }
        }
    }
}
