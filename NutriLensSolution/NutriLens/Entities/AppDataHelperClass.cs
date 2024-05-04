using ExceptionLibrary;
using NutriLens.Models;
using NutriLens.Services;
using NutriLensClassLibrary.Entities;
using NutriLensClassLibrary.Models;
using StringLibrary;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace NutriLens.Entities
{
    /// <summary>
    /// Classe que gerencia o acesso aos dados salvos no aplicativo
    /// </summary>
    public static class AppDataHelperClass
    {
        /// <summary>
        /// Enumerador de tipos de dados
        /// </summary>
        public enum DataItems
        {
            /// <summary>
            /// Lista de refeições
            /// </summary>
            MealList,

            /// <summary>
            /// Informações do usuário
            /// </summary>
            UserInfo,

            /// <summary>
            /// Itens alimentícios da tabela TBCA
            /// </summary>
            TbcaFoodItems,

            /// <summary>
            /// Itens alimentícios da tabela TACO
            /// </summary>
            TacoFoodItems,

            /// <summary>
            /// Token da API do NutriLens
            /// </summary>
            NutriLensApiToken,

            /// <summary>
            /// Lista de atividades físicas realizadas
            /// </summary>
            PhysicalActivityList
        }

        private static List<Meal> _mealList;
        private static UserInfo _userInfo;
        private static List<TbcaItem> _tbcaFoodItems;
        private static List<TacoItem> _tacoFoodItems;
        private static List<PhysicalActivity> _physicalActivityList;
        private static string _nutriLensApiToken;
        private static string[] excludedWords = new[] { "de", "com", "sem", "em", "tipo", "inteiro", "à", "grande", "para", "da", "ao", "e", "do" };
        /// <summary>
        /// Lista de refeições
        /// </summary>
        private static List<Meal> MealList
        {
            get
            {
                if (_mealList == null)
                {
                    try
                    {
                        _mealList = ViewServices.AppDataManager.GetItem<List<Meal>>(DataItems.MealList);
                        _mealList = _mealList
                            .Where(x => x.FoodItems != null && x.FoodItems.Count > 0)
                            .ToList();
                    }
                    catch (NotFoundException)
                    {
                        _mealList = new List<Meal>();
                    }
                }

                return _mealList;
            }
        }

        /// <summary>
        /// Informações do usuário
        /// </summary>
        public static UserInfo UserInfo
        {
            get
            {
                if (_userInfo == null)
                {
                    try
                    {
                        _userInfo = ViewServices.AppDataManager.GetItem<UserInfo>(DataItems.UserInfo);
                    }
                    catch (NotFoundException)
                    {

                    }
                }

                if (_userInfo == null)
                    _userInfo = new UserInfo();

                return _userInfo;
            }
        }

        /// <summary>
        /// Lista de itens da tabela TBCA
        /// </summary>
        public static List<TbcaItem> TbcaFoodItems
        {
            get
            {
                if (_tbcaFoodItems == null)
                {
                    try
                    {
                        _tbcaFoodItems = ViewServices.AppDataManager.GetItem<List<TbcaItem>>(DataItems.TbcaFoodItems);
                    }
                    catch (NotFoundException)
                    {
                        _tbcaFoodItems = new List<TbcaItem>();
                    }
                }

                return _tbcaFoodItems;
            }
        }

        public static List<TacoItem> TacoFoodItems
        {
            get
            {
                if (_tacoFoodItems == null)
                {
                    try
                    {
                        _tacoFoodItems = ViewServices.AppDataManager.GetItem<List<TacoItem>>(DataItems.TacoFoodItems);
                    }
                    catch (NotFoundException)
                    {
                        _tacoFoodItems = new List<TacoItem>();
                    }
                }

                return _tacoFoodItems;
            }
        }

        /// <summary>
        /// Lista de atividades físicas realizadas
        /// </summary>
        private static List<PhysicalActivity> PhysicalActivityList
        {
            get
            {
                if (_physicalActivityList == null)
                {
                    try
                    {
                        _physicalActivityList = ViewServices.AppDataManager.GetItem<List<PhysicalActivity>>(DataItems.PhysicalActivityList);
                    }
                    catch (NotFoundException)
                    {
                        _physicalActivityList = new List<PhysicalActivity>();
                    }
                }

                return _physicalActivityList;
            }
        }

        /// <summary>
        /// Token de acesso à API do NutriLens
        /// </summary>
        public static string NutriLensApiToken
        {
            get
            {
                if (_nutriLensApiToken == null)
                {
                    try
                    {
                        _nutriLensApiToken = ViewServices.AppDataManager.GetItem<string>(DataItems.NutriLensApiToken);

                        if (_nutriLensApiToken == null)
                            _nutriLensApiToken = string.Empty;
                    }
                    catch (NotFoundException)
                    {
                        _nutriLensApiToken = string.Empty;
                    }
                }

                return _nutriLensApiToken;
            }
        }

        /// <summary>
        /// Verifica se existe informações de usuário salvas no aplicativo
        /// </summary>
        public static bool HasUserInfo { get => UserInfo != null && !string.IsNullOrEmpty(UserInfo.Id); }

        #region Meal methods

        /// <summary>
        /// Adiciona uma nova refeição
        /// </summary>
        /// <param name="meal"></param>
        public static void AddMeal(Meal meal)
        {
            MealList.Add(meal);
            ViewServices.AppDataManager.SetItem(DataItems.MealList, MealList);
        }

        /// <summary>
        /// Retorna todas as refeições salvas
        /// </summary>
        /// <returns></returns>
        public static List<Meal> GetAllMeals()
        {
            return MealList;
        }

        /// <summary>
        /// Retorna todas as refeições da data atual
        /// </summary>
        /// <returns></returns>
        public static List<Meal> GetTodayMeals()
        {
            return MealList
                .Where(x => x.DateTime.Date == DateTime.Today)
                .ToList();
        }

        /// <summary>
        /// Retorna todas as refeições em um data específica
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<Meal> GetMealsByDate(DateTime date)
        {
            return MealList
                .Where(x => x.DateTime.Date == date.Date)
                .ToList();
        }

        /// <summary>
        /// Retorna as refeições que aconteceram entre um intervalo específico
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static List<Meal> GetMealsByDateRange(DateTime startDate, DateTime endDate)
        {
            return MealList
                .Where(x => x.DateTime.Date >= startDate.Date && x.DateTime.Date <= endDate.Date)
                .ToList();
        }

        public static DateTime GetFirstMealDateTime()
        {
            List<Meal> meals = GetAllMeals();

            if (meals.Count == 0)
                return DateTime.Now;
            else
                return meals.OrderBy(x => x.DateTime).ToList()[0].DateTime;
        }

        public static void RemoveMeal(Meal meal)
        {
            MealList.Remove(meal);
            ViewServices.AppDataManager.SetItem(DataItems.MealList, MealList);
        }

        public static void SaveChangesOnMeals()
        {
            ViewServices.AppDataManager.SetItem(DataItems.MealList, MealList);
        }

        #endregion

        #region Physical Activity Methods

        /// <summary>
        /// Adiciona uma nova atividade física
        /// </summary>
        /// <param name="physicalActivity"></param>
        public static void AddPhysicalActivity(PhysicalActivity physicalActivity)
        {
            PhysicalActivityList.Add(physicalActivity);
            ViewServices.AppDataManager.SetItem(DataItems.PhysicalActivityList, PhysicalActivityList);
        }

        /// <summary>
        /// Retorna todas as atividades físicas salvas
        /// </summary>
        /// <returns></returns>
        public static List<PhysicalActivity> GetAllPhysicalActivities()
        {
            return PhysicalActivityList;
        }

        /// <summary>
        /// Retorna todas as atividades físicas da data atual
        /// </summary>
        /// <returns></returns>
        public static List<PhysicalActivity> GetTodayPhysicalActivities()
        {
            return PhysicalActivityList
                .Where(x => x.DateTime.Date == DateTime.Today)
                .ToList();
        }

        /// <summary>
        /// Retorna todas as atividades físicas em uma data específica
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<PhysicalActivity> GetPhysicalActivitiesByDate(DateTime date)
        {
            return PhysicalActivityList
                .Where(x => x.DateTime.Date == date.Date)
                .ToList();
        }

        /// <summary>
        /// Retorna as atividades físicas que aconteceram entre um intervalo específico
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static List<PhysicalActivity> GetPhysicalActivitiesByDateRange(DateTime startDate, DateTime endDate)
        {
            return PhysicalActivityList
                .Where(x => x.DateTime >= startDate && x.DateTime <= endDate)
                .ToList();
        }

        #endregion

        /// <summary>
        /// Seta as informações de usuário
        /// </summary>
        /// <param name="userInfo"></param>
        public static void SetUserInfo(UserInfo userInfo)
        {
            ViewServices.AppDataManager.SetItem(DataItems.UserInfo, userInfo);
            _userInfo = userInfo;
        }

        /// <summary>
        /// Limpa o objeto UserInfo do dispositivo
        /// </summary>
        /// <param name="userInfo"></param>
        public static void CleanUserInfo()
        {
            ViewServices.AppDataManager.SetItem(DataItems.UserInfo, null);
            _userInfo = null;
        }

        /// <summary>
        /// Obtém o objetivo de consumo energético atual
        /// </summary>
        /// <returns></returns>
        public static double GetEnergeticDiaryObjective()
        {
            if (AppConfigHelperClass.EnergeticUnit == EnergeticUnit.kJ)
                return UserInfo.KiloCaloriesDiaryObjective * Constants.kcalToKJFactor;
            else
                return UserInfo.KiloCaloriesDiaryObjective;
        }

        /// <summary>
        /// Seta os items da TBCA na memória do dispositivo
        /// </summary>
        /// <param name="tbcaItems"></param>
        public static void SetTbcaItems(List<TbcaItem> tbcaItems)
        {
            ViewServices.AppDataManager.SetItem(DataItems.TbcaFoodItems, tbcaItems);
            _tbcaFoodItems = tbcaItems;
        }

        /// <summary>
        /// Seta os items da TACO na memória do dispositivo
        /// </summary>
        /// <param name="tacoItems"></param>
        public static void SetTacoItems(List<TacoItem> tacoItems)
        {
            ViewServices.AppDataManager.SetItem(DataItems.TacoFoodItems, tacoItems);
            _tacoFoodItems = tacoItems;
        }

        /// <summary>
        /// Calcular idade a partir da data de nascimento
        /// </summary>
        /// <param name="bornDate"></param>
        /// <returns></returns>
        public static int GetAge(DateTime bornDate)
        {
            TimeSpan dif = DateTime.Now - bornDate;
            int age = (int)(dif.TotalDays / 365.25);
            return age;
        }

        /// <summary>
        /// Calculo calorias basal
        /// </summary>
        /// <param name="age"></param>
        /// <param name="genero"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static double GetBasalDailyCalories(int age, string genero, double weight)
        {
            double basalCalories = -1;

            if (genero.Equals("Masculine"))
            {
                if (age < 3)
                {
                    basalCalories = (59.512 * UserInfo.Weight) - 30.4;
                }
                else if (age >= 3 && age < 10)
                {
                    basalCalories = (22.706 * UserInfo.Weight) + 504.3;
                }
                else if (age >= 10 && age < 18)
                {
                    basalCalories = (17.686 * UserInfo.Weight) + 658.2;
                }
                else if (age >= 18 && age < 30)
                {
                    basalCalories = (15.057 * UserInfo.Weight) + 692.2;
                }
                else if (age >= 30 && age < 60)
                {
                    basalCalories = (11.472 * UserInfo.Weight) + 873.1;
                }
                else if (age >= 60)
                {
                    basalCalories = (11.711 * UserInfo.Weight) + 587.7;
                }
            }
            else if (genero.Equals("Feminine"))
            {
                if (age < 3)
                {
                    basalCalories = (58.317 * UserInfo.Weight) - 31.1;
                }
                else if (age >= 3 && age < 10)
                {
                    basalCalories = (20.315 * UserInfo.Weight) + 485.9;
                }
                else if (age >= 10 && age < 18)
                {
                    basalCalories = (13.384 * UserInfo.Weight) + 692.6;
                }
                else if (age >= 18 && age < 30)
                {
                    basalCalories = (14.818 * UserInfo.Weight) + 486.6;
                }
                else if (age >= 30 && age < 60)
                {
                    basalCalories = (8.126 * UserInfo.Weight) + 845.6;
                }
                else if (age >= 60)
                {
                    basalCalories = (9.082 * UserInfo.Weight) + 658.5;
                }
            }
            else
            {
                basalCalories = 0.00;
            }

            return basalCalories;
        }

        /// <summary>
        /// Calculo de gasto diario calorias conforme nivel atividade fisica
        /// </summary>
        /// <param name="basalDailyCalories"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static double GetDailyKiloCaloriesBurn(double basalDailyCalories, string activity, string genero)
        {
            double dailyKiloCaloriesBurn = 0;

            switch (activity)
            {
                case ("LightActivity"):
                    if (genero == "Masculine")
                        dailyKiloCaloriesBurn = basalDailyCalories * 1.55;
                    else
                        dailyKiloCaloriesBurn = basalDailyCalories * 1.56;
                    break;
                case ("ModeratelyActive"):
                    if (genero == "Masculine")
                        dailyKiloCaloriesBurn = basalDailyCalories * 1.78;
                    else
                        dailyKiloCaloriesBurn = basalDailyCalories * 1.64;
                    break;
                case ("VigorouslyActive"):
                    if (genero == "Masculine")
                        dailyKiloCaloriesBurn = basalDailyCalories * 2.10;
                    else
                        dailyKiloCaloriesBurn = basalDailyCalories * 1.82;
                    break;

            }

            return dailyKiloCaloriesBurn;

        }

        /// <summary>
        /// Calculo de ingestao de calorias diarias conforme objetivo do usuario
        /// </summary>
        /// <param name="dailyKiloCaloriesBurn"></param>
        /// <param name="objective"></param>
        /// <returns></returns>
        public static double GetDailyKiloCaloriesGoal(double dailyKiloCaloriesBurn, string objective)
        {
            double dailyKiloCaloriesGoal = 0;

            switch (objective)
            {
                case ("Reduce"):
                    dailyKiloCaloriesGoal = dailyKiloCaloriesBurn - 500;
                    break;
                case ("Maintain"):
                    dailyKiloCaloriesGoal = dailyKiloCaloriesBurn;
                    break;
                case ("Fatten"):
                    dailyKiloCaloriesGoal = dailyKiloCaloriesBurn + 500;
                    break;

            }

            return dailyKiloCaloriesGoal;

        }

        /// <summary>
        /// Seta no dispositivo o token de acesso aos recursos da API do NutriLens
        /// </summary>
        /// <param name="token"></param>
        public static void SetNutriLensApiToken(string token)
        {
            token = token.Replace("\"", string.Empty);
            ViewServices.AppDataManager.SetItem(DataItems.NutriLensApiToken, token);
            _nutriLensApiToken = token;
        }

        public static ObservableCollection<TbcaItem> TbcaItems { get; set; }

        public static ObservableCollection<TacoItem> TacoItems { get; set; }

        public static void GetStringTacoItemsBySimpleFoodItemsV2(List<SimpleFoodItem> simpleFoodItems, out List<FoodItem> foodItems)
        {
            List<TacoItem> detectados = new();
            foodItems = new List<FoodItem>();

            if (TacoFoodItems == null || TacoFoodItems.Count == 0)
            {
                List<TacoItem> tacoItems = DaoHelperClass.GetTacoItemsList();
                SetTacoItems(tacoItems.OrderBy(x => x.Nome).ToList());
            }

            TacoFoodItemParseHelperClass.TacoFoodItems = TacoFoodItems;

            // Verifica se o nome do alimento identificado esta com nome composto tipo Arroz Grego, se sim, Busca por Arroz Grego e após busca apenas por Arroz
            foreach (SimpleFoodItem simpleFoodItem in simpleFoodItems)
            {
                FoodItem tacoFoodItem = TacoFoodItemParseHelperClass.Parse(simpleFoodItem);
                foodItems.Add(tacoFoodItem);
            }
        }

        public static TbcaItem GetTbcaFoodItem(TbcaItem tbca, string portion)
        {
            double portionDouble = double.Parse(portion);

            tbca.UmidadeG = (tbca.UmidadeG * portionDouble / 100);
            tbca.CarboidratoTotal = (tbca.CarboidratoTotal * portionDouble) / 100;
            tbca.CarboidratoDisponivel = (tbca.CarboidratoDisponivel * portionDouble) / 100;
            tbca.Proteina = (tbca.Proteina * portionDouble) / 100;
            tbca.Lipidios = (tbca.Lipidios * portionDouble) / 100;
            tbca.FibraAlimentar = (tbca.FibraAlimentar * portionDouble) / 100;
            tbca.Alcool = (tbca.Alcool * portionDouble) / 100;
            tbca.Cinzas = (tbca.Cinzas * portionDouble) / 100;
            tbca.Colesterol = (tbca.Colesterol * portionDouble) / 100;
            tbca.AcidosGraxosSaturados = (tbca.AcidosGraxosSaturados * portionDouble) / 100;
            tbca.AcidosGraxosMonoinsaturados = (tbca.AcidosGraxosMonoinsaturados * portionDouble) / 100;
            tbca.AcidosGraxosPolinsaturados = (tbca.AcidosGraxosPolinsaturados * portionDouble) / 100;
            tbca.AcidosGraxosTrans = (tbca.AcidosGraxosTrans * portionDouble) / 100;
            tbca.Calcio = (tbca.Calcio * portionDouble) / 100;
            tbca.Ferro = (tbca.Ferro * portionDouble) / 100;
            tbca.Sodio = (tbca.Sodio * portionDouble) / 100;
            tbca.Magnesio = (tbca.Magnesio * portionDouble) / 100;
            tbca.Fosforo = (tbca.Fosforo * portionDouble) / 100;
            tbca.Potassio = (tbca.Potassio * portionDouble) / 100;
            tbca.Zinco = (tbca.Zinco * portionDouble) / 100;
            tbca.Cobre = (tbca.Cobre * portionDouble) / 100;
            tbca.Selenio = (tbca.Selenio * portionDouble) / 100;
            tbca.VitaminaARae = (tbca.VitaminaARae * portionDouble) / 100;
            tbca.VitaminaD = (tbca.VitaminaD * portionDouble) / 100;
            tbca.VitaminaE = (tbca.VitaminaE * portionDouble) / 100;
            tbca.Tiamina = (tbca.Tiamina * portionDouble) / 100;
            tbca.Riboflavina = (tbca.Riboflavina * portionDouble) / 100;
            tbca.Niacina = (tbca.Niacina * portionDouble) / 100;
            tbca.VitaminaB6 = (tbca.VitaminaB6 * portionDouble) / 100;
            tbca.VitaminaB12 = (tbca.VitaminaB12 * portionDouble) / 100;
            tbca.VitaminaC = (tbca.VitaminaC * portionDouble) / 100;
            tbca.EquivalenteDeFolato = (tbca.EquivalenteDeFolato * portionDouble) / 100;
            tbca.SalDeAdicao = (tbca.SalDeAdicao * portionDouble) / 100;
            tbca.AcucarDeAdicao = (tbca.AcucarDeAdicao * portionDouble) / 100;

            return tbca;
        }

        public static TacoItem GetTacoFoodItem(TacoItem taco, string portion)
        {
            double portionDouble = double.Parse(portion);

            taco.Umidade = (taco.GetValue(nameof(taco.Umidade)) * portionDouble / 100);
            taco.Proteina = (taco.GetValue(nameof(taco.Proteina)) * portionDouble / 100);
            taco.Lipideos = (taco.GetValue(nameof(taco.Lipideos)) * portionDouble / 100);
            taco.Colesterol = (taco.GetValue(nameof(taco.Colesterol)) * portionDouble / 100);
            taco.Carboidrato = (taco.GetValue(nameof(taco.Carboidrato)) * portionDouble) / 100;
            taco.FibraAlimentar = (taco.GetValue(nameof(taco.FibraAlimentar)) * portionDouble / 100);
            taco.Cinzas = (taco.GetValue(nameof(taco.Cinzas)) * portionDouble / 100);
            taco.Calcio = (taco.GetValue(nameof(taco.Calcio)) * portionDouble / 100);
            taco.Magnesio = (taco.GetValue(nameof(taco.Magnesio)) * portionDouble / 100);
            taco.Manganes = (taco.GetValue(nameof(taco.Manganes)) * portionDouble / 100);
            taco.Fosforo = (taco.GetValue(nameof(taco.Fosforo)) * portionDouble / 100);
            taco.Ferro = (taco.GetValue(nameof(taco.Ferro)) * portionDouble / 100);
            taco.Sodio = (taco.GetValue(nameof(taco.Sodio)) * portionDouble / 100);
            taco.Potassio = (taco.GetValue(nameof(taco.Potassio)) * portionDouble / 100);
            taco.Cobre = (taco.GetValue(nameof(taco.Cobre)) * portionDouble / 100);
            taco.Zinco = (taco.GetValue(nameof(taco.Zinco)) * portionDouble / 100);
            taco.Retinol = (taco.GetValue(nameof(taco.Retinol)) * portionDouble / 100);
            taco.RE = (taco.GetValue(nameof(taco.RE)) * portionDouble / 100);
            taco.RAE = (taco.GetValue(nameof(taco.RAE)) * portionDouble / 100);
            taco.Tiamina = (taco.GetValue(nameof(taco.Tiamina)) * portionDouble / 100);
            taco.Riboflavina = (taco.GetValue(nameof(taco.Riboflavina)) * portionDouble / 100);
            taco.Piridoxina = (taco.GetValue(nameof(taco.Piridoxina)) * portionDouble / 100);
            taco.Niacina = (taco.GetValue(nameof(taco.Niacina)) * portionDouble / 100);
            taco.VitaminaC = (taco.GetValue(nameof(taco.VitaminaC)) * portionDouble / 100);

            return taco;
        }

        public static List<FoodItem> DetectedFoodItems { get; set; } = new List<FoodItem>();

        public static string NewFoodPicturePath { get; set; }

        public static Meal MealToEdit { get; set; }

        public static MealListClass FilteredMealList { get; set; }
    }
}

