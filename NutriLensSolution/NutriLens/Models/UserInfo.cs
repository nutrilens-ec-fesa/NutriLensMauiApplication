namespace NutriLens.Models
{
    /// <summary>
    /// Classe de dados do usuário, configurações de limites e registro de restrições alimentares
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Data de nascimento do usuário
        /// </summary>
        public DateTime BornDate { get; set; }

        /// <summary>
        /// Peso em kg
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Altura em metros
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Objetivo de consumo diário de calorias
        /// </summary>
        public int KiloCaloriesDiaryObjective { get; set; }

        /// <summary>
        /// Usuário hipertenso
        /// </summary>
        public bool Hipertension { get; set; }

        /// <summary>
        /// Usuário com diabetes
        /// </summary>
        public bool Diabetes { get; set; }

        /// <summary>
        /// Usuário intolerante a lactose
        /// </summary>
        public bool LactoseIntolerant { get; set; }

        /// <summary>
        /// Usuário intolerante a glúten
        /// </summary>
        public bool GlutenIntolerant { get; set; }
    }
}
