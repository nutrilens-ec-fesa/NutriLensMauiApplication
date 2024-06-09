using ExceptionLibrary;
using Newtonsoft.Json;
using NutriLens.Models;
using NutriLens.Services;
using NutriLens.Views.Popups;
using NutriLensClassLibrary.Entities;
using NutriLensClassLibrary.Models;
using Org.Apache.Http.Conn;
using Xamarin.KotlinX.Coroutines;
using ZXing.QrCode.Internal;

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
            PhysicalActivityList,

            /// <summary>
            /// Hash da última tabela TACO baixada
            /// </summary>
            TacoHash,

            /// <summary>
            /// Hash das refeições do usuário na base de dados
            /// </summary>
            MealListHash,
        }

        private static List<Meal> _mealList;
        private static UserInfo _userInfo;
        private static List<TbcaItem> _tbcaFoodItems;
        private static List<TacoItem> _tacoFoodItems;
        private static List<PhysicalActivity> _physicalActivityList;
        private static string _nutriLensApiToken;
        private static List<BarcodeItemEntry> _barcodeItems;
        private static Random _random = new Random();

        private static List<TacoItem> breakfastTacoItems = TacoFoodItems.Where(x =>
                        x.TacoCategory == TacoCategory.CEREAIS_E_DERIVADOS ||
                        x.TacoCategory == TacoCategory.FRUTAS_E_DERIVADOS ||
                        x.TacoCategory == TacoCategory.LEITE_E_DERIVADOS ||
                        x.TacoCategory == TacoCategory.BEBIDAS ||
                        x.TacoCategory == TacoCategory.OVOS_E_DERIVADOS).ToList();

        private static List<TacoItem> morningSnackTacoItems = TacoFoodItems.Where(x =>
                        x.TacoCategory == TacoCategory.FRUTAS_E_DERIVADOS ||
                        x.TacoCategory == TacoCategory.LEITE_E_DERIVADOS).ToList();

        private static List<TacoItem> afternoonSnackTacoItems = TacoFoodItems.Where(x =>
                    x.TacoCategory == TacoCategory.FRUTAS_E_DERIVADOS ||
                    x.TacoCategory == TacoCategory.PRODUTOS_ACUCARADOS ||
                    x.TacoCategory == TacoCategory.NOZES_E_SEMENTES).ToList();

        private static List<TacoItem> LunchAndMealTacoItems = TacoFoodItems.Where(x =>
                    x.TacoCategory == TacoCategory.CEREAIS_E_DERIVADOS ||
                    x.TacoCategory == TacoCategory.FRUTAS_E_DERIVADOS ||
                    x.TacoCategory == TacoCategory.LEGUMINOSAS_E_DERIVADOS ||
                    x.TacoCategory == TacoCategory.CARNES_E_DERIVADOS ||
                    x.TacoCategory == TacoCategory.PESCADOS_E_FRUTOS_DO_MAR ||
                    x.TacoCategory == TacoCategory.ALIMENTOS_PREPARADOS ||
                    x.TacoCategory == TacoCategory.PRODUTOS_ACUCARADOS ||
                    x.TacoCategory == TacoCategory.OVOS_E_DERIVADOS).ToList();

        public static string NewFoodPicturePath { get; set; }

        public static Meal MealToEdit { get; set; }

        public static MealListClass FilteredMealList { get; set; }

        public static List<FoodItem> DetectedFoodItems { get; set; } = new List<FoodItem>();

        public static bool AddTacoItemRequested { get; set; }

        public static TacoItem AddedTacoItem { get; set; }

        /// <summary>
        /// Lista de refeições
        /// </summary>
        private static List<Meal> MealList
        {
            get
            {
                if (_mealList == null)
                {
                    // Tenta obter as refeições vinculadas ao usuário
                    try
                    {
                        _mealList = ViewServices.AppDataManager.GetItem<List<Meal>>(nameof(DataItems.MealList) + UserInfo.Id);
                        _mealList = _mealList
                            .Where(x => x.FoodItems != null && x.FoodItems.Count > 0)
                            .ToList();
                    }
                    catch (NotFoundException)
                    {
                        // Caso não encontre, ou realmente não há, ou as refeições estão no modelo antigo
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

        /// <summary>
        /// Indica o tipo de refeição, os valores correspondem a hora em que a refeição normalmente
        /// é feita
        /// </summary>
        public enum MealKind
        {
            Breakfast = 8,
            MorningSnack = 10,
            Lunch = 12,
            AfternoonSnack = 15,
            Dinner = 18
        }

        #region Meal methods

        /// <summary>
        /// Adiciona uma nova refeição
        /// </summary>
        /// <param name="meal"></param>
        public static async void AddMeal(Meal meal)
        {
            meal.UserInfoId = UserInfo.Id;
            MealList.Add(meal);
            ViewServices.AppDataManager.SetItem(nameof(DataItems.MealList) + UserInfo.Id, MealList);

            bool mealSyncronized = false;

            EntitiesHelperClass.ShowLoading("Sincronizando refeição em nuvem");

            await Task.Run(() =>
            {
                mealSyncronized = DaoHelperClass.InsertMeals(new List<Meal> { meal });
                HashItem cloudUserMealsHash = DaoHelperClass.GetUserMealsHash();
                SetUserMealsHash(cloudUserMealsHash.Hash);
            });

            await EntitiesHelperClass.CloseLoading();
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

        /// <summary>
        /// Verifica se já existe alguma lista de refeições específicas de usuário, e
        /// caso não haja, e hajam refeições sem usuário associado, vincula todas as refeições
        /// ao usuário atualmente logado
        /// </summary>
        public static void CheckUndefinedUserMeals()
        {
            // Obtém as chaves cadastradas
            List<string> registeredKeys = ViewServices.AppDataManager.GetAllKeys();

            // Verifica se há chaves cadastradas
            if (registeredKeys.Count > 0)
            {
                // Obtém todas as chaves de refeições
                List<string> mealsKeys = registeredKeys.Where(x => x.StartsWith(nameof(DataItems.MealList)))
                    .ToList();

                // Se houver apenas uma, e ela não conter a concatenação do UserInfoId, 
                // é necessário obter as refeições e associar ao usuário
                if (mealsKeys.Count == 1 && mealsKeys[0] == nameof(DataItems.MealList))
                {
                    List<Meal> meals = GetAllMeals();

                    foreach (Meal meal in meals)
                    {
                        meal.UserInfoId = UserInfo.Id;
                    }

                    SaveChangesOnMeals();

                    ViewServices.AppDataManager.DeleteItem(DataItems.MealList);
                }
            }
        }

        public static DateTime GetFirstMealDateTime()
        {
            List<Meal> meals = GetAllMeals();

            if (meals.Count == 0)
                return DateTime.Now;
            else
                return meals.OrderBy(x => x.DateTime).ToList()[0].DateTime;
        }

        public static async Task RemoveMeal(Meal meal)
        {
            MealList.Remove(meal);
            ViewServices.AppDataManager.SetItem(nameof(DataItems.MealList) + UserInfo.Id, MealList);

            bool mealSyncronized = false;

            EntitiesHelperClass.ShowLoading("Sincronizando refeição em nuvem");

            await Task.Run(() =>
            {
                mealSyncronized = DaoHelperClass.RemoveMeal(meal);
                HashItem cloudUserMealsHash = DaoHelperClass.GetUserMealsHash();
                SetUserMealsHash(cloudUserMealsHash.Hash);
            });

            await EntitiesHelperClass.CloseLoading();
        }

        public static async Task UpdateMeal(Meal meal)
        {
            bool mealSyncronized = false;

            EntitiesHelperClass.ShowLoading("Sincronizando refeição em nuvem");

            await Task.Run(() =>
            {
                mealSyncronized = DaoHelperClass.UpdateMeal(meal);
                HashItem cloudUserMealsHash = DaoHelperClass.GetUserMealsHash();
                SetUserMealsHash(cloudUserMealsHash.Hash);
            });

            await EntitiesHelperClass.CloseLoading();

            SaveChangesOnMeals();
        }

        public static async Task RemoveAllMeals()
        {
            _mealList = new List<Meal>();
            ViewServices.AppDataManager.DeleteItem(nameof(DataItems.MealList) + UserInfo.Id);

            bool mealSyncronized = false;

            EntitiesHelperClass.ShowLoading("Sincronizando refeições em nuvem");

            await Task.Run(() =>
            {
                mealSyncronized = DaoHelperClass.RemoveAllMeals();
                HashItem cloudUserMealsHash = DaoHelperClass.GetUserMealsHash();
                SetUserMealsHash(cloudUserMealsHash.Hash);
            });

            await EntitiesHelperClass.CloseLoading();
        }

        public static void SaveChangesOnMeals()
        {
            ViewServices.AppDataManager.SetItem(nameof(DataItems.MealList) + UserInfo.Id, MealList);
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

        /// <summary>
        /// Limpa todas as variáveis, para que o novo login não acesse coisas do usuário anterior
        /// </summary>
        public static void CleanSessionInfo()
        {
            // Limpeza de variáveis privadas
            _mealList = null;
            _userInfo = null;
            _tbcaFoodItems = null;
            _tacoFoodItems = null;
            _physicalActivityList = null;
            _nutriLensApiToken = string.Empty;
            SetUserMealsHash(string.Empty);

            // Limpeza de propriedades
            NewFoodPicturePath = string.Empty;
            MealToEdit = null;
            FilteredMealList = null;
            DetectedFoodItems = new List<FoodItem>();
        }

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

        public static bool GetIfMacroNutrientsAreOnAnvisaLimits()
        {
            AnvisaLimits anvisaLimits = AnvisaLimits.GetAnvisaLimits();

            return
                UserInfo.DailyCarbohydrateGoal == anvisaLimits.Carboidratos &&
                UserInfo.DailyProteinGoal == anvisaLimits.Proteinas &&
                UserInfo.DailyFatGoal == anvisaLimits.GordurasTotais &&
                UserInfo.DailyFiberGoal == anvisaLimits.FibraAlimentar &&
                UserInfo.DailySodiumGoal == anvisaLimits.Sodio &&
                UserInfo.DailyCholesterolGoal == anvisaLimits.Colesterol &&
                UserInfo.DailyCalciumGoal == anvisaLimits.Calcio &&
                UserInfo.DailyIronGoal == anvisaLimits.Ferro;
        }


        public static async Task CheckTacoUpdate()
        {
            await Task.Run(() =>
            {
                bool tacoDownloaded = TacoFoodItems != null && TacoFoodItems.Count > 0;
                string cloudTacoHash = DaoHelperClass.GetTacoHash();

                if (!tacoDownloaded)
                {
                    List<TacoItem> tacoItems = DaoHelperClass.GetTacoItemsList();
                    SetTacoItems(tacoItems.OrderBy(x => x.Nome).ToList());
                    SetTacoHash(cloudTacoHash);
                }
                else
                {
                    string localTacoHash = GetTacoHash();

                    if (cloudTacoHash != localTacoHash)
                    {
                        List<TacoItem> tacoItems = DaoHelperClass.GetTacoItemsList();
                        SetTacoItems(tacoItems.OrderBy(x => x.Nome).ToList());
                        SetTacoHash(cloudTacoHash);
                    }
                }
            });
        }

        public static string GetTacoHash()
        {
            try
            {
                return ViewServices.AppDataManager.GetItem<string>(DataItems.TacoHash);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void SetTacoHash(string hash)
        {
            ViewServices.AppDataManager.SetItem(DataItems.TacoHash, hash);
        }

        public static string GetUserMealsHash()
        {
            try
            {
                return ViewServices.AppDataManager.GetItem<string>(DataItems.MealListHash);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void SetUserMealsHash(string hash)
        {
            ViewServices.AppDataManager.SetItem(DataItems.MealListHash, hash);
        }

        public static async Task CheckCloudMeals()
        {
            await Task.Run(() =>
            {
                HashItem cloudUserMealsHash = DaoHelperClass.GetUserMealsHash();

                if (!string.IsNullOrEmpty(cloudUserMealsHash.Hash) && cloudUserMealsHash.ItemsCount > 0)
                {
                    if (GetUserMealsHash() != cloudUserMealsHash.Hash)
                    {
                        List<Meal> meals = DaoHelperClass.GetAllUserMeals();
                        ViewServices.AppDataManager.SetItem(nameof(DataItems.MealList) + UserInfo.Id, meals);
                        _mealList = GetAllMeals();
                        SetUserMealsHash(cloudUserMealsHash.Hash);
                    }
                }
                else if (MealList.Count > 0)
                {
                    DaoHelperClass.InsertMeals(MealList);
                    cloudUserMealsHash = DaoHelperClass.GetUserMealsHash();
                    SetUserMealsHash(cloudUserMealsHash.Hash);
                }
            });
        }

        /// <summary>
        /// Cria refeilções mockadas
        /// </summary>
        /// <returns></returns>
        public static async Task CreateMockMeals()
        {
            // Array com os horários base para as refeições
            int[] mealHours = new int[]
            {
                (int)MealKind.Breakfast,
                (int)MealKind.MorningSnack,
                (int)MealKind.Lunch,
                (int)MealKind.AfternoonSnack,
                (int)MealKind.Dinner
            };

            // Índice do horário de refeição
            int mealHourIndex = 0;

            // Data e hora do início do mock (6 meses para trás)
            DateTime dateTime = DateTime.Now.AddMonths(-6).Date;

            List<Meal> randomizedMeals = new List<Meal>();

            // Enquanto a data atual foi maior que a data e hora de controle
            while (DateTime.Now > dateTime)
            {
                // Quantidade de segundos que serão deslocados do horário de refeição planejado
                // Utilizado para simular uma situação real
                int randomizedSeconds = _random.Next(0, 1800);

                // Caso o número de segundos tenha sido par, desloca para cima
                // Caso o número de segundos tenha sido ímpar, descola para baixo
                randomizedSeconds = randomizedSeconds % 2 == 0 ? randomizedSeconds : randomizedSeconds * (-1);

                DateTime mealDateTime = dateTime.Date.AddHours(mealHours[mealHourIndex]).AddSeconds(randomizedSeconds);

                if (mealDateTime > DateTime.Now)
                    break;

                // Refeição aleatória
                Meal randomizedMeal = new Meal()
                {
                    // Data com o deslocamento aleatório em segundos
                    DateTime = mealDateTime,
                    UserInfoId = UserInfo.Id,
                    Name = "Refeição",
                    FoodItems = await GetRandomFoodItems((MealKind)mealHours[mealHourIndex++]),
                };

                // Reset da váriavel de índice
                if (mealHourIndex == mealHours.Length)
                {
                    mealHourIndex = 0;
                    dateTime = mealDateTime.Date.AddDays(1);
                }

                randomizedMeals.Add(randomizedMeal);
            }

            _mealList = randomizedMeals;

            ViewServices.AppDataManager.SetItem(nameof(DataItems.MealList) + UserInfo.Id, _mealList);

            bool mealSyncronized = false;

            await Task.Run(() =>
            {
                mealSyncronized = DaoHelperClass.InsertMeals(_mealList);
                HashItem cloudUserMealsHash = DaoHelperClass.GetUserMealsHash();
                SetUserMealsHash(cloudUserMealsHash.Hash);
            });

            await ViewServices.PopUpManager.PopInfoAsync("Refeições mocks geradas com sucesso!");
        }

        /// <summary>
        /// Obtém uma lista de FoodItems aleatórios
        /// </summary>
        /// <param name="mealKind"></param>
        /// <returns></returns>
        public static async Task<List<FoodItem>> GetRandomFoodItems(MealKind mealKind)
        {
            // Quantidade de itens alimentícios que terão na refeição
            int foodItemsQuantity = _random.Next(1, 6);

            List<FoodItem> randomizedFoodItems = new();

            List<TacoItem> possibleTacoItems = new List<TacoItem>();

            switch (mealKind)
            {
                case MealKind.Breakfast:
                    possibleTacoItems = breakfastTacoItems;
                    break;
                case MealKind.MorningSnack:
                    possibleTacoItems = morningSnackTacoItems;
                    break;
                case MealKind.AfternoonSnack:
                    possibleTacoItems = afternoonSnackTacoItems;
                    break;
                case MealKind.Lunch:
                case MealKind.Dinner:
                    possibleTacoItems = LunchAndMealTacoItems;
                    break;
            }

            for (int i = 0; i < foodItemsQuantity; i++)
            {
                // Escolhe um número entre 0 a 10, caso seja maior ou igual a 2, indica que será gerado um item da TACO
                // caso seja menor que 2, indica que será gerado um item de código de barras
                // bool tacoItem = _random.Next(0, 10) >= 2;

                FoodItem foodItem = new();

                //if (tacoItem)
                //{
                TacoItem randomTacoItem;

                // Do-while para continuar até selecionar um alimento coerente
                do
                {
                    // Obtém um item aleatório da TACO
                    randomTacoItem = possibleTacoItems.ElementAt(_random.Next(0, possibleTacoItems.Count));
                } while (randomTacoItem.TacoCategory == TacoCategory.CARNES_E_DERIVADOS && randomTacoItem.Nome.ToUpper().Contains("CRU"));

                // Porção aleatória a ser consumida desse alimento
                int portion = _random.Next(50, 151);

                double kcalTaco = Convert.ToDouble(randomTacoItem.EnergiaKcal);
                double kiloCalories = (portion * kcalTaco) / 100;

                foodItem = new FoodItem()
                {
                    Name = randomTacoItem.Nome,
                    Portion = portion.ToString(),
                    KiloCalories = kiloCalories,
                    TacoFoodItem = GetTacoFoodItem(randomTacoItem, portion.ToString())
                };

                randomizedFoodItems.Add(foodItem);
            }

            return randomizedFoodItems;
        }
    }
}

