﻿using CommunityToolkit.Mvvm.ComponentModel;
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
using CommunityToolkit.Maui.Core.Extensions;
using System.Reflection.Metadata.Ecma335;

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

        public string CarbohydrateGoalEntry { get; set; }

        public string ProteinGoalEntry { get; set; }

        public string FatGoalEntry { get; set; }

        public string FiberGoalEntry { get; set; }

        public string SodiumGoalEntry { get; set; }

        public string CholesterolGoalEntry { get; set; }

        public string CalciumGoalEntry { get; set; }

        public string IronGoalEntry { get; set; }

        public bool UseSuggestedCaloricGoalEntry { get; set; }

        public bool ValidInput { get; set; }

        public int CalculatedCalories { get; set; }

        public int? OldSetCalories { get; set; }

        public string CaloriesEntry { get; set; }

        public bool ManualCaloriesChange { get; set; }
        public bool InvalidInput { get => !ValidInput; }

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

        public Color CarbohydrateGoalColorValidation
        {
            get
            {
                if (StringFunctions.ParseDoubleValue(CarbohydrateGoalEntry, out double carbohydrateValue) && carbohydrateValue > 0)
                    return ColorHelperClass.ValidFieldColor;
                else
                    return ColorHelperClass.InvalidFieldColor;
            }
        }

        public Color ProteinGoalColorValidation
        {
            get
            {
                if (StringFunctions.ParseDoubleValue(ProteinGoalEntry, out double proteinValue) && proteinValue > 0)
                    return ColorHelperClass.ValidFieldColor;
                else
                    return ColorHelperClass.InvalidFieldColor;
            }
        }

        public Color FatGoalColorValidation
        {
            get
            {
                if (StringFunctions.ParseDoubleValue(FatGoalEntry, out double fatValue) && fatValue > 0)
                    return ColorHelperClass.ValidFieldColor;
                else
                    return ColorHelperClass.InvalidFieldColor;
            }
        }

        public Color FiberGoalColorValidation
        {
            get
            {
                if (StringFunctions.ParseDoubleValue(FiberGoalEntry, out double fiberValue) && fiberValue > 0)
                    return ColorHelperClass.ValidFieldColor;
                else
                    return ColorHelperClass.InvalidFieldColor;
            }
        }

        public Color SodiumGoalColorValidation
        {
            get
            {
                if (StringFunctions.ParseDoubleValue(SodiumGoalEntry, out double sodiumValue) && sodiumValue > 0)
                    return ColorHelperClass.ValidFieldColor;
                else
                    return ColorHelperClass.InvalidFieldColor;
            }
        }

        public Color CholesterolGoalColorValidation
        {
            get
            {
                if (StringFunctions.ParseDoubleValue(CholesterolGoalEntry, out double cholesterolValue) && cholesterolValue > 0)
                    return ColorHelperClass.ValidFieldColor;
                else
                    return ColorHelperClass.InvalidFieldColor;
            }
        }

        public Color CalciumGoalColorValidation
        {
            get
            {
                if (StringFunctions.ParseDoubleValue(CalciumGoalEntry, out double calciumValue) && calciumValue > 0)
                    return ColorHelperClass.ValidFieldColor;
                else
                    return ColorHelperClass.InvalidFieldColor;
            }
        }

        public Color IronGoalColorValidation
        {
            get
            {
                if (StringFunctions.ParseDoubleValue(IronGoalEntry, out double ironValue) && ironValue > 0)
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

            CaloriesEntry = UserInfo.KiloCaloriesDiaryObjective.ToString();
            OldSetCalories = int.Parse(CaloriesEntry);

            var anvisaLimits = AnvisaLimits.GetAnvisaLimits();

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
            WeightEntry = Math.Round(UserInfo.Weight, 2).ToString();
            HeightEntry = Math.Round(UserInfo.Height, 2).ToString();

            if (UserInfo.DailyCarbohydrateGoal.IsZeroOrNaN())
                CarbohydrateGoalEntry = Math.Round(anvisaLimits.Carboidratos, 2).ToString();
            else
                CarbohydrateGoalEntry = Math.Round(UserInfo.DailyCarbohydrateGoal, 2).ToString();

            if (UserInfo.DailyProteinGoal.IsZeroOrNaN())
                ProteinGoalEntry = Math.Round(anvisaLimits.Proteinas, 2).ToString();
            else
                ProteinGoalEntry = Math.Round(UserInfo.DailyProteinGoal, 2).ToString();

            if (UserInfo.DailyFatGoal.IsZeroOrNaN())
                FatGoalEntry = anvisaLimits.GordurasTotais.ToString();
            else
                FatGoalEntry = Math.Round(UserInfo.DailyFatGoal, 2).ToString();

            if (UserInfo.DailyFiberGoal.IsZeroOrNaN())
                FiberGoalEntry = anvisaLimits.FibraAlimentar.ToString();
            else
                FiberGoalEntry = Math.Round(UserInfo.DailyFiberGoal, 2).ToString();

            if (UserInfo.DailySodiumGoal.IsZeroOrNaN())
                SodiumGoalEntry = anvisaLimits.Sodio.ToString();
            else
                SodiumGoalEntry = Math.Round(UserInfo.DailySodiumGoal, 2).ToString();

            if (UserInfo.DailyCholesterolGoal.IsZeroOrNaN())
                CholesterolGoalEntry = anvisaLimits.Colesterol.ToString();
            else
                CholesterolGoalEntry = Math.Round(UserInfo.DailyCholesterolGoal, 2).ToString();

            if (UserInfo.DailyCalciumGoal.IsZeroOrNaN())
                CalciumGoalEntry = anvisaLimits.Calcio.ToString();
            else
                CalciumGoalEntry = Math.Round(UserInfo.DailyCalciumGoal, 2).ToString();

            if (UserInfo.DailyIronGoal.IsZeroOrNaN())
                IronGoalEntry = anvisaLimits.Ferro.ToString();
            else
                IronGoalEntry = Math.Round(UserInfo.DailyIronGoal, 2).ToString();

            UseSuggestedCaloricGoalEntry = UserInfo.UseSuggestedCaloricGoal;

            //if (UseSuggestedCaloricGoalEntry)
            //{
            //    UserInfo.KiloCaloriesDiaryObjective = int.Parse(DailyKiloCaloriesGoal);
            //}

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
        private async Task Appearing()
        {
            UpdateUi();
            await CaloricSuggestChanged();
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

            if(UserInfo.KiloCaloriesDiaryObjective == 0)
            {
                await ViewServices.PopUpManager.PopErrorAsync("Meta de calorias não informada!");
                return;
            }

            UserInfo.Gender = (Gender)GenderIndex;
            UserInfo.HabitualPhysicalActivity = (HabitualPhysicalActivity)HabitualPhysicalActivityIndex;
            UserInfo.DailyKiloCaloriesObjective = (DailyKiloCaloriesObjective)DailyKiloCaloriesObjectiveIndex;
            UserInfo.Weight = double.Parse(WeightEntry);
            UserInfo.DailyCarbohydrateGoal = double.Parse(CarbohydrateGoalEntry);
            UserInfo.DailyProteinGoal = double.Parse(ProteinGoalEntry);
            UserInfo.DailyFatGoal = double.Parse(FatGoalEntry);
            UserInfo.DailyFiberGoal = double.Parse(FiberGoalEntry);
            UserInfo.DailySodiumGoal = double.Parse(SodiumGoalEntry);
            UserInfo.DailyCholesterolGoal = double.Parse(CholesterolGoalEntry);
            UserInfo.DailyCalciumGoal = double.Parse(CalciumGoalEntry);
            UserInfo.DailyIronGoal = double.Parse(IronGoalEntry);
            UserInfo.UseSuggestedCaloricGoal = UseSuggestedCaloricGoalEntry;

            DateTime born = UserInfo.BornDate;
            int age = AppDataHelperClass.GetAge(born);
            string gender = UserInfo.Gender.ToString();
            double basal = AppDataHelperClass.GetBasalDailyCalories(age, gender, UserInfo.Weight);
            BasalDailyCalories = Math.Round(basal, 2).ToString();

            string activity = UserInfo.HabitualPhysicalActivity.ToString();
            double dailyKiloCaloriesBurn = AppDataHelperClass.GetDailyKiloCaloriesBurn(basal, activity, gender);
            DailyKiloCaloriesBurn = Math.Round(dailyKiloCaloriesBurn, 2).ToString();

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
                if (int.TryParse(CaloriesEntry, out int caloriesEntry))
                    UserInfo.KiloCaloriesDiaryObjective = caloriesEntry;
                else if (string.IsNullOrEmpty(CaloriesEntry))
                    UserInfo.KiloCaloriesDiaryObjective = 0;
                else
                    return;

                DateTime born = UserInfo.BornDate;
                int age = AppDataHelperClass.GetAge(born);
                UserInfo.Gender = (Gender)GenderIndex;
                UserInfo.Weight = double.Parse(WeightEntry);
                UserInfo.DailyCarbohydrateGoal = double.Parse(CarbohydrateGoalEntry);
                UserInfo.DailyProteinGoal = double.Parse(ProteinGoalEntry);
                UserInfo.DailyFatGoal = double.Parse(FatGoalEntry);
                UserInfo.DailyFiberGoal = double.Parse(FiberGoalEntry);
                UserInfo.DailySodiumGoal = double.Parse(SodiumGoalEntry);
                UserInfo.DailyCholesterolGoal = double.Parse(CholesterolGoalEntry);
                UserInfo.DailyCalciumGoal = double.Parse(CalciumGoalEntry);
                UserInfo.DailyIronGoal = double.Parse(IronGoalEntry);
                UserInfo.UseSuggestedCaloricGoal = UseSuggestedCaloricGoalEntry;
                double weight = UserInfo.Weight;
                string gender = UserInfo.Gender.ToString();
                double basal = AppDataHelperClass.GetBasalDailyCalories(age, gender, weight);
                BasalDailyCalories = Math.Round(basal, 2).ToString();
                OnPropertyChanged(nameof(BasalDailyCalories));

                UserInfo.HabitualPhysicalActivity = (HabitualPhysicalActivity)HabitualPhysicalActivityIndex;
                string activity = UserInfo.HabitualPhysicalActivity.ToString();
                double dailyKiloCaloriesBurn = AppDataHelperClass.GetDailyKiloCaloriesBurn(basal, activity, gender);
                DailyKiloCaloriesBurn = Math.Round(dailyKiloCaloriesBurn, 2).ToString();
                OnPropertyChanged(nameof(DailyKiloCaloriesBurn));

                UserInfo.DailyKiloCaloriesObjective = (DailyKiloCaloriesObjective)DailyKiloCaloriesObjectiveIndex;
                string objective = UserInfo.DailyKiloCaloriesObjective.ToString();

                double dailyKiloCalories = AppDataHelperClass.GetDailyKiloCaloriesGoal(dailyKiloCaloriesBurn, objective);

                if (dailyKiloCalories == 0)
                {
                    ValidInput = false;
                    UpdateUi();
                    return;
                }
                else
                    CalculatedCalories = int.Parse(Math.Round(AppDataHelperClass.GetDailyKiloCaloriesGoal(dailyKiloCaloriesBurn, objective)).ToString());

                DailyKiloCaloriesGoal = CalculatedCalories.ToString() + " kcal";

                OnPropertyChanged(nameof(DailyKiloCaloriesGoal));

                //if (UseSuggestedCaloricGoalEntry)
                //    UserInfo.KiloCaloriesDiaryObjective = CalculatedCalories;


                UserInfo.UseSuggestedCaloricGoal = UserInfo.KiloCaloriesDiaryObjective == CalculatedCalories;

                ManualCaloriesChange = true;

                ValidInput = true;
            }
            catch (Exception ex)
            {
                ValidInput = false;
                Console.WriteLine(ex);
            }

            UpdateUi();
        }

        [RelayCommand]
        private async Task SuggestedCaloriesCheckedChanged()
        {
            if (ManualCaloriesChange)
            {
                ManualCaloriesChange = false;
                return;
            }

            UseSuggestedCaloricGoalEntry = UserInfo.UseSuggestedCaloricGoal;

            if (UserInfo.UseSuggestedCaloricGoal)
            {
                if (OldSetCalories == null)
                    OldSetCalories = UserInfo.KiloCaloriesDiaryObjective;

                UserInfo.KiloCaloriesDiaryObjective = CalculatedCalories;
            }
            else
                UserInfo.KiloCaloriesDiaryObjective = OldSetCalories != null ? (int)OldSetCalories : 0;

            CaloriesEntry = UserInfo.KiloCaloriesDiaryObjective.ToString();

            UpdateUi();
        }

        private void UpdateUi()
        {
            OnPropertyChanged(nameof(UserInfo.BornDate));
            OnPropertyChanged(nameof(UserInfo.UseSuggestedCaloricGoal));
            OnPropertyChanged(nameof(UserInfo.KiloCaloriesDiaryObjective));
            OnPropertyChanged(nameof(UserInfo));

            OnPropertyChanged(nameof(CaloriesEntry));
            OnPropertyChanged(nameof(DailyKiloCaloriesGoal));
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
            OnPropertyChanged(nameof(UseSuggestedCaloricGoalEntry));

            OnPropertyChanged(nameof(ValidInput));
            OnPropertyChanged(nameof(InvalidInput));
        }
    }

}
