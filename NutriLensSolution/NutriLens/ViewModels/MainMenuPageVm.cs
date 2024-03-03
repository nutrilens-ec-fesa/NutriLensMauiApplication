using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using NutriLens.Views;
using System.Reflection;

namespace NutriLens.ViewModels
{
    public partial class MainMenuPageVM : ObservableObject
    {
        private INavigation _navigation;

        private MealListClass _mealList;

        public string TodayTotalEnergeticConsumption
        {
            get
            {
                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());
                return _mealList.TotalEnergeticConsumption();
            }
        }

        public string TodayProgressInfo
        {
            get
            {
                _mealList = new MealListClass(AppDataHelperClass.GetTodayMeals());

                double todayCalories = double.Parse(_mealList.TotalEnergeticConsumption(true));
                double diaryObjective = AppDataHelperClass.GetEnergeticDiaryObjective();

                return $"{todayCalories} / {diaryObjective} {AppConfigHelperClass.EnergeticUnit}{Environment.NewLine}({todayCalories / diaryObjective * 100:0.00}% atingido)";
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
            await _navigation.PushAsync(ViewServices.ResolvePage<IMealHistoricPage>(MealHistoryFilter.All));
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
        private void Appearing()
        {
            OnPropertyChanged(nameof(TodayProgressInfo));
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
