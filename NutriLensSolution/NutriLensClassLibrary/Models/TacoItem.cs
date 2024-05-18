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
        public int? Category { get; set; }
        public string Nome { get; set; }
        [BsonElement("Energia (kJ)")]
        public double? EnergiaKj { get; set; }
        [BsonElement("Energia (kcal)")]
        public double? EnergiaKcal { get; set; }
        [BsonElement("Umidade (%)")]
        public double? Umidade { get; set; }
        [BsonElement("Proteína (g)")]
        public double? Proteina { get; set; }
        [BsonElement("Lipídeos (g)")]
        public double? Lipideos { get; set; }
        [BsonElement("Colesterol (mg)")]
        public double? Colesterol { get; set; }
        [BsonElement("Carboidrato (g)")]
        public double? Carboidrato { get; set; }
        [BsonElement("Fibra Alimentar (g)")]
        public double? FibraAlimentar { get; set; }
        [BsonElement("Cinzas (g)")]
        public double? Cinzas { get; set; }
        [BsonElement("Cálcio (mg)")]
        public double? Calcio { get; set; }
        [BsonElement("Magnésio (mg)")]
        public double? Magnesio { get; set; }
        [BsonElement("Manganês (mg)")]
        public double? Manganes { get; set; }
        [BsonElement("Fósforo (mg)")]
        public double? Fosforo { get; set; }
        [BsonElement("Ferro (mg)")]
        public double? Ferro { get; set; }
        [BsonElement("Sódio (mg)")]
        public double? Sodio { get; set; }
        [BsonElement("Potássio (mg)")]
        public double? Potassio { get; set; }
        [BsonElement("Cobre (mg)")]
        public double? Cobre { get; set; }
        [BsonElement("Zinco (mg)")]
        public double? Zinco { get; set; }
        [BsonElement("Retinol (µg)")]
        public double? Retinol { get; set; }
        [BsonElement("RE (µg)")]
        public double? RE { get; set; }
        [BsonElement("RAE (µg)")]
        public double? RAE { get; set; }
        [BsonElement("Tiamina (mg)")]
        public double? Tiamina { get; set; }
        [BsonElement("Riboflavina (mg)")]
        public double? Riboflavina { get; set; }
        [BsonElement("Piridoxina (mg)")]
        public double? Piridoxina { get; set; }
        [BsonElement("Niacina (mg)")]
        public double? Niacina { get; set; }
        [BsonElement("Vitamina C (mg)")]
        public double? VitaminaC { get; set; }
        public bool? Liquid { get; set; }
        public bool? TacoOriginal { get; set; }
        public bool? Gluten { get; set; }
        public bool? Lactose { get; set; }

        [BsonIgnore, JsonIgnore]
        public TacoCategory TacoCategory { get => Category != null ? (TacoCategory)Category : TacoCategory.INDEFINIDO; }

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

    public enum TacoCategory
    {
        INDEFINIDO,
        CEREAIS_E_DERIVADOS,
        VERDURAS_HORTALICAS_E_DERIVADOS,
        FRUTAS_E_DERIVADOS,
        GORDURAS_E_OLEOS,
        PESCADOS_E_FRUTOS_DO_MAR,
        CARNES_E_DERIVADOS,
        LEITE_E_DERIVADOS,
        BEBIDAS,
        OVOS_E_DERIVADOS,
        PRODUTOS_ACUCARADOS,
        MISCELANEAS,
        OUTROS_ALIMENTOS_INDUSTRIALIZADOS,
        ALIMENTOS_PREPARADOS,
        LEGUMINOSAS_E_DERIVADOS,
        NOZES_E_SEMENTES
    }
}
