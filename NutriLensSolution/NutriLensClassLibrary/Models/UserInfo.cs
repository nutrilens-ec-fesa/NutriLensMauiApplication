﻿using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NutriLensClassLibrary.Models
{
    public enum HabitualPhysicalActivity
    {
        Uninformed,
        LightActivity,
        ModeratelyActive,
        VigorouslyActive
    }

    public enum DailyKiloCaloriesObjective
    {
        Uninformed,
        Reduce,
        Maintain,
        Fatten
    }

    public enum Gender
    {
        Uninformed,
        Masculine,
        Feminine
    }

    /// <summary>
    /// Classe de dados do usuário, configurações de limites e registro de restrições alimentares
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Id na base de dados
        /// </summary>
        [BsonId]
        public string Id { get; set; }
        ///// <summary>
        ///// Nome do usuário
        ///// </summary>
        //public string Name { get; set; }

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
        /// Gasto energético basal diário conforme gênero e idade
        /// https://www.tuasaude.com/como-calcular-o-gasto-calorico/
        /// </summary>
        public double BasalDailyCalories { get; set; }

        /// <summary>
        /// Meta de ingestão de calorias diarias baseado gasto basal e no nível de atividade física.
        /// </summary>
        public double DailyKiloCaloriesBurn { get; set; }

        /// <summary>
        /// Meta de ingestão de calorias diárias baseado no objetivo de ganho ou perca de peso
        /// </summary>
        public double DailyKiloCaloriesGoal { get; set; }

        /// <summary>
        /// Customização da meta de ingestão de carboidratos diários
        /// </summary>
        public double DailyCarbohydrateGoal { get; set; }
        
        /// /// <summary>
        /// Customização da meta de ingestão de proteinas diárias
        /// </summary>
        public double DailyProteinGoal { get; set; }
        

        /// <summary>
        /// Customização da meta de ingestão de gorduras diários
        /// </summary>
        public double DailyFatGoal { get; set; }
        

        /// <summary>
        /// Customização da meta de ingestão de fibras diários
        /// </summary>
        public double DailyFiberGoal { get; set; }
        

        /// <summary>
        /// Customização da meta de ingestão de sódio diários
        /// </summary>
        public double DailySodiumGoal { get; set; }

        /// <summary>
        /// Customização da meta de ingestão de colesterol diários
        /// </summary>
        public double DailyCholesterolGoal { get; set; }

        /// <summary>
        /// Customização da meta de ingestão de calcio diários
        /// </summary>
        public double DailyCalciumGoal { get; set; }

        /// <summary>
        /// Customização da meta de ingestão de ferro diários
        /// </summary>
        public double DailyIronGoal { get; set; }

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
        /// Informa se o usuário aceitou os Termos de Uso do aplicativo
        /// </summary>
        public bool? TermsAccepted { get; set; }

        /// <summary>
        /// Define o grau de intensidade em atividades físicas
        /// </summary>
        public HabitualPhysicalActivity HabitualPhysicalActivity { get; set; }

        /// <summary>
        /// Objetivo do usuario de perder, manter ou ganhar peso
        /// </summary>
        public DailyKiloCaloriesObjective DailyKiloCaloriesObjective { get; set; }

        /// <summary>
        /// Propriedade que identifica se o usuário possui uma especialização dentro do sistema
        /// </summary>
        /// 
        public string Role { get; set; }

        /// <summary>
        /// Informa se usuario utiliza estimativa de gasto calorico automatica
        /// </summary>
        public bool UseSuggestedCaloricGoal { get; set; }

        [JsonIgnore, BsonIgnore]
        public bool DevUser { get => Role == "DEV"; }
    }
}
