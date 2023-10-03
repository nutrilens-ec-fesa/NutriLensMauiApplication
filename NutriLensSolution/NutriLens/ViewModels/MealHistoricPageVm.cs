using CommunityToolkit.Mvvm.ComponentModel;
using NutriLens.Entities;
using NutriLens.Models;
using System.Collections.ObjectModel;

namespace NutriLens.ViewModels
{
    public partial class MealHistoricPageVm : ObservableObject
    {
        private INavigation _navigation;
        private MealHistoryFilter _mealHistoryFilter;
        private ObservableCollection<MealListClass> MealsList { get; set; }
        private List<Meal> MealList { get => MealsList[0].MealList; }
        public MealHistoricPageVm(INavigation navigation, MealHistoryFilter mealHistoryFilter)
        {
            _navigation = navigation;
            _mealHistoryFilter = mealHistoryFilter;

            List<MealListClass> mealLists = new List<MealListClass>();

            List<Meal> meals = new List<Meal>();

            switch (mealHistoryFilter)
            {
                case MealHistoryFilter.PerDay:
                    meals = AppDataHelperClass.GetAllMeals();

                    DateTime lastDateTime = DateTime.Now;

                    foreach (Meal meal in meals.OrderByDescending(x => x.DateTime))
                    {
                        if (meal.DateTime.ToShortDateString() != lastDateTime.ToShortDateString())
                        {
                            lastDateTime = meal.DateTime;
                            MealsList.Add(new MealListClass(new List<Meal>()));
                        }

                        MealsList[^1].MealList.Add(meal);
                    }

                    break;
                case MealHistoryFilter.PerWeek:
                    break;
                case MealHistoryFilter.PerMonth:
                    break;
                case MealHistoryFilter.PerPeriod:
                    break;
            }
        }
    }
}
