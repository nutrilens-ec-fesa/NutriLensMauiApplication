using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace NutriLensClassLibrary.Models
{
    public class TacoItem
    {
        [BsonId, JsonIgnore]
        public ObjectId _id { get; set; }
        public string Id { get; set; }
        public string Nome { get; set; }
        public int? EnergiaKj { get; set; }
        public int? EnergiaKcal { get; set; }
        public double? Umidade { get; set; }
        public double? Proteina { get; set; }
        public double? Lipideos { get; set; }
        public double? Colesterol { get; set; }
        public double? Carboidrato { get; set; }
        public double? FibraAlimentar { get; set; }
        public double? Cinzas { get; set; }
        public double? Calcio { get; set; }
        public double? Magnesio { get; set; }
        public double? Manganes { get; set; }
        public double? Fosforo { get; set; }
        public double? Ferro { get; set; }
        public double? Sodio { get; set; }
        public double? Potassio { get; set; }
        public double? Cobre { get; set; }
        public double? Zinco { get; set; }
        public double? Retinol { get; set; }
        public double? RE { get; set; }
        public double? RAE { get; set; }
        public double? Tiamina { get; set; }
        public double? Riboflavina { get; set; }
        public double? Piridoxina { get; set; }
        public double? Niacina { get; set; }
        public double? VitaminaC { get; set; }


        public override string ToString()
        {
            return Nome;
        }
    }
}
