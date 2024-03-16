using MongoDB.Bson.Serialization.Attributes;
using NutriLensClassLibrary.Models;

namespace NutriLens.Models
{
    /// <summary>
    /// Representa um modelo de refeição
    /// </summary>
    public class Meal
    {
        /// <summary>
        /// Id da refeição no Mongo
        /// </summary>
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Propriedade que define o usuário que criou essa refeição
        /// </summary>
        public string UserIdentifier { get; set; }

        /// <summary>
        /// Data e hora da refeição
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Nome da refeição
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Itens alimentícios presentes na refeição
        /// </summary>
        public List<FoodItem> FoodItems { get; set; }

        public override string ToString()
        {
            if(FoodItems.Count > 1)
                return $"{DateTime} - {Name} - {FoodItems.Count} itens";
            else
                return $"{DateTime} - {Name} - {FoodItems.Count} item";
        }
    }
}
