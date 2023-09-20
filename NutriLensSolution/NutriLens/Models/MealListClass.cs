using NutriLens.Entities;

namespace NutriLens.Models
{
    public class MealListClass
    {
        public List<Meal> MealList { get; set; }

        public MealListClass(List<Meal> mealList)
        {
            MealList = mealList;
        }

        public string TotalEnergeticConsumption(EnergeticUnit energeticUnit)
        {
            double totalCalories = MealList
                .SelectMany(meal => meal.FoodItems)
                .Sum(foodItem => foodItem.KiloCalories);

            return energeticUnit switch
            {
                EnergeticUnit.kcal => $"{totalCalories} {Constants.kcalUnit}",
                EnergeticUnit.kJ => $"{totalCalories * Constants.kcalToKJFactor} {Constants.kJUnit}",
                _ => default
            };
        }
    }
}
