using NutriLensClassLibrary.Models;

namespace NutriLensWebApp.Interfaces
{
    public interface IMeal
    {
        public List<Meal> GetAllMeals();
        public List<Meal> GetAllMealsByUserIdentifier(string userIdentifier);
        public void InsertMeals(List<Meal> meals);
        public void UpdateMeal(Meal meal);
        public void RemoveMeal(string mealId);
    }
}
