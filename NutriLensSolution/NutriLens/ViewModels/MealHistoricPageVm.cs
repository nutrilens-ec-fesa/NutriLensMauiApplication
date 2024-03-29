﻿using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLensClassLibrary.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

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

            if (meals.Count == 0)
            {
                ViewServices.PopUpManager.PopPersonalizedAsync("Sem refeições", "Não foram encontradas refeições registradas no dispositivo", "OK");
                _navigation.PopAsync();
                return;
            }

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
                case MealHistoryFilter.All:
                    foreach (Meal meal in meals.OrderByDescending(x => x.DateTime))
                    {
                        MealsList.Add(new MealListClass(new List<Meal>()));
                        MealsList[^1].MealList.Add(meal);
                    }
                    break;
            }

            foreach (MealListClass meal in MealsList)
            {
                switch (_mealHistoryFilter)
                {
                    case MealHistoryFilter.PerDay:
                        MealsListString.Add(meal.DailyInfo);
                        break;
                    case MealHistoryFilter.PerMonth:
                        MealsListString.Add(meal.MonthlyInfo);
                        break;
                    case MealHistoryFilter.All:
                        string mealInfo = meal.MealList[0].ToString() + Environment.NewLine + Environment.NewLine;

                        StringBuilder mealInfoBuilder = new StringBuilder(mealInfo);

                        foreach (FoodItem foodItem in meal.MealList[0].FoodItems)
                        {
                            // Se for um item contido na TBCA
                            if (foodItem.TbcaFoodItem != null)
                            {
                                mealInfoBuilder.Append($"   * {foodItem.TbcaFoodItem.Alimento} ({foodItem.Portion} g - {foodItem.KiloCalorieInfo}){Environment.NewLine}");
                            }
                            else
                            {
                                mealInfoBuilder.Append($"  * {foodItem.Name} ({foodItem.Portion} g - {foodItem.KiloCalorieInfo}){Environment.NewLine}");
                            }
                        }

                        MealsListString.Add(mealInfoBuilder.ToString());
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
                MealHistoryFilter.All => "Todas as refeições",
                _ => string.Empty,
            };
        }

        [RelayCommand]
        private async void MealTapped(object obj)
        {
            // TODO: Tomar ação para quando o item for clicado
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
