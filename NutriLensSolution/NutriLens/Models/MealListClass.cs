using NutriLens.Entities;

namespace NutriLens.Models
{
    public class MealListClass
    {
        public List<Meal> MealList { get; set; }

        public string MealListInfo
        {
            get
            {
                if (MealList.DistinctBy(x => x.DateTime.ToShortDateString()).Count() == 1)
                    return $"{MealList[0].DateTime.ToShortDateString()} {TotalEnergeticConsumption(AppConfigHelperClass.EnergeticUnit)}";
                else
                    return "undefined";
            }
        }

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
