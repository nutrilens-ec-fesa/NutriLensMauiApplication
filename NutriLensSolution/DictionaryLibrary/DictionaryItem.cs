using Newtonsoft.Json;
using System.ComponentModel;

namespace DictionaryLibrary
{
    public class DictionaryItem : INotifyPropertyChanged
    {
        private string _keyString;
        private string _portuguese;
        private string _spanish;
        private string _english;

        /// <summary>
        /// Chave string do item
        /// </summary>
        [JsonProperty("Key")]
        public string KeyString
        {
            get => _keyString;
            set
            {
                _keyString = value;
                OnPropertyChanged(nameof(KeyString));
            }
        }

        /// <summary>
        /// Texto em português
        /// </summary>
        [JsonProperty("Pt")]
        public string Portuguese
        {
            get => _portuguese;
            set
            {
                _portuguese = value;
                OnPropertyChanged(nameof(_portuguese));
            }
        }

        /// <summary>
        /// Texto em espanhol
        /// </summary>
        [JsonProperty("Sp")]
        public string Spanish
        {
            get => _spanish;
            set
            {
                _spanish = value;
                OnPropertyChanged(nameof(_spanish));
            }
        }

        /// <summary>
        /// Texto em inglês
        /// </summary>
        [JsonProperty("En")]
        public string English
        {
            get => _english;
            set
            {
                _english = value;
                OnPropertyChanged(nameof(_english));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Bool que indica se o item possui chave
        /// </summary>
        [JsonIgnore]
        public bool HasKey { get => !string.IsNullOrEmpty(KeyString); }

        /// <summary>
        /// Retorna o texto no idioma especificado
        /// </summary>
        /// <param name="language">Idioma</param>
        /// <returns>Texto no idioma especificado</returns>
        public string GetValue(Languages language)
        {
            switch (language)
            {
                case Languages.Portuguese:
                    return Portuguese;
                case Languages.Spanish:
                    return Spanish;
                case Languages.English:
                    return English;
                default:
                    return null;
            }
        }
    }
}
