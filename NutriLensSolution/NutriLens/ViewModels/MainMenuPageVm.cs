using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using NutriLens.Views;
using NutriLens.Views.Popups;
using NutriLensClassLibrary.Models;
using System.Reflection;
using ExceptionLibrary;
using Android.Speech;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;

namespace NutriLens.ViewModels
{
    public partial class MainMenuPageVM : ObservableObject
    {
        private INavigation _navigation;

        private MealListClass _mealList;
        private List<PhysicalActivity> _physicalActivitiesList;
        private UserInfo UserInfo;

        private double _caloricBalance;
        private double _mealCalories;
        private double _physicalActivitiesCalories;

        public string TodayTotalEnergeticConsumption
        {
            get
            {
                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());
                _physicalActivitiesList = AppDataHelperClass.GetTodayPhysicalActivities();
                _caloricBalance = _mealList.TotalEnergeticConsumption() - EntitiesHelperClass.TotalEnergeticConsumption(_physicalActivitiesList);
                return _caloricBalance.ToString("0.00") + " " + AppConfigHelperClass.EnergeticUnit;
            }
        }

        public string PartialResults
        {
            get
            {
                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());
                _physicalActivitiesList = AppDataHelperClass.GetTodayPhysicalActivities();
                _caloricBalance = _mealList.TotalEnergeticConsumption() - EntitiesHelperClass.TotalEnergeticConsumption(_physicalActivitiesList);
                double diaryObjective = AppDataHelperClass.GetEnergeticDiaryObjective();
                double partialResults = (_caloricBalance / diaryObjective) * 100;
                return partialResults.ToString("0");
            }
        }

        public List<DataModel> PartialResultsChart
        {
            get
            {
                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());
                _physicalActivitiesList = AppDataHelperClass.GetTodayPhysicalActivities();
                _caloricBalance = _mealList.TotalEnergeticConsumption() - EntitiesHelperClass.TotalEnergeticConsumption(_physicalActivitiesList);
                double diaryObjective = AppDataHelperClass.GetEnergeticDiaryObjective();
                double partialResults = (_caloricBalance / diaryObjective) * 100;

                List<DataModel> donut = new List<DataModel>();
                DataModel consumed = new DataModel();
                consumed.Label = "Consumido: " + _caloricBalance.ToString("0.0") + " Kcal";
                consumed.Value = partialResults;
                donut.Add(consumed);

                DataModel empty = new DataModel();
                empty.Label = "Restam: " + (diaryObjective - _caloricBalance).ToString("0.0") + " Kcal";
                empty.Value = 100 - partialResults;
                donut.Add(empty);

                return donut;
            }
        }

        public string TodayProgressInfo
        {
            get
            {
                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());
                _physicalActivitiesList = AppDataHelperClass.GetTodayPhysicalActivities();

                _mealCalories = _mealList.TotalEnergeticConsumption();
                _physicalActivitiesCalories = EntitiesHelperClass.TotalEnergeticConsumption(_physicalActivitiesList);
                _caloricBalance = _mealCalories - _physicalActivitiesCalories;

                double diaryObjective = AppDataHelperClass.GetEnergeticDiaryObjective();

                return $"{_caloricBalance:0.00} / {diaryObjective} {AppConfigHelperClass.EnergeticUnit}{Environment.NewLine}({_caloricBalance / diaryObjective * 100:0.00}% atingido)";
            }
        }

        public string TodayMealCalories
        {
            get
            {
                return $"Ganho calórico: + {_mealCalories:0.00} {AppConfigHelperClass.EnergeticUnit}";
            }
        }

        public string TodayPhysicalActivitiesCalories
        {
            get
            {
                return $"Perda calórica: - {_physicalActivitiesCalories:0.00} {AppConfigHelperClass.EnergeticUnit}";
            }
        }

        public string TodayMealCarbohydrates
        {
            get
            {
                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());
                return $"Carboidrato: {_mealList.TotalCarbohydratesConsumption()} g";
            }
        }

        public string TodayMealProteins
        {
            get
            {
                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());
                return $"Proteína: {_mealList.TotalProteinsConsumption()} g";
            }
        }

        public string TodayMealFat
        {
            get
            {
                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());
                return $"Gordura: {_mealList.TotalFatConsumption()} g";
            }
        }

        public string TodayMealFibers
        {
            get
            {
                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());
                return $"Fibra: {_mealList.TotalFibersConsumption()} g";
            }
        }

        public string TodayMealSodium
        {
            get
            {
                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());
                return $"Sódio: {_mealList.TotalSodiumConsumption()} mg";
            }
        }

        public string TodayMealCholesterol
        {
            get
            {
                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());
                return $"Colesterol: {_mealList.TotalCholesterolConsumption} mg";
            }
        }

        public string TodayMealCalcium
        {
            get
            {
                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());
                return $"Calcio: {_mealList.TotalCalciumConsumption} mg";
            }
        }

        public string TodayMealIron
        {
            get
            {
                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());
                return $"Ferro: {_mealList.TotalIronConsumption} mg";
            }
        }


        public string AppVersion { get => "V " + AppInfo.Current.VersionString; }

        public ObservableCollection<Brush> Chart1ColorPalette { get; set; }
        public ObservableCollection<Brush> Chart2ColorPalette { get; set; }

        public ObservableCollection<Brush> Chart3ColorPalette { get; set; }

        public List<DataModel> PartialResultsMacroNutrients1
        {
            get
            {
                var anvisaLimits = AppDataHelperClass.GetAnvisaLimits();
                double limiteCarboidratos = AppDataHelperClass.UserInfo.DailyCarbohydrateGoal;
                double limiteProteinas = AppDataHelperClass.UserInfo.DailyProteinGoal;

                if (AppDataHelperClass.UserInfo.DailyCarbohydrateGoal.IsZeroOrNaN())
                    limiteCarboidratos = anvisaLimits.Carboidratos;

                if (AppDataHelperClass.UserInfo.DailyCarbohydrateGoal.IsZeroOrNaN())
                    limiteProteinas = anvisaLimits.Proteinas;


                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());
                _physicalActivitiesList = AppDataHelperClass.GetTodayPhysicalActivities();
                _caloricBalance = _mealList.TotalEnergeticConsumption() - EntitiesHelperClass.TotalEnergeticConsumption(_physicalActivitiesList);
                double diaryObjective = AppDataHelperClass.GetEnergeticDiaryObjective();
                double partialResults = (_caloricBalance / diaryObjective) * 100;

                double carboidratos = _mealList.TotalCarbohydratesConsumption();    //300g   %100 VD
                double proteinas = _mealList.TotalProteinsConsumption();            //75g    %100 VD 

                List<DataModel> barChart = new List<DataModel>();

                DataModel prot = new DataModel();
                prot.Label = "Proteínas:\n " + proteinas.ToString("0.0") + "g\n" + " de " + limiteProteinas.ToString("0.0") + "g";
                prot.Value = (proteinas / limiteProteinas) * 100;
                barChart.Add(prot);

                DataModel carb = new DataModel();
                carb.Label = "Carboidratos:\n " + carboidratos.ToString("0.0") + "g\n" + " de " + limiteCarboidratos.ToString("0.0") + "g";
                carb.Value = (carboidratos / limiteCarboidratos) * 100;
                barChart.Add(carb);

                DataModel consumed = new DataModel();
                consumed.Label = "Calorias:\n " + _caloricBalance.ToString("0.0") + "kcal\n" + " de " + diaryObjective.ToString("0.0") + "kcal";
                consumed.Value = partialResults;
                barChart.Add(consumed);

                return barChart;
            }
        }

        public List<DataModel> PartialResultsMacroNutrients2
        {
            get
            {
                var anvisaLimits = AppDataHelperClass.GetAnvisaLimits();
                double limiteGorduras = AppDataHelperClass.UserInfo.DailyFatGoal;
                double limiteFibras = AppDataHelperClass.UserInfo.DailyFiberGoal;
                double limiteSodio = AppDataHelperClass.UserInfo.DailySodiumGoal;

                if (AppDataHelperClass.UserInfo.DailyFatGoal.IsZeroOrNaN())
                    limiteGorduras = anvisaLimits.GordurasTotais;

                if (AppDataHelperClass.UserInfo.DailyFiberGoal.IsZeroOrNaN())
                    limiteFibras = anvisaLimits.FibraAlimentar;

                if (AppDataHelperClass.UserInfo.DailySodiumGoal.IsZeroOrNaN())
                    limiteSodio = anvisaLimits.Sodio;

                double gordura = _mealList.TotalFatConsumption();                   //55g    %100 VD Gorduras totais
                double fibra = _mealList.TotalFibersConsumption();                  //25g    %100 VD
                double sodio = _mealList.TotalSodiumConsumption();                  //2,4g   %100 VD

                List<DataModel> barChart = new List<DataModel>();

                DataModel gord = new DataModel();
                gord.Label = "Gorduras:\n " + gordura.ToString("0.0") + "g\n" + " de " + limiteGorduras.ToString("0.0") + "g";
                gord.Value = (gordura / limiteGorduras) * 100;
                gord.TrackStroke = SolidColorBrush.Red;
                barChart.Add(gord);


                DataModel fib = new DataModel();
                fib.Label = "Fibras:\n " + fibra.ToString("0.0") + "g\n" + " de " + limiteFibras.ToString("0.0") + "g";
                fib.Value = (fibra / limiteFibras) * 100;
                fib.TrackFill = SolidColorBrush.Magenta;
                barChart.Add(fib);

                DataModel sod = new DataModel();
                sod.Label = "Sódio:\n " + sodio.ToString("0.0") + "mg\n" + " de " + limiteSodio.ToString("0.0") + "mg";
                sod.Value = (sodio / limiteSodio) * 100;
                barChart.Add(sod);

                return barChart;
            }
        }

        public List<DataModel> PartialResultsMacroNutrients3
        {
            get
            {
                var anvisaLimits = AppDataHelperClass.GetAnvisaLimits();
                double limiteColesterol = AppDataHelperClass.UserInfo.DailyCholesterolGoal;
                double limiteCalcio = AppDataHelperClass.UserInfo.DailyCalciumGoal;
                double limiteFerro = AppDataHelperClass.UserInfo.DailyIronGoal;

                if (AppDataHelperClass.UserInfo.DailyCholesterolGoal.IsZeroOrNaN())
                    limiteColesterol = anvisaLimits.Colesterol;

                if (AppDataHelperClass.UserInfo.DailyCalciumGoal.IsZeroOrNaN())
                    limiteCalcio = anvisaLimits.Calcio;

                if (AppDataHelperClass.UserInfo.DailyIronGoal.IsZeroOrNaN())
                    limiteFerro = anvisaLimits.Ferro;


                double colesterol = _mealList.TotalCholesterolConsumption();        //300mg    %100 VD Gorduras totais
                double calcio = _mealList.TotalCalciumConsumption();                //1000mg    %100 VD
                double ferro = _mealList.TotalIronConsumption();                    //14mg   %100 VD

                List<DataModel> barChart = new List<DataModel>();

                DataModel col = new DataModel();
                col.Label = "Colesterol:\n " + colesterol.ToString("0.0") + "mg\n" + " de " + limiteColesterol.ToString("0.0") + "mg";
                col.Value = (colesterol / limiteColesterol) * 100;
                barChart.Add(col);


                DataModel cal = new DataModel();
                cal.Label = "Cálcio:\n " + calcio.ToString("0.0") + "mg\n" + " de " + limiteCalcio.ToString("0.0") + "mg";
                cal.Value = (calcio / limiteCalcio) * 100;
                barChart.Add(cal);

                DataModel fer = new DataModel();
                fer.Label = "Ferro:\n " + ferro.ToString("0.0") + "mg\n" + " de " + limiteFerro.ToString("0.0") + "mg";
                fer.Value = (ferro / limiteFerro) * 100;
                barChart.Add(fer);

                return barChart;
            }
        }

        public bool DevUser { get => AppDataHelperClass.UserInfo.DevUser; }

        public MainMenuPageVM(INavigation navigation)
        {
            _navigation = navigation;
            Application.Current.UserAppTheme = AppTheme.Light;

            Chart1ColorPalette = new ObservableCollection<Brush>
            {
                new SolidColorBrush(ColorHelperClass.ProteinColor),
                new SolidColorBrush(ColorHelperClass.CarbohydratesColor),
                new SolidColorBrush(ColorHelperClass.CaloriesColor)
            };

            Chart2ColorPalette = new ObservableCollection<Brush>
            {
                new SolidColorBrush(ColorHelperClass.FatColor),
                new SolidColorBrush(ColorHelperClass.FibersColor),
                new SolidColorBrush(ColorHelperClass.SodiumColor),
            };

            Chart3ColorPalette = new ObservableCollection<Brush>
            {
                new SolidColorBrush(ColorHelperClass.CholesterolColor),
                new SolidColorBrush(ColorHelperClass.CalciumColor),
                new SolidColorBrush(ColorHelperClass.IronColor),
            };
        }


        [RelayCommand]
        private async Task OpenFlyout()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IFlyoutPage>());
        }

        [RelayCommand]
        private async Task OpenCamera()
        {
            await _navigation.PushAsync(new MobileCameraPageV2(_navigation));
        }

        [RelayCommand]
        private async Task OpenBarCode()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IBarCodePage>());
        }

        [RelayCommand]
        private async Task OpenManualInput()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IManualInputPage>());
        }

        [RelayCommand]
        private async Task VoiceInput()
        {
            await TextToSpeech.SpeakAsync("Claro! Informe todos os itens do seu prato e a quantidade em gramas de cada um deles.");
            string command = await VoiceCommandsHelperClass.GetVoiceCommand();

            if (await ViewServices.PopUpManager.PopYesOrNoAsync("Detecção por voz", "Fala detectada: " + command + Environment.NewLine + "Deseja continuar?"))
            {
                List<FoodItem> foodItems;

                while (true)
                {
                    foodItems = await EntitiesHelperClass.GetAiAnalysisByMealDescription(command);

                    if (foodItems == null)
                    {
                        if (!await ViewServices.PopUpManager.PopYesOrNoAsync("Falha na identificação dos alimentos", "Deseja tentar novamente?"))
                            return;
                    }
                    else
                        break;
                }

                AppDataHelperClass.DetectedFoodItems = foodItems;
                await OpenManualInput();
            }
        }

        [RelayCommand]
        private async Task DescriptionInput()
        {
            TextEntryPopup textEntryPopup = new TextEntryPopup("Informe todos os itens do seu prato e a quantidade aproximada em gramas de cada um deles.");
            await Application.Current.MainPage.ShowPopupAsync(textEntryPopup);

            if (!string.IsNullOrEmpty(textEntryPopup.Entry))
            {
                List<FoodItem> foodItems;

                while (true)
                {
                    foodItems = await EntitiesHelperClass.GetAiAnalysisByMealDescription(textEntryPopup.Entry);

                    if (foodItems == null)
                    {
                        if (!await ViewServices.PopUpManager.PopYesOrNoAsync("Falha na identificação dos alimentos", "Deseja tentar novamente?"))
                            return;
                    }
                    else
                        break;
                }

                AppDataHelperClass.DetectedFoodItems = foodItems;
                await OpenManualInput();
            }
        }

        [RelayCommand]
        private async Task RegisterPhysicalActivity()
        {
            AddPhysicalActivityPopup physicalActivityPopup = new AddPhysicalActivityPopup();
            await Application.Current.MainPage.ShowPopupAsync(physicalActivityPopup);
            OnPropertyChanged(nameof(TodayProgressInfo));
            OnPropertyChanged(nameof(TodayPhysicalActivitiesCalories));
            OnPropertyChanged(nameof(PartialResults));
            OnPropertyChanged(nameof(PartialResultsChart));
            OnPropertyChanged(nameof(PartialResultsMacroNutrients1));
            OnPropertyChanged(nameof(PartialResultsMacroNutrients2));
            OnPropertyChanged(nameof(PartialResultsMacroNutrients3));
        }

        [RelayCommand]
        private async Task PerDayHistoric()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IGroupedMealHistoricPage>(MealHistoryFilter.PerDay));
        }

        [RelayCommand]
        private async Task PerWeekHistoric()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IGroupedMealHistoricPage>(MealHistoryFilter.PerWeek));
        }

        [RelayCommand]
        private async Task PerMonthHistoric()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IGroupedMealHistoricPage>(MealHistoryFilter.PerMonth));
        }

        [RelayCommand]
        private async Task PerPeriodHistoric()
        {
            PeriodChoosePopup periodChoosePopup = new PeriodChoosePopup();
            await Application.Current.MainPage.ShowPopupAsync(periodChoosePopup);

            if (periodChoosePopup.Confirmed)
            {
                List<Meal> mealsInPeriod = AppDataHelperClass.GetMealsByDateRange(periodChoosePopup.StartSelectedDate, periodChoosePopup.EndSelectedDate);

                if (mealsInPeriod.Count == 0)
                {
                    await ViewServices.PopUpManager.PopErrorAsync("Não foram encontradas refeições no perído informado!");
                    return;
                }
                else
                {
                    MealListClass mealListClass = new MealListClass(mealsInPeriod, periodChoosePopup.StartSelectedDate, periodChoosePopup.EndSelectedDate);
                    AppDataHelperClass.FilteredMealList = mealListClass;
                    await _navigation.PushAsync(ViewServices.ResolvePage<IMealHistoricPage>());
                }
            }
        }

        [RelayCommand]
        private async Task ListAll()
        {
            try
            {
                AppDataHelperClass.FilteredMealList = null;
                await _navigation.PushAsync(ViewServices.ResolvePage<IMealHistoricPage>());
            }
            catch (Exception ex)
            {
                await ViewServices.PopUpManager.PopErrorAsync(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [RelayCommand]
        private async Task OpenGallery()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IPicturesGridPage>());
        }

        [RelayCommand]
        private async Task EditBarCodeProducts()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IEditBarCodeProductsPage>());
        }

        [RelayCommand]
        private async Task EditAiModelPrompt()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IAiModelPromptPage>());
        }

        [RelayCommand]
        private async Task OpenConfig()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IUserConfigPage>());
        }

        [RelayCommand]
        private async Task OpenTermsOfUse()
        {
            TermsOfUsePopup termsOfUsePopup = new TermsOfUsePopup(true);
            await Application.Current.MainPage.ShowPopupAsync(termsOfUsePopup);
        }

        [RelayCommand]
        private void Appearing()
        {
            OnPropertyChanged(nameof(TodayProgressInfo));
            OnPropertyChanged(nameof(TodayMealCalories));
            OnPropertyChanged(nameof(TodayPhysicalActivitiesCalories));
            OnPropertyChanged(nameof(TodayMealCarbohydrates));
            OnPropertyChanged(nameof(TodayMealProteins));
            OnPropertyChanged(nameof(TodayMealFat));
            OnPropertyChanged(nameof(TodayMealFibers));
            OnPropertyChanged(nameof(TodayMealSodium));
            OnPropertyChanged(nameof(TodayMealCholesterol));
            OnPropertyChanged(nameof(TodayMealCalcium));
            OnPropertyChanged(nameof(TodayMealIron));
            OnPropertyChanged(nameof(PartialResultsChart));
            OnPropertyChanged(nameof(PartialResultsMacroNutrients1));
            OnPropertyChanged(nameof(PartialResultsMacroNutrients2));
            OnPropertyChanged(nameof(PartialResultsMacroNutrients3));
        }

        [RelayCommand]
        private void Disappearing()
        {

        }

        [RelayCommand]
        private async void LogOut()
        {
            if (await ViewServices.PopUpManager.PopYesOrNoAsync("Sair", "Deseja sair deste dispositivo?"))
            {
                AppDataHelperClass.CleanUserInfo();
                AppDataHelperClass.CleanSessionInfo();
                _navigation.InsertPageBefore(ViewServices.ResolvePage<ILoginPage>(), _navigation.NavigationStack.Last());
                await _navigation.PopAsync();
            }
        }

        [RelayCommand]
        private async void CloseApplication()
        {
            if (await ViewServices.PopUpManager.PopYesOrNoAsync("Fechar aplicativo", "Deseja fechar o aplicativo?"))
                Application.Current.Quit();
        }
    }
}
