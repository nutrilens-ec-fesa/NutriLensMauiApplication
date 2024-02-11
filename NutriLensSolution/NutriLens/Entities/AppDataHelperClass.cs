using ExceptionLibrary;
using Newtonsoft.Json;
using MongoDB.Bson.IO;
using NutriLens.Models;
using NutriLens.Services;
using NutriLensClassLibrary.Entities;
using NutriLensClassLibrary.Models;

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
            /// Token da API do NutriLens
            /// </summary>
            NutriLensApiToken
        }

        private static List<Meal> _mealList;
        private static UserInfo _userInfo;
        private static List<TbcaItem> _tbcaFoodItemsItems;
        private static string _nutriLensApiToken;

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
                if (_tbcaFoodItemsItems == null)
                {
                    try
                    {
                        _tbcaFoodItemsItems = ViewServices.AppDataManager.GetItem<List<TbcaItem>>(DataItems.TbcaFoodItems);
                    }
                    catch (NotFoundException)
                    {
                        _tbcaFoodItemsItems = new List<TbcaItem>();
                    }
                }

                return _tbcaFoodItemsItems;
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
                .Where(x => x.DateTime >= startDate && x.DateTime <= endDate)
                .ToList();
        }

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
            _tbcaFoodItemsItems = tbcaItems;
        }

        public static int GetAge(DateTime bornDate)
        {
            TimeSpan dif = DateTime.Now - bornDate;
            int age = (int)(dif.TotalDays / 365.25);
            return age;
        }

        public static double GetBasalDailyCalories (int age, string genero, double weight)
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

        public static double GetDailyKiloCaloriesBurn (double basalDailyCalories, string activity)
        {
            double dailyKiloCaloriesBurn = 0;

            switch (activity)
            {
                case ("LightActivity"):
                    dailyKiloCaloriesBurn = basalDailyCalories * 1.55;
                    break;
                case ("ModeratelyActive"):
                    dailyKiloCaloriesBurn = basalDailyCalories * 1.84;
                    break;
                case ("VigorouslyActive"):
                    dailyKiloCaloriesBurn = basalDailyCalories * 2.2;
                    break;

            }

            return dailyKiloCaloriesBurn;

        }


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

        public static List<RecognizedImageInfoModel> GetRecognizedImageInfoModel(string resultadoAnalise) 
        { 
            int ini = resultadoAnalise.IndexOf('[');
            int fim = resultadoAnalise.IndexOf(']');
            int len = (fim - ini)+1;
            string texto = resultadoAnalise.Substring(ini, len);
            List<RecognizedImageInfoModel> alimentosReconhecidos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RecognizedImageInfoModel>>(texto);
            return alimentosReconhecidos;
        }

        public static string GetRecognizedImageInfoText (List<RecognizedImageInfoModel> alimentosReconhecidos)
        {
            string alimentos = string.Empty;

            foreach(RecognizedImageInfoModel item in alimentosReconhecidos)
            {
                alimentos += item.Item + " - " + item.Quantidade + "g" + '\n';
            }

            return alimentos;
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
    }
}
