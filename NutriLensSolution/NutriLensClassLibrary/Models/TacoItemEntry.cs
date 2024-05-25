using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using StringLibrary;
using System.ComponentModel;

namespace NutriLensClassLibrary.Models
{
    public class TacoItemEntry : TacoItem, INotifyPropertyChanged
    {
        [JsonIgnore, BsonIgnore]
        public double Portion { get; set; }

        [JsonIgnore, BsonIgnore]
        public string FoodType { get; set; }

        private string _glutenEntry;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonIgnore, BsonIgnore]
        public string GlutenEntry
        {

            // TODO, ainda não foi finalizado
            get
            {
                if (Gluten == null)
                    _glutenEntry = "Não informado";
                else if ((bool)Gluten)
                    _glutenEntry = "Sim";
                else
                    _glutenEntry = "Não";

                OnPropertyChanged(nameof(GlutenEntry));

                return _glutenEntry;
            }
            set
            {
                _glutenEntry = value;
            }
        }

        public string LactoseEntry { get; set; }

        [JsonIgnore, BsonIgnore]
        public string PortionEntry
        {
            get => StringFunctions.GetRoundDoubleString(Portion);
            set { Portion = StringFunctions.ParseDoubleValue(value); }
        }

        [JsonIgnore, BsonIgnore]
        public string EnergiaKcalEntry
        {
            get => StringFunctions.GetRoundDoubleString(EnergiaKcal);
            set { EnergiaKcal = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string UmidadeEntry
        {
            get => StringFunctions.GetRoundDoubleString(Umidade);
            set { Umidade = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string ProteinaEntry
        {
            get => StringFunctions.GetRoundDoubleString(Proteina);
            set { Proteina = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string LipideosEntry
        {
            get => StringFunctions.GetRoundDoubleString(Lipideos);
            set { Lipideos = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string ColesterolEntry
        {
            get => StringFunctions.GetRoundDoubleString(Colesterol);
            set { Colesterol = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string CarboidratoEntry
        {
            get => StringFunctions.GetRoundDoubleString(Carboidrato);
            set { Carboidrato = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string FibraAlimentarEntry
        {
            get => StringFunctions.GetRoundDoubleString(FibraAlimentar);
            set { FibraAlimentar = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string CinzasEntry
        {
            get => StringFunctions.GetRoundDoubleString(Cinzas);
            set { Cinzas = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string CalcioEntry
        {
            get => StringFunctions.GetRoundDoubleString(Calcio);
            set { Calcio = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string MagnesioEntry
        {
            get => StringFunctions.GetRoundDoubleString(Magnesio);
            set { Magnesio = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string ManganesEntry
        {
            get => StringFunctions.GetRoundDoubleString(Manganes);
            set { Manganes = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string FosforoEntry
        {
            get => StringFunctions.GetRoundDoubleString(Fosforo);
            set { Fosforo = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string FerroEntry
        {
            get => StringFunctions.GetRoundDoubleString(Ferro);
            set { Ferro = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string SodioEntry
        {
            get => StringFunctions.GetRoundDoubleString(Sodio);
            set { Sodio = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string PotassioEntry
        {
            get => StringFunctions.GetRoundDoubleString(Potassio);
            set { Potassio = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string CobreEntry
        {
            get => StringFunctions.GetRoundDoubleString(Cobre);
            set { Cobre = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string ZincoEntry
        {
            get => StringFunctions.GetRoundDoubleString(Zinco);
            set { Zinco = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string RetinolEntry
        {
            get => StringFunctions.GetRoundDoubleString(Retinol);
            set { Retinol = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string REEntry
        {
            get => StringFunctions.GetRoundDoubleString(RE);
            set { RE = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string RAEEntry
        {
            get => StringFunctions.GetRoundDoubleString(RAE);
            set { RAE = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string TiaminaEntry
        {
            get => StringFunctions.GetRoundDoubleString(Tiamina);
            set { Tiamina = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string RiboflavinaEntry
        {
            get => StringFunctions.GetRoundDoubleString(Riboflavina);
            set { Riboflavina = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string PiridoxinaEntry
        {
            get => StringFunctions.GetRoundDoubleString(Piridoxina);
            set { Piridoxina = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string NiacinaEntry
        {
            get => StringFunctions.GetRoundDoubleString(Niacina);
            set { Niacina = StringFunctions.ParseDoubleValue(value); }
        }
        [JsonIgnore, BsonIgnore]
        public string VitaminaCEntry
        {
            get => StringFunctions.GetRoundDoubleString(VitaminaC);
            set { VitaminaC = StringFunctions.ParseDoubleValue(value); }
        }
    }
}
