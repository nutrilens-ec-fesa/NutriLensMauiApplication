﻿using CommunityToolkit.Mvvm.ComponentModel;
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
            // Teste de consulta com o ChatGPT
            try
            {
                string input = await ViewServices.PopUpManager.PopFreeInputAsync("Teste GPT", "Informe o alimento");

                if (!string.IsNullOrEmpty(input))
                {
                    FoodItem foodItem = new()
                    {
                        Name = input
                    };

                    string nutritionalInfo = DaoHelperClass.GetNutritionalInfo(foodItem);

                    await ViewServices.PopUpManager.PopInfoAsync(nutritionalInfo);
                }
            }
            catch(Exception ex)
            {
                await ViewServices.PopUpManager.PopErrorAsync(ex.Message);
            }
        }

        [RelayCommand]
        private async Task PerMonthHistoric()
        {
            try
            {
                // Teste de conexão com o mongo
                await ViewServices.PopUpManager.PopInfoAsync(DaoHelperClass.MongoDbPingTest());
            }
            catch(Exception ex)
            {
                await ViewServices.PopUpManager.PopErrorAsync(ex.Message);
            }
        }

        [RelayCommand]
        private async Task PerPeriodHistoric()
        {
            await ViewServices.PopUpManager.PopInDevelopment();
        }

        [RelayCommand]
        private async Task OpenGallery()
        {
            await _navigation.PushAsync(ViewServices.ResolvePage<IPicturesGridPage>());
        }

        [RelayCommand]
        private void Appearing()
        {
            OnPropertyChanged(nameof(TodayTotalEnergeticConsumption));
        }
    }
}
