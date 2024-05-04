using DateTimeLibrary;
using NutriLens.Entities;
using NutriLensClassLibrary.Entities;
using NutriLensClassLibrary.Models;
using System.ComponentModel;
using System.Text;

namespace NutriLens.Models
{
    /// <summary>
    /// Representa um modelo contendo uma lista de refeições
    /// </summary>
    public class MealListClass : INotifyPropertyChanged
    {
        /// <summary>
        /// Lista de refeições
        /// </summary>
        public List<Meal> MealList { get; set; }

        public MealHistoryFilter MealHistoryFilter { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string MealListInfo
        {
            get
            {
                switch (MealHistoryFilter)
                {
                    case MealHistoryFilter.PerDay:
                        return DailyInfo;
                    case MealHistoryFilter.PerWeek:
                        return WeeklyInfo;
                    case MealHistoryFilter.PerMonth:
                        return MonthlyInfo;
                    case MealHistoryFilter.PerPeriod:
                        return PeriodInfo;
                    default:
                        return string.Empty;
                }
            }
        }
        /// <summary>
        /// Caso todas as refeições sejam da mesma data, retorna uma string com o consumo energético
        /// </summary>
        public string DailyInfo
        {
            get
            {
                if (MealList.DistinctBy(x => x.DateTime.ToShortDateString()).Count() == 1)
                    return $"{MealList[0].DateTime.ToShortDateString()} - {DateTimeFunctions.GetWeekDayNameByDateTime(MealList[0].DateTime)}";
                else
                    return "undefined";
            }
        }

        public string WeeklyInfo
        {
            get
            {
                if (MealList == null || MealList.Count == 0)
                    return string.Empty;

                DateTime lastDayOfWeek = DateTimeFunctions.GetLastDayOfWeekByDateTime(MealList[0].DateTime);
                
                return $"{lastDayOfWeek.AddDays(-6).ToShortDateString()} -> {lastDayOfWeek.ToShortDateString()}";

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
                    return $"{DateTimeFunctions.GetMonthNameByDateTime(MealList[0].DateTime)}/{MealList[0].DateTime.Year}";
                else
                    return "undefined";
            }
        }

        public string PeriodInfo
        {
            get
            {
                return $"{StartDate.Date.ToShortDateString()} -> {EndDate.Date.ToShortDateString()}";
            }
        }

        public string MealCount { get => MealList.Count(x => !string.IsNullOrEmpty(x.Name)).ToString(); }

        private string _mealPlusItemsInfo;

        public string MealPlusItemsInfo
        {
            get => _mealPlusItemsInfo;
            set
            {
                _mealPlusItemsInfo = value;
                OnPropertyChanged(nameof(MealPlusItemsInfo));
            }
        }

        public string TotalEnergeticConsumptionString { get => TotalEnergeticConsumption(false); }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="mealList">Lista de refeições</param>
        public MealListClass(List<Meal> mealList)
        {
            MealList = mealList;
        }

        public MealListClass(List<Meal> mealList, MealHistoryFilter mealHistoryFilter)
        {
            MealList = mealList;
            MealHistoryFilter = mealHistoryFilter;
        }

        public MealListClass(List<Meal> mealList, MealHistoryFilter mealHistoryFilter, DateTime dateTime)
        {
            MealList = mealList;
            MealHistoryFilter = mealHistoryFilter;

            if (MealList.Count == 0)
            {
                MealList.Add(new Meal() { DateTime = dateTime });
            }
        }

        public MealListClass(List<Meal> mealList, DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            MealList = mealList;
            MealHistoryFilter = MealHistoryFilter.PerPeriod;
            StartDate = dateTimeStart;
            EndDate = dateTimeEnd;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Retorna uma string do consumo energético total das refeições da lista
        /// </summary>
        /// <param name="energeticUnit">Unidade de consumo energético a ser retornado</param>
        /// <returns></returns>
        public string TotalEnergeticConsumption(bool valueOnly = false)
        {
            double totalCalories = TotalEnergeticConsumption();

            if (valueOnly)
                return totalCalories.ToString("0.00");
            else
            {
                return AppConfigHelperClass.EnergeticUnit switch
                {
                    EnergeticUnit.kcal => $"{Math.Round(totalCalories, 2)} {Constants.kcalUnit}",
                    EnergeticUnit.kJ => $"{Math.Round(totalCalories, 2)} {Constants.kJUnit}",
                    _ => default
                };
            }
        }

        /// <summary>
        /// Retorna um double do consumo energético total das refeições da lista
        /// </summary>
        /// <param name="energeticUnit">Unidade de consumo energético a ser retornado</param>
        /// <returns></returns>
        public double TotalEnergeticConsumption()
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

            return AppConfigHelperClass.EnergeticUnit switch
            {
                EnergeticUnit.kcal => totalCalories,
                EnergeticUnit.kJ => totalCalories * Constants.kcalToKJFactor,
                _ => default
            };
        }

        public double TotalCarbohydratesConsumption()
        {
            double totalCarbohydrates;

            try
            {
                totalCarbohydrates = Math.Round(MealList
                    .SelectMany(meal => meal.FoodItems)
                    .Where(x => x.TacoFoodItem != null && x.TacoFoodItem.Carboidrato != null && double.TryParse(x.TacoFoodItem.Carboidrato.ToString(), out _))
                    .Sum(foodItem => (double)foodItem.TacoFoodItem.Carboidrato), 2);
            }
            catch
            {
                totalCarbohydrates = 0;
            }

            return totalCarbohydrates;
        }

        public double TotalProteinsConsumption()
        {
            double totalProteins;

            try
            {
                totalProteins = Math.Round(MealList
                    .SelectMany(meal => meal.FoodItems)
                    .Where(x => x.TacoFoodItem != null && x.TacoFoodItem.Proteina != null && double.TryParse(x.TacoFoodItem.Proteina.ToString(), out _))
                    .Sum(foodItem => (double)foodItem.TacoFoodItem.Proteina), 2);
            }
            catch
            {
                totalProteins = 0;
            }

            return totalProteins;
        }

        public double TotalFatConsumption()
        {
            double totalFat;

            try
            {
                totalFat = Math.Round(MealList
                    .SelectMany(meal => meal.FoodItems)
                    .Where(x => x.TacoFoodItem != null && x.TacoFoodItem.Lipideos != null && double.TryParse(x.TacoFoodItem.Lipideos.ToString(), out _))
                    .Sum(foodItem => (double)foodItem.TacoFoodItem.Lipideos), 2);
            }
            catch
            {
                totalFat = 0;
            }

            return totalFat;
        }

        public double TotalFibersConsumption()
        {
            double totalFibers;

            try
            {
                totalFibers = Math.Round(MealList
                    .SelectMany(meal => meal.FoodItems)
                    .Where(x => x.TacoFoodItem != null && x.TacoFoodItem.FibraAlimentar != null && double.TryParse(x.TacoFoodItem.FibraAlimentar.ToString(), out _))
                    .Sum(foodItem => (double)foodItem.TacoFoodItem.FibraAlimentar), 2);
            }
            catch
            {
                totalFibers = 0;
            }

            return totalFibers;
        }

        public double TotalSodiumConsumption()
        {
            double totalSodium;

            try
            {
                totalSodium = Math.Round(MealList
                    .SelectMany(meal => meal.FoodItems)
                    .Where(x => x.TacoFoodItem != null && x.TacoFoodItem.Sodio != null && double.TryParse(x.TacoFoodItem.Sodio.ToString(), out _))
                    .Sum(foodItem => (double)foodItem.TacoFoodItem.Sodio), 2);
            }
            catch
            {
                totalSodium = 0;
            }

            return totalSodium;
        }
    }
}
