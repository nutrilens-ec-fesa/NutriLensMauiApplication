using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace NutriLens.Models
{
    public class TbcaItem
    {
        [BsonId, JsonIgnore]
        public ObjectId _id { get; set; }
        public string Id { get; set; }
        public string Alimento { get; set; }
        public int? EnergiaKj { get; set; }
        public int? EnergiaKcal { get; set; }
        public double? UmidadeG { get; set; }
        public double? CarboidratoTotal { get; set; }
        public double? CarboidratoDisponivel { get; set; }
        public double? Proteina { get; set; }
        public double? Lipidios { get; set; }
        public double? FibraAlimentar { get; set; }
        public double? Alcool { get; set; }
        public double? Cinzas { get; set; }
        public double? Colesterol { get; set; }
        public double? AcidosGraxosSaturados { get; set; }
        public double? AcidosGraxosMonoinsaturados { get; set; }
        public double? AcidosGraxosPolinsaturados { get; set; }
        public double? AcidosGraxosTrans { get; set; }
        public double? Calcio { get; set; }
        public double? Ferro { get; set; }
        public double? Sodio { get; set; }
        public double? Magnesio { get; set; }
        public double? Fosforo { get; set; }
        public double? Potassio { get; set; }
        public double? Zinco { get; set; }
        public double? Cobre { get; set; }
        public double? Selenio { get; set; }
        public double? VitaminaARe { get; set; }
        public double? VitaminaARae { get; set; }
        public double? VitaminaD { get; set; }
        public double? VitaminaE { get; set; }
        public double? Tiamina { get; set; }
        public double? Riboflavina { get; set; }
        public double? Niacina { get; set; }
        public double? VitaminaB6 { get; set; }
        public double? VitaminaB12 { get; set; }
        public double? VitaminaC { get; set; }
        public double? EquivalenteDeFolato { get; set; }
        public double? SalDeAdicao { get; set; }
        public double? AcucarDeAdicao { get; set; }

        public override string ToString()
        {
            return Alimento;
        }
    }
}
