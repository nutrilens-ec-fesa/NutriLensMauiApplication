using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DateTimeLibrary;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using NutriLensClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriLens.ViewModels
{
    internal partial class GroupedMealHistoricPageVm : ObservableObject
    {
        private INavigation _navigation;
        private MealHistoryFilter _mealHistoryFilter;

        public string HistoricPageName { get; set; }

        public ObservableCollection<Meal> Meals { get; set; }
        public ObservableCollection<MealListClass> MealsList { get; set; }

        public GroupedMealHistoricPageVm(INavigation navigation, MealHistoryFilter mealHistoryFilter)
        {
            _navigation = navigation;
            _mealHistoryFilter = mealHistoryFilter;
            MealsList = new ObservableCollection<MealListClass>();
            HistoricPageName = GetFilterPageName();
        }

        [RelayCommand]
        private void Appearing()
        {
            if (MealsList.Count > 0)
                return;

            List<Meal> meals = AppDataHelperClass.GetAllMeals();

            if (meals.Count == 0)
            {
                ViewServices.PopUpManager.PopPersonalizedAsync("Sem refeições", "Não foram encontradas refeições registradas no dispositivo", "OK");
                _navigation.PopAsync();
                return;
            }

            Meals = new ObservableCollection<Meal>();

            DateTime lastDateTime = DateTime.MinValue;

            switch (_mealHistoryFilter)
            {
                case MealHistoryFilter.PerDay:

                    lastDateTime = DateTime.Now;

                    Meal lastMeal = meals.OrderByDescending(x => x.DateTime).ToList()[0];

                    while (lastDateTime > lastMeal.DateTime)
                    {
                        if (DateTimeFunctions.CheckSameIfIsSameDate(lastDateTime, lastMeal.DateTime))
                            break;
                        else
                            MealsList.Add(new MealListClass(new List<Meal>(), _mealHistoryFilter, lastDateTime));

                        lastDateTime = lastDateTime.AddDays(-1);
                    }

                    lastDateTime = DateTime.MinValue;

                    foreach (Meal meal in meals.OrderByDescending(x => x.DateTime))
                    {
                        if (lastDateTime == DateTime.MinValue)
                        {
                            MealsList.Add(new MealListClass(new List<Meal>(), _mealHistoryFilter));
                            lastDateTime = meal.DateTime;
                        }

                        if (meal.DateTime.ToShortDateString() != lastDateTime.ToShortDateString())
                        {
                            int i = 1;

                            while (lastDateTime.AddDays(-i) > meal.DateTime)
                            {
                                if (DateTimeFunctions.CheckSameIfIsSameDate(lastDateTime.AddDays(-i), meal.DateTime))
                                    break;
                                else
                                    MealsList.Add(new MealListClass(new List<Meal>(), _mealHistoryFilter, lastDateTime.AddDays(-i)));
                                
                                i++;
                            }

                            lastDateTime = meal.DateTime;
                            MealsList.Add(new MealListClass(new List<Meal>(), _mealHistoryFilter));
                        }

                        MealsList[^1].MealList.Add(meal);
                    }
                    break;
                case MealHistoryFilter.PerWeek:

                    DateTime lastDayOfWeek = DateTimeFunctions.GetLastDayOfWeekByDateTimeNow();

                    lastDayOfWeek = lastDayOfWeek.AddDays(-1);

                    DateTime weekLastDay = DateTimeFunctions.GetDateTimeEnd(lastDayOfWeek.Date);
                    DateTime weekFirstDay = weekLastDay.AddDays(-6).Date;

                    bool realMealInserted = false;

                    foreach (Meal meal in meals.OrderByDescending(x => x.DateTime))
                    {
                        while (meal.DateTime < weekFirstDay || meal.DateTime > weekLastDay)
                        {
                            if(!realMealInserted)
                                MealsList.Add(new MealListClass(new List<Meal>(), _mealHistoryFilter, weekLastDay));
                            else
                                MealsList.Add(new MealListClass(new List<Meal>(), _mealHistoryFilter));

                            weekLastDay = weekFirstDay.AddDays(-1).Date;
                            weekFirstDay = weekLastDay.AddDays(-6).Date;
                        }

                        if (MealsList.Count == 0 || (MealsList[^1].MealList.Count > 0 && MealsList[^1].MealList[0].DateTime < weekFirstDay))
                            MealsList.Add(new MealListClass(new List<Meal>(), _mealHistoryFilter));

                        realMealInserted = true;

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
                                MealsList.Add(new MealListClass(new List<Meal>() { new Meal { DateTime = new DateTime(lastDateTime.Year, lastDateTime.Month - 1, 1) } }, _mealHistoryFilter));
                                i++;
                            }

                            lastDateTime = meal.DateTime;
                            MealsList.Add(new MealListClass(new List<Meal>(), _mealHistoryFilter));
                        }

                        MealsList[^1].MealList.Add(meal);
                    }
                    break;
                case MealHistoryFilter.PerPeriod:
                    break;
                case MealHistoryFilter.All:
                    foreach (Meal meal in meals.OrderByDescending(x => x.DateTime))
                    {
                        Meals.Add(meal);
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
                        // MealsListString.Add(meal.DailyInfo);
                        break;
                    case MealHistoryFilter.PerMonth:
                        // MealsListString.Add(meal.MonthlyInfo);
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

                        meal.MealPlusItemsInfo = mealInfoBuilder.ToString();
                        // MealsListString.Add(mealInfoBuilder.ToString());
                        break;
                }

                OnPropertyChanged(nameof(Meals));
                OnPropertyChanged(nameof(MealsList));

                // OnPropertyChanged(nameof(MealsListString));
            }

            UpdateMealsList();
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

        private void UpdateMealsList()
        {
            List<MealListClass> updatedMeals = MealsList.ToList();

            MealsList.Clear();

            foreach (MealListClass mealListClass in updatedMeals)
            {
                MealsList.Add(mealListClass);
            }
        }

        [RelayCommand]
        private async Task ListMeals(MealListClass filteredMealList)
        {
            if (int.Parse(filteredMealList.MealCount) == 0)
            {
                await ViewServices.PopUpManager.PopErrorAsync("O período não possui refeições registradas.");
                return;
            }

            AppDataHelperClass.FilteredMealList = filteredMealList;

            await _navigation.PushAsync(ViewServices.ResolvePage<IMealHistoricPage>());
            //if (item.FoodItems[0].BarcodeItemEntry != null)
            //    await _navigation.PushAsync(ViewServices.ResolvePage<IBarCodePage>());
            //else
            //    await _navigation.PushAsync(ViewServices.ResolvePage<IManualInputPage>());
        }
    }
}
