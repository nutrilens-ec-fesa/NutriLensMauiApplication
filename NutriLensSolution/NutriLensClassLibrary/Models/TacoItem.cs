using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Dynamic;

namespace NutriLensClassLibrary.Models
{
    public class TacoItem
    {
        [BsonId, JsonIgnore]
        public ObjectId _id { get; set; }
        public int id { get; set; }
        public string Nome { get; set; }
        [BsonElement("Energia (kJ)")]
        public object? EnergiaKj { get; set; }
        [BsonElement("Energia (kcal)")]
        public object? EnergiaKcal { get; set; }
        [BsonElement("Umidade (%)")]
        public object? Umidade { get; set; }
        [BsonElement("Proteína (g)")]
        public object? Proteina { get; set; }
        [BsonElement("Lipídeos (g)")]
        public object? Lipideos { get; set; }
        [BsonElement("Colesterol (mg)")]
        public object? Colesterol { get; set; }
        [BsonElement("Carboidrato (g)")]
        public object? Carboidrato { get; set; }
        [BsonElement("Fibra Alimentar (g)")]
        public object? FibraAlimentar { get; set; }
        [BsonElement("Cinzas (g)")]
        public object? Cinzas { get; set; }
        [BsonElement("Cálcio (mg)")]
        public object? Calcio { get; set; }
        [BsonElement("Magnésio (mg)")]
        public object? Magnesio { get; set; }
        [BsonElement("Manganês (mg)")]
        public object? Manganes { get; set; }
        [BsonElement("Fósforo (mg)")]
        public object? Fosforo { get; set; }
        [BsonElement("Ferro (mg)")]
        public object? Ferro { get; set; }
        [BsonElement("Sódio (mg)")]
        public object? Sodio { get; set; }
        [BsonElement("Potássio (mg)")]
        public object? Potassio { get; set; }
        [BsonElement("Cobre (mg)")]
        public object? Cobre { get; set; }
        [BsonElement("Zinco (mg)")]
        public object? Zinco { get; set; }
        [BsonElement("Retinol (µg)")]
        public object? Retinol { get; set; }
        [BsonElement("RE (µg)")]
        public object? RE { get; set; }
        [BsonElement("RAE (µg)")]
        public object? RAE { get; set; }
        [BsonElement("Tiamina (mg)")]
        public object? Tiamina { get; set; }
        [BsonElement("Riboflavina (mg)")]
        public object? Riboflavina { get; set; }
        [BsonElement("Piridoxina (mg)")]
        public object? Piridoxina { get; set; }
        [BsonElement("Niacina (mg)")]
        public object? Niacina { get; set; }
        [BsonElement("Vitamina C (mg)")]
        public object? VitaminaC { get; set; }

        public double GetValue(string propertyName)
        {
            object? propValue = GetType().GetProperty(propertyName).GetValue(this, null);

            if (propValue == null)
                return 0;
            if (propValue is string)
                return 0;

            return Convert.ToDouble(propValue);
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
