using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics.Text;
using MongoDB.Bson.Serialization.Serializers;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using System.Collections.ObjectModel;

namespace NutriLens.ViewModels
{
    public partial class UserConfigPageVm : ObservableObject
    {
        private INavigation _navigation;

        /// <summary>
        /// Objeto de dados do usuário
        /// </summary>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// String do campo de peso
        /// </summary>
        public string WeightEntry { get; set; }

        /// <summary>
        /// String do campo de altura
        /// </summary>
        public string HeightEntry { get; set; }

        /// <summary>
        /// Booleana que indica se a unidade kcal está habilitada
        /// </summary>
        public bool KcalEnabled { get; set; }

        /// <summary>
        /// Booleana que indica se a unidade kJ está habilitada
        /// </summary>
        public bool KJEnabled { get; set; }

        /// <summary>
        /// Indica o índice de sexo selecionado pelo usuário
        /// </summary>
        public int GenderIndex { get; set; }

        /// <summary>
        /// Data de nascimento na tela
        /// </summary>
        public DateTime Born { get; set; }

        /// <summary>
        /// Indica o índice de hábito de atividade física selecionado pelo usuário
        /// </summary>
        public int HabitualPhysicalActivityIndex { get; set; }

        public int DailyKiloCaloriesObjectiveIndex { get; set; }

        public string BasalDailyCalories { get; set; }

        public string DailyKiloCaloriesBurn { get; set; }

        public string DailyKiloCaloriesGoal { get; set; }

        /// <summary>
        /// Picker de gêneros
        /// </summary>
        public ObservableCollection<string> GenderOptions { get; set; }

        /// <summary>
        /// Picker de hábitos de atividade física
        /// </summary>
        public ObservableCollection<string> HabitualPhysicalActivityOptions { get; set; }

        /// <summary>
        /// Picker do objetivo do usuário (manter peso, emagrecer ou engordar)
        /// </summary>
        public ObservableCollection<string> DailyKiloCaloriesObjectiveOptions { get; set; }

        public UserConfigPageVm(INavigation navigation)
        {
            _navigation = navigation;
            UserInfo = AppDataHelperClass.UserInfo;
            GenderIndex = (int)UserInfo.Gender;
            KcalEnabled = AppConfigHelperClass.EnergeticUnit == EnergeticUnit.kcal;
            KJEnabled = !KcalEnabled;
            WeightEntry = UserInfo.Weight.ToString("0.00");
            HeightEntry = UserInfo.Height.ToString("0.00");

            HabitualPhysicalActivityOptions = new ObservableCollection<string>();
            DailyKiloCaloriesObjectiveOptions = new ObservableCollection<string>();

            GenderOptions = new ObservableCollection<string>()
            {
                "Não informado",
                "Masculino",
                "Feminino"
            };

            foreach (HabitualPhysicalActivity habitualPhysicalActivity in (HabitualPhysicalActivity[])Enum.GetValues(typeof(HabitualPhysicalActivity)))
            {
                string toAdd = string.Empty;

                switch (habitualPhysicalActivity)
                {
                    case HabitualPhysicalActivity.LightActivity:
                        toAdd = "Sedentário(a) ou faz atividade física leve";
                        break;
                    case HabitualPhysicalActivity.ModeratelyActive:
                        toAdd = "Faz atividade física moderada";
                        break;
                    case HabitualPhysicalActivity.VigorouslyActive:
                        toAdd = "Faz atividade física intensa";
                        break;
                }

                HabitualPhysicalActivityOptions.Add(toAdd);
            }

            HabitualPhysicalActivityIndex = (int)UserInfo.HabitualPhysicalActivity;

            foreach (DailyKiloCaloriesObjective dailyKiloCaloriesObjective in (DailyKiloCaloriesObjective[])Enum.GetValues(typeof(DailyKiloCaloriesObjective)))
            {
                string toAdd = string.Empty;

                switch (dailyKiloCaloriesObjective)
                {
                    case DailyKiloCaloriesObjective.Reduce:
                        toAdd = "Reduzir o peso";
                        break;
                    case DailyKiloCaloriesObjective.Maintain:
                        toAdd = "Manter o peso";
                        break;
                    case DailyKiloCaloriesObjective.Fatten:
                        toAdd = "Aumentar o peso";
                        break;
                }

                DailyKiloCaloriesObjectiveOptions.Add(toAdd);
            }

            DailyKiloCaloriesObjectiveIndex = (int)UserInfo.DailyKiloCaloriesObjective;


        }

        [RelayCommand]
        private async Task SaveConfigs()
        {
            if (EntitiesHelperClass.ParseDoubleValue(WeightEntry, out double weightValue))
                UserInfo.Weight = weightValue;
            else
            {
                await ViewServices.PopUpManager.PopErrorAsync("Peso informado inválido!");
                return;
            }

            if (EntitiesHelperClass.ParseDoubleValue(HeightEntry, out double heightValue))
                UserInfo.Height = heightValue;
            else
            {
                await ViewServices.PopUpManager.PopErrorAsync("Altura informada inválida!");
                return;
            }

            UserInfo.Gender = (Gender)GenderIndex;
            UserInfo.HabitualPhysicalActivity = (HabitualPhysicalActivity)HabitualPhysicalActivityIndex;
            UserInfo.DailyKiloCaloriesObjective = (DailyKiloCaloriesObjective)DailyKiloCaloriesObjectiveIndex;

            DateTime born = UserInfo.BornDate;
            int age = AppDataHelperClass.GetAge(born);
            string gender = UserInfo.Gender.ToString();
            double basal = AppDataHelperClass.GetBasalDailyCalories(age, gender);
            BasalDailyCalories = basal.ToString("0.00");

            string activity = UserInfo.HabitualPhysicalActivity.ToString();
            double dailyKiloCaloriesBurn = AppDataHelperClass.GetDailyKiloCaloriesBurn(basal, activity);
            DailyKiloCaloriesBurn = dailyKiloCaloriesBurn.ToString("0.00");

            string objective = UserInfo.DailyKiloCaloriesObjective.ToString();
            DailyKiloCaloriesGoal = AppDataHelperClass.GetDailyKiloCaloriesGoal(dailyKiloCaloriesBurn, objective).ToString("0.00");

            bool userUpdatedSuccesfully = false;

            EntitiesHelperClass.ShowLoading("Atualizando usuário");

            await Task.Run(() => userUpdatedSuccesfully = DaoHelperClass.UpdateUserInfo(UserInfo));

            EntitiesHelperClass.CloseLoading();

            if (userUpdatedSuccesfully)
            {
                AppDataHelperClass.SetUserInfo(UserInfo);

                if (KcalEnabled)
                    AppConfigHelperClass.SetEnergeticUnit(EnergeticUnit.kcal);
                else
                    AppConfigHelperClass.SetEnergeticUnit(EnergeticUnit.kJ);

                await ViewServices.PopUpManager.PopInfoAsync("Configurações salvas com sucesso!");

                await _navigation.PopAsync();
            }
            else
                await ViewServices.PopUpManager.PopErrorAsync("Houve algum problema para atualizar as informações de usuário. Aguarde uns instantes e tente novamente.");
        }


        [RelayCommand]
        private async Task CaloricSuggestChanged()
        {
            DateTime born = UserInfo.BornDate;
            int age = AppDataHelperClass.GetAge(born);
            UserInfo.Gender = (Gender)GenderIndex;
            string gender = UserInfo.Gender.ToString();
            double basal = AppDataHelperClass.GetBasalDailyCalories(age, gender);
            BasalDailyCalories = basal.ToString("0.00");
            OnPropertyChanged(nameof(BasalDailyCalories));

            UserInfo.HabitualPhysicalActivity = (HabitualPhysicalActivity)HabitualPhysicalActivityIndex;
            string activity = UserInfo.HabitualPhysicalActivity.ToString();
            double dailyKiloCaloriesBurn = AppDataHelperClass.GetDailyKiloCaloriesBurn(basal, activity);
            DailyKiloCaloriesBurn = dailyKiloCaloriesBurn.ToString("0.00");
            OnPropertyChanged(nameof(DailyKiloCaloriesBurn));

            UserInfo.DailyKiloCaloriesObjective = (DailyKiloCaloriesObjective)DailyKiloCaloriesObjectiveIndex;
            string objective = UserInfo.DailyKiloCaloriesObjective.ToString();
            DailyKiloCaloriesGoal = AppDataHelperClass.GetDailyKiloCaloriesGoal(dailyKiloCaloriesBurn, objective ).ToString("0.00");
            OnPropertyChanged(nameof(DailyKiloCaloriesGoal));
        }

    }

}
