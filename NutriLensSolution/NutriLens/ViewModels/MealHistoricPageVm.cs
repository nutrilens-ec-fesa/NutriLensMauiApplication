using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace NutriLens.ViewModels
{
    internal partial class MealHistoricPageVm : INotifyPropertyChanged
    {
        private INavigation _navigation;
        private MealHistoryFilter _mealHistoryFilter;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<MealListClass> MealsList { get; set; }
        private ObservableCollection<string> MealsListString { get; set; }
        public MealHistoricPageVm(INavigation navigation, MealHistoryFilter mealHistoryFilter)
        {
            _navigation = navigation;
            _mealHistoryFilter = mealHistoryFilter;
            MealsListString = new ObservableCollection<string>();
            MealsList = new ObservableCollection<MealListClass>();
        }

        [RelayCommand]
        private void Appearing()
        {
            List<Meal> meals;

            switch (_mealHistoryFilter)
            {
                case MealHistoryFilter.PerDay:
                    meals = AppDataHelperClass.GetAllMeals();

                    DateTime lastDateTime = DateTime.MinValue;

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

            foreach (MealListClass meal in MealsList)
            {
                MealsListString.Add(meal.MealListInfo);
                OnPropertyChanged(nameof(MealsListString));
            }
        }

        [RelayCommand]
        private void Test()
        {
            OnPropertyChanged(nameof(MealsListString));
        }
    }
}
