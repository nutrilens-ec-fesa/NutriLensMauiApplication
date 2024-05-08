using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics.Text;
using ExceptionLibrary;
using MongoDB.Bson.Serialization.Serializers;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLensClassLibrary.Models;
using System.Collections.ObjectModel;
using StringLibrary;
using System.ComponentModel;

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

        public Color GenderColorValidation { get => GenderIndex == 0 ? ColorHelperClass.InvalidFieldColor : ColorHelperClass.ValidFieldColor; }

        public Color BornColorValidation
        {
            get
            {
                return UserInfo.BornDate.Date >= DateTime.Now.Date ? ColorHelperClass.InvalidFieldColor : ColorHelperClass.ValidFieldColor;
            }
        }

        public Color WeightColorValidation
        {
            get
            {
                if (StringFunctions.ParseDoubleValue(WeightEntry, out double weightValue) && weightValue > 0)
                    return ColorHelperClass.ValidFieldColor;
                else
                    return ColorHelperClass.InvalidFieldColor;
            }
        }

        public Color HeightColorValidation
        {
            get
            {
                if (StringFunctions.ParseDoubleValue(HeightEntry, out double heightValue) && heightValue > 0)
                    return ColorHelperClass.ValidFieldColor;
                else
                    return ColorHelperClass.InvalidFieldColor;
            }
        }

        public Color PhysicalActivityColorValidation { get => HabitualPhysicalActivityIndex == 0 ? ColorHelperClass.InvalidFieldColor : ColorHelperClass.ValidFieldColor; }
        
        public Color DailyKiloCaloriesObjectiveColorValidation { get => DailyKiloCaloriesObjectiveIndex == 0 ? ColorHelperClass.InvalidFieldColor : ColorHelperClass.ValidFieldColor; }

        public Color KiloCaloriesDiaryObjectiveColorValidation { get => UserInfo.KiloCaloriesDiaryObjective <= 0 ? ColorHelperClass.InvalidFieldColor : ColorHelperClass.ValidFieldColor; }
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

            if (UserInfo.BornDate == DateTime.MinValue)
                UserInfo.BornDate = DateTime.Now;

            GenderIndex = (int)UserInfo.Gender;
            HabitualPhysicalActivityIndex = (int)UserInfo.HabitualPhysicalActivity;
            DailyKiloCaloriesObjectiveIndex = (int)UserInfo.DailyKiloCaloriesObjective;

            if (GenderIndex <= 0)
                GenderIndex = 0;

            if (HabitualPhysicalActivityIndex <= 0)
                HabitualPhysicalActivityIndex = 0;

            if (DailyKiloCaloriesObjectiveIndex <= 0)
                DailyKiloCaloriesObjectiveIndex = 0;

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

            HabitualPhysicalActivityOptions = new ObservableCollection<string>()
            {
                "Não informado",
                "Sedentário(a) ou faz atividade física leve",
                "Faz atividade física moderada",
                "Faz atividade física intensa"
            };

            DailyKiloCaloriesObjectiveOptions = new ObservableCollection<string>()
            {
                "Não informado",
                "Reduzir o peso",
                "Manter o peso",
                "Aumentar o peso"
            };
        }

        [RelayCommand]
        private void Appearing()
        {
            UpdateUi();
        }

        [RelayCommand]
        private async Task SaveConfigs()
        {
            if (StringFunctions.ParseDoubleValue(WeightEntry, out double weightValue))
                UserInfo.Weight = weightValue;
            else
            {
                await ViewServices.PopUpManager.PopErrorAsync("Peso informado inválido!");
                return;
            }

            if (StringFunctions.ParseDoubleValue(HeightEntry, out double heightValue))
                UserInfo.Height = heightValue;
            else
            {
                await ViewServices.PopUpManager.PopErrorAsync("Altura informada inválida!");
                return;
            }

            if (UserInfo.BornDate == DateTime.MinValue || UserInfo.BornDate.Date == DateTime.Now.Date)
            {
                await ViewServices.PopUpManager.PopErrorAsync("Data de nascimento não informada!");
                return;
            }
            else if (UserInfo.BornDate >= DateTime.Now)
            {
                await ViewServices.PopUpManager.PopErrorAsync("Data de nascimento não pode ser maior que a data atual!");
                return;
            }

            UserInfo.Gender = (Gender)GenderIndex;
            UserInfo.HabitualPhysicalActivity = (HabitualPhysicalActivity)HabitualPhysicalActivityIndex;
            UserInfo.DailyKiloCaloriesObjective = (DailyKiloCaloriesObjective)DailyKiloCaloriesObjectiveIndex;
            UserInfo.Weight = double.Parse(WeightEntry);

            DateTime born = UserInfo.BornDate;
            int age = AppDataHelperClass.GetAge(born);
            string gender = UserInfo.Gender.ToString();
            double basal = AppDataHelperClass.GetBasalDailyCalories(age, gender, UserInfo.Weight);
            BasalDailyCalories = basal.ToString("0.00");

            string activity = UserInfo.HabitualPhysicalActivity.ToString();
            double dailyKiloCaloriesBurn = AppDataHelperClass.GetDailyKiloCaloriesBurn(basal, activity, gender);
            DailyKiloCaloriesBurn = dailyKiloCaloriesBurn.ToString("0.00");

            string objective = UserInfo.DailyKiloCaloriesObjective.ToString();
            DailyKiloCaloriesGoal = Math.Round(AppDataHelperClass.GetDailyKiloCaloriesGoal(dailyKiloCaloriesBurn, objective)).ToString() + " kcal";

            EntitiesHelperClass.ShowLoading("Atualizando usuário");

            try
            {
                await Task.Run(() => DaoHelperClass.UpdateUserInfo(UserInfo));
            }
            catch (Exception ex)
            {
                await ViewServices.PopUpManager.PopErrorAsync("Houve algum problema para atualizar as informações de usuário. Aguarde uns instantes e tente novamente." + ExceptionManager.ExceptionMessage(ex));
                EntitiesHelperClass.CloseLoading();
                return;
            }

            EntitiesHelperClass.CloseLoading();

            AppDataHelperClass.SetUserInfo(UserInfo);

            if (KcalEnabled)
                AppConfigHelperClass.SetEnergeticUnit(EnergeticUnit.kcal);
            else
                AppConfigHelperClass.SetEnergeticUnit(EnergeticUnit.kJ);

            await ViewServices.PopUpManager.PopInfoAsync("Configurações salvas com sucesso!");

            await _navigation.PopAsync();

        }


        [RelayCommand]
        private async Task CaloricSuggestChanged()
        {
            UpdateUi();

            try
            {
                DateTime born = UserInfo.BornDate;
                int age = AppDataHelperClass.GetAge(born);
                UserInfo.Gender = (Gender)GenderIndex;
                UserInfo.Weight = double.Parse(WeightEntry);
                double weight = UserInfo.Weight;
                string gender = UserInfo.Gender.ToString();
                double basal = AppDataHelperClass.GetBasalDailyCalories(age, gender, weight);
                BasalDailyCalories = basal.ToString("0.00");
                OnPropertyChanged(nameof(BasalDailyCalories));

                UserInfo.HabitualPhysicalActivity = (HabitualPhysicalActivity)HabitualPhysicalActivityIndex;
                string activity = UserInfo.HabitualPhysicalActivity.ToString();
                double dailyKiloCaloriesBurn = AppDataHelperClass.GetDailyKiloCaloriesBurn(basal, activity, gender);
                DailyKiloCaloriesBurn = dailyKiloCaloriesBurn.ToString("0.00");
                OnPropertyChanged(nameof(DailyKiloCaloriesBurn));

                UserInfo.DailyKiloCaloriesObjective = (DailyKiloCaloriesObjective)DailyKiloCaloriesObjectiveIndex;
                string objective = UserInfo.DailyKiloCaloriesObjective.ToString();
                DailyKiloCaloriesGoal = Math.Round(AppDataHelperClass.GetDailyKiloCaloriesGoal(dailyKiloCaloriesBurn, objective)).ToString() + " kcal";
                OnPropertyChanged(nameof(DailyKiloCaloriesGoal));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void UpdateUi()
        {
            OnPropertyChanged(nameof(UserInfo.BornDate));

            OnPropertyChanged(nameof(HabitualPhysicalActivityIndex));
            OnPropertyChanged(nameof(DailyKiloCaloriesObjectiveIndex));
            OnPropertyChanged(nameof(GenderIndex));

            OnPropertyChanged(nameof(WeightColorValidation));
            OnPropertyChanged(nameof(HeightColorValidation));

            OnPropertyChanged(nameof(GenderColorValidation));
            OnPropertyChanged(nameof(PhysicalActivityColorValidation));
            OnPropertyChanged(nameof(DailyKiloCaloriesObjectiveColorValidation));
            OnPropertyChanged(nameof(BornColorValidation));
            OnPropertyChanged(nameof(KiloCaloriesDiaryObjectiveColorValidation));
        }
    }

}
