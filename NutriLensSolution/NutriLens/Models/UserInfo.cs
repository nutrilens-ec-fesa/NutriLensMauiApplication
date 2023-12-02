namespace NutriLens.Models
{
    public enum HabitualPhysicalActivity
    {
        LightActivity,
        ModeratelyActive,
        VigorouslyActive
    }

    public enum Gender
    {
        Masculine,
        Feminine
    }

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
        /// Sexo do usuário
        /// </summary>
        public Gender Gender { get; set; }

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

        /// <summary>
        /// Define o grau de intensidade em atividades físicas
        /// </summary>
        public HabitualPhysicalActivity HabitualPhysicalActivity { get; set; }
    }
}
