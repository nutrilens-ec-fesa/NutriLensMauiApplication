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

namespace NutriLens.ViewModels
{
    public partial class MainMenuPageVM : ObservableObject
    {
        private INavigation _navigation;

        private MealListClass _mealList;
        private List<PhysicalActivity> _physicalActivitiesList;

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

        public MainMenuPageVM(INavigation navigation)
        {
            _navigation = navigation;
            Application.Current.UserAppTheme = AppTheme.Light;
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

            if(await ViewServices.PopUpManager.PopYesOrNoAsync("Detecção por voz", "Fala detectada: " + command + Environment.NewLine + "Deseja continuar?"))
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
        private async Task RegisterPhysicalActivity()
        {
            AddPhysicalActivityPopup physicalActivityPopup = new AddPhysicalActivityPopup();
            await Application.Current.MainPage.ShowPopupAsync(physicalActivityPopup);
            OnPropertyChanged(nameof(TodayProgressInfo));
            OnPropertyChanged(nameof(TodayPhysicalActivitiesCalories));
        }

        [RelayCommand]
        private async Task PerDayHistoric()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IMealHistoricPage>(MealHistoryFilter.PerDay));
        }

        [RelayCommand]
        private async Task PerWeekHistoric()
        {
            await ViewServices.PopUpManager.PopInDevelopment(MethodBase.GetCurrentMethod());
            //await _navigation.PushAsync(ViewServices.ResolvePage<IMealHistoricPage>(MealHistoryFilter.PerWeek));
            // Teste de consulta com o ChatGPT
            //try
            //{
            //    string input = await ViewServices.PopUpManager.PopFreeInputAsync("Teste GPT", "Informe o alimento");

            //    if (!string.IsNullOrEmpty(input))
            //    {
            //        FoodItem foodItem = new()
            //        {
            //            Name = input
            //        };

            //        string nutritionalInfo = DaoHelperClass.GetNutritionalInfo(foodItem);

            //        await ViewServices.PopUpManager.PopInfoAsync(nutritionalInfo);
            //    }
            //}
            //catch(Exception ex)
            //{
            //    await ViewServices.PopUpManager.PopErrorAsync(ex.Message);
            //}
        }

        [RelayCommand]
        private async Task PerMonthHistoric()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IMealHistoricPage>(MealHistoryFilter.PerMonth));
        }

        [RelayCommand]
        private async Task PerPeriodHistoric()
        {
            await ViewServices.PopUpManager.PopInDevelopment(MethodBase.GetCurrentMethod());
        }

        [RelayCommand]
        private async Task ListAll()
        {
            try
            {
                await _navigation.PushAsync(ViewServices.ResolvePage<IMealHistoricPage>(MealHistoryFilter.All));
            }
            catch(Exception ex)
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
        }

        [RelayCommand]
        private void Disappearing()
        {
            
        }

        [RelayCommand]
        private async void LogOut()
        {
            if(await ViewServices.PopUpManager.PopYesOrNoAsync("Sair", "Deseja sair deste dispositivo?"))
            {
                AppDataHelperClass.CleanUserInfo();
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
