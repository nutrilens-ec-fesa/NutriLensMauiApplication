using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        /// Indica o índice de hábito de atividade física selecionado pelo usuário
        /// </summary>
        public int HabitualPhysicalActivityIndex { get; set; }

        /// <summary>
        /// Picker de gêneros
        /// </summary>
        public ObservableCollection<string> GenderOptions { get; set; }

        /// <summary>
        /// Picker de hábitos de atividade física
        /// </summary>
        public ObservableCollection<string> HabitualPhysicalActivityOptions { get; set; }

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

            GenderOptions = new ObservableCollection<string>()
            {
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
        }

        [RelayCommand]
        private async Task SaveConfigs()
        {
            char cultureDecimalSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            char decimalToBeReplaced = cultureDecimalSeparator == ',' ? '.' : ',';

            if (double.TryParse(WeightEntry.Replace(decimalToBeReplaced, cultureDecimalSeparator), out double weightValue))
                UserInfo.Weight = weightValue;
            else
            {
                await ViewServices.PopUpManager.PopErrorAsync("Peso informado inválido!");
                return;
            }

            if (double.TryParse(HeightEntry.Replace(decimalToBeReplaced, cultureDecimalSeparator), out double heightValue))
                UserInfo.Height = heightValue;
            else
            {
                await ViewServices.PopUpManager.PopErrorAsync("Altura informada inválida!");
                return;
            }

            UserInfo.Gender = (Gender)GenderIndex;
            UserInfo.HabitualPhysicalActivity = (HabitualPhysicalActivity)HabitualPhysicalActivityIndex;

            AppDataHelperClass.SetUserInfo(UserInfo);

            if (KcalEnabled)
                AppConfigHelperClass.SetEnergeticUnit(EnergeticUnit.kcal);
            else
                AppConfigHelperClass.SetEnergeticUnit(EnergeticUnit.kJ);

            await ViewServices.PopUpManager.PopInfoAsync("Configurações salvas com sucesso!");

            await _navigation.PopAsync();
        }
    }
}
