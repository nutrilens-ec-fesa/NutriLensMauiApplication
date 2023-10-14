using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace NutriLens.ViewModels
{
    internal partial class MealHistoricPageVm : INotifyPropertyChanged
    {
        private INavigation _navigation;
        private MealHistoryFilter _mealHistoryFilter;
        public string HistoricPageName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<MealListClass> MealsList { get; set; }
        public ObservableCollection<string> MealsListString { get; set; }
        public MealHistoricPageVm(INavigation navigation, MealHistoryFilter mealHistoryFilter)
        {
            _navigation = navigation;
            _mealHistoryFilter = mealHistoryFilter;
            MealsListString = new ObservableCollection<string>();
            MealsList = new ObservableCollection<MealListClass>();
            HistoricPageName = GetFilterPageName();
        }

        [RelayCommand]
        private void Appearing()
        {
            List<Meal> meals;
            meals = AppDataHelperClass.GetAllMeals();
            DateTime lastDateTime = DateTime.MinValue;

            switch (_mealHistoryFilter)
            {
                case MealHistoryFilter.PerDay:

                    lastDateTime = DateTime.MinValue;

                    foreach (Meal meal in meals.OrderByDescending(x => x.DateTime))
                    {
                        if (meal.DateTime.ToShortDateString() != lastDateTime.ToShortDateString())
                        {
                            int i = 1;

                            while (lastDateTime != DateTime.MinValue && lastDateTime.AddDays(-i) > meal.DateTime)
                            {
                                MealsList.Add(new MealListClass(new List<Meal>() { new Meal { DateTime = lastDateTime.AddDays(-i) } }));
                                i++;
                            }

                            lastDateTime = meal.DateTime;
                            MealsList.Add(new MealListClass(new List<Meal>()));
                        }

                        MealsList[^1].MealList.Add(meal);
                    }

                    break;
                case MealHistoryFilter.PerWeek:
                    foreach (Meal meal in meals.OrderByDescending(x => x.DateTime))
                    {
                        if (meal.DateTime.ToShortDateString() != lastDateTime.ToShortDateString())
                        {
                            int i = 1;

                            while (lastDateTime != DateTime.MinValue && lastDateTime.AddDays(-i) > meal.DateTime)
                            {
                                MealsList.Add(new MealListClass(new List<Meal>() { new Meal { DateTime = lastDateTime.AddDays(-i) } }));
                                i++;
                            }

                            lastDateTime = meal.DateTime;
                            MealsList.Add(new MealListClass(new List<Meal>()));
                        }

                        MealsList[^1].MealList.Add(meal);
                    }
                    break;
                case MealHistoryFilter.PerMonth:
                    foreach (Meal meal in meals.OrderByDescending(x => x.DateTime))
                    {
                        if (meal.DateTime.Month != lastDateTime.Month || meal.DateTime.Year != lastDateTime.Year)
                        {
                            int i = 1;

                            while (lastDateTime != DateTime.MinValue && lastDateTime.Year == meal.DateTime.Year && lastDateTime.Month - 1 > meal.DateTime.Month)
                            {
                                MealsList.Add(new MealListClass(new List<Meal>() { new Meal { DateTime = new DateTime(lastDateTime.Year, lastDateTime.Month - 1, 1) } }));
                                i++;
                            }

                            lastDateTime = meal.DateTime;
                            MealsList.Add(new MealListClass(new List<Meal>()));
                        }

                        MealsList[^1].MealList.Add(meal);
                    }
                    break;
                case MealHistoryFilter.PerPeriod:
                    break;
            }

            foreach (MealListClass meal in MealsList)
            {
                switch (_mealHistoryFilter)
                {
                    case MealHistoryFilter.PerDay:
                        MealsListString.Add(meal.MealListDailyInfo);
                        break;
                    case MealHistoryFilter.PerMonth:
                        MealsListString.Add(meal.MealListMonthlyInfo);
                        break;
                }

                OnPropertyChanged(nameof(MealsListString));
            }
        }

        private string GetFilterPageName()
        {
            return _mealHistoryFilter switch
            {
                MealHistoryFilter.PerDay => "Histórico por dia",
                MealHistoryFilter.PerWeek => "Histórico por semana",
                MealHistoryFilter.PerMonth => "Histórico por mês",
                MealHistoryFilter.PerPeriod => "Histórico por período",
                _ => string.Empty,
            };
        }

        //public static int GetWeekOfYear(DateTime date1)
        //{
        //    //CultureInfo cultureInfo = CultureInfo.CurrentCulture;

        //    //int week1 = cultureInfo.Calendar.GetWeekOfYear(date1, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek);
        //    //int week2 = cultureInfo.Calendar.GetWeekOfYear(date2, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek);

        //    //// Compare the week numbers
        //    //return week1 == week2;
        //}
    }
}
