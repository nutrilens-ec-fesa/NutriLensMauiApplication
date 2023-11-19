using NutriLens.Entities;

namespace NutriLens.Models
{
    /// <summary>
    /// Representa um modelo contendo uma lista de refeições
    /// </summary>
    public class MealListClass
    {
        /// <summary>
        /// Lista de refeições
        /// </summary>
        public List<Meal> MealList { get; set; }
        
        /// <summary>
        /// Caso todas as refeições sejam da mesma data, retorna uma string com o consumo energético
        /// </summary>
        public string DailyInfo
        {
            get
            {
                if (MealList.DistinctBy(x => x.DateTime.ToShortDateString()).Count() == 1)
                    return $"{MealList[0].DateTime.ToShortDateString()} - {TotalEnergeticConsumption(AppConfigHelperClass.EnergeticUnit)}";
                else
                    return "undefined";
            }
        }

        /// <summary>
        /// Caso todas as refeições sejam do mesmo mês, retorna uma string com o consumo energético
        /// </summary>
        public string MonthlyInfo
        {
            get
            {
                if (MealList.DistinctBy(x => x.DateTime.Month).Count() == 1)
                    return $"{MealList[0].DateTime.Month}/{MealList[0].DateTime.Year} - {TotalEnergeticConsumption(AppConfigHelperClass.EnergeticUnit)}";
                else
                    return "undefined";
            }
        }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="mealList">Lista de refeições</param>
        public MealListClass(List<Meal> mealList)
        {
            MealList = mealList;
        }

        /// <summary>
        /// Retorna uma string do consumo energético total das refeições da lista
        /// </summary>
        /// <param name="energeticUnit">Unidade de consumo energético a ser retornado</param>
        /// <returns></returns>
        public string TotalEnergeticConsumption(EnergeticUnit energeticUnit)
        {
            double totalCalories;

            try
            {
                totalCalories = MealList
                    .SelectMany(meal => meal.FoodItems)
                    .Sum(foodItem => foodItem.KiloCalories);
            }
            catch
            {
                totalCalories = 0;
            }

            return energeticUnit switch
            {
                EnergeticUnit.kcal => $"{totalCalories} {Constants.kcalUnit}",
                EnergeticUnit.kJ => $"{totalCalories * Constants.kcalToKJFactor} {Constants.kJUnit}",
                _ => default
            };
        }
    }
}
