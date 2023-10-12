using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
                return _mealList.TotalEnergeticConsumption(AppConfigHelperClass.EnergeticUnit);
            }
        }

        public MainMenuPageVM(INavigation navigation)
        {
            _navigation = navigation;
        }

        [RelayCommand]
        private async Task OpenFlyout()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IFlyoutPage>());
        }

        [RelayCommand]
        private async Task OpenCamera()
        {
            await _navigation.PushAsync(new MobileCameraPageV2());
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
            try
            {
                string input = await ViewServices.PopUpManager.PopFreeInputAsync("Teste GPT", "Informe o alimento");

                FoodItem foodItem = new()
                {
                    Name = input
                };

                string nutritionalInfo = DaoHelperClass.GetNutritionalInfo(foodItem);

                await ViewServices.PopUpManager.PopInfoAsync(nutritionalInfo);
            }
            catch(Exception ex)
            {
                await ViewServices.PopUpManager.PopErrorAsync(ex.Message);
            }
        }

        [RelayCommand]
        private async Task PerMonthHistoric()
        {
            await ViewServices.PopUpManager.PopInDevelopment(MethodBase.GetCurrentMethod());
        }

        [RelayCommand]
        private async Task PerPeriodHistoric()
        {
            await ViewServices.PopUpManager.PopInDevelopment(MethodBase.GetCurrentMethod());
        }

        [RelayCommand]
        private void Appearing()
        {
            OnPropertyChanged(nameof(TodayTotalEnergeticConsumption));
        }
    }
}
