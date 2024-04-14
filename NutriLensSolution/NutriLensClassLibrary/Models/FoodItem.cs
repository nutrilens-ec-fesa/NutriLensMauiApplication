using MongoDB.Bson;
using Newtonsoft.Json;
using NutriLensClassLibrary.Entities;
using System.ComponentModel;

namespace NutriLensClassLibrary.Models
{
    /// <summary>
    /// Representa um modelo de item alimentício
    /// </summary>
    public class FoodItem : INotifyPropertyChanged
    {
        #region Attibutes

        private string _name;
        private string _portion;
        private double _kiloCalories;

        #endregion

        #region Getters and Setters

        /// <summary>
        /// Id Bson para utilização no MongoDb
        /// </summary>
        public BsonObjectId Id { get; set; }

        /// <summary>
        /// Nome do item
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }


        /// <summary>
        /// Porção do item
        /// </summary>
        public string Portion
        {
            get => _portion;
            set
            {
                if (_portion != value)
                {
                    _portion = value;
                    OnPropertyChanged(nameof(Portion));
                    OnPropertyChanged(nameof(PortionInfo));
                    OnPropertyChanged(nameof(KiloCalorieInfo));
                }
            }
        }

        /// <summary>
        /// Quantidade de kcal do item
        /// </summary>
        public double KiloCalories
        {
            get => _kiloCalories;
            set
            {
                if (_kiloCalories != value)
                {
                    _kiloCalories = value;
                    OnPropertyChanged(nameof(KiloCalories));
                    OnPropertyChanged(nameof(PortionInfo));
                    OnPropertyChanged(nameof(KiloCalorieInfo));
                }
            }
        }

        public TbcaItem TbcaFoodItem { get; set; }

        public TacoItem TacoFoodItem { get; set; }

        public BarcodeItemEntry BarcodeItemEntry { get; set; } 

        #endregion

        #region Getters only

        [JsonIgnore]
        public string NamePlusPortion { get => $"{Name}{(string.IsNullOrEmpty(Portion) ? string.Empty : $" - {Portion}g")}"; }
        [JsonIgnore]
        public string NamePlusPortionPlusKcalInfo { get => $"{Name}{(string.IsNullOrEmpty(Portion) ? string.Empty : $" - {Portion}g")} - {KiloCalorieInfo}"; }
        [JsonIgnore]
        public string NamePlusPortionPlusKjInfo { get => $"{Name}{(string.IsNullOrEmpty(Portion) ? string.Empty : $" - {Portion}g")} - {KiloJoulesInfo}"; }
        [JsonIgnore]
        public double KiloJoules { get => KiloCalories * Constants.kcalToKJFactor; }
        [JsonIgnore]
        public string KiloCalorieInfo { get => $"{KiloCalories} {Constants.kcalUnit}"; }
        [JsonIgnore]
        public string KiloJoulesInfo { get => $"{KiloCalories * Constants.kcalToKJFactor} {Constants.kJUnit}"; }
        [JsonIgnore]
        public string GptQueryString { get => $"{Name}{(string.IsNullOrEmpty(Portion) ? string.Empty : $" - {Portion}")}"; }
        [JsonIgnore]
        public string PortionInfo { get => Portion + 'g'; }



        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"{Name}{(string.IsNullOrEmpty(Portion) ? string.Empty : $" - {Portion}")} - {KiloCalorieInfo}";
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        #endregion
    }
}
