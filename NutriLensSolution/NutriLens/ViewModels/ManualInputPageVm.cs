﻿using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLens.Views.Popups;
using NutriLensClassLibrary.Models;
using System.Collections.ObjectModel;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace NutriLens.ViewModels
{
    public partial class ManualInputPageVm : ObservableObject
    {
        private INavigation _navigation;
        private ObservableCollection<FoodItem> _foodItems;
        public ObservableCollection<FoodItem> FoodItems
        {
            get => _foodItems;
            set
            {
                _foodItems = value;
                OnPropertyChanged(nameof(FoodItems));
            }
        }

        public int FoodItemsQuantity => FoodItems.Count;
        public double KiloCalories => FoodItems.Sum(x => x.KiloCalories);
        public string EnergeticUnit => AppConfigHelperClass.EnergeticUnit.ToString();

        public ManualInputPageVm(INavigation navigation)
        {
            _navigation = navigation;
            FoodItems = new ObservableCollection<FoodItem>();
        }

        public ManualInputPageVm(INavigation navigation, List<FoodItem> foods)
        {
            _navigation = navigation;
            FoodItems = new ObservableCollection<FoodItem>();
            foreach(FoodItem food in foods)
            {
                FoodItems.Add(food);
            }
        }

        [RelayCommand]
        public async Task AddNewItemFromTbca()
        {
            AddTbcaFoodItemPopup addFoodItemPopup = new AddTbcaFoodItemPopup();
            await Application.Current.MainPage.ShowPopupAsync(addFoodItemPopup);

            if (addFoodItemPopup.Confirmed)
            {
                FoodItem foodItem;

                if (addFoodItemPopup.SelectedItem != null)
                {
                    foodItem = new()
                    {
                        Name = addFoodItemPopup.SelectedItem.Alimento,
                        Portion = addFoodItemPopup.InputPortion,
                        KiloCalories = addFoodItemPopup.InputCalories
                    };
                }
                else
                    return;

                // Se não informou as calorias
                //if (addFoodItemPopup.InputCalories == -1)
                //{
                //    string gptJson = DaoHelperClass.GetNutritionalInfo(foodItem);
                //    GptNutritionalInfo gptNutritionalInfo = JsonConvert.DeserializeObject<GptNutritionalInfo>(gptJson);
                //    foodItem.KiloCalories = gptNutritionalInfo.CaloriesValue;
                //}

                FoodItems.Add(foodItem);

                OnPropertyChanged(nameof(FoodItemsQuantity));
                OnPropertyChanged(nameof(KiloCalories));
            }
        }

        [RelayCommand]
        public async Task AddNewItemFromTaco()
        {
            AddTacoFoodItemPopup addFoodItemPopup = new AddTacoFoodItemPopup();
            await Application.Current.MainPage.ShowPopupAsync(addFoodItemPopup);

            if (addFoodItemPopup.Confirmed)
            {
                FoodItem foodItem;

                if (addFoodItemPopup.SelectedItem != null)
                {
                    foodItem = new()
                    {
                        Name = addFoodItemPopup.SelectedItem.Nome,
                        Portion = addFoodItemPopup.InputPortion,
                        KiloCalories = addFoodItemPopup.InputCalories
                    };
                }
                else
                    return;

                // Se não informou as calorias
                //if (addFoodItemPopup.InputCalories == -1)
                //{
                //    string gptJson = DaoHelperClass.GetNutritionalInfo(foodItem);
                //    GptNutritionalInfo gptNutritionalInfo = JsonConvert.DeserializeObject<GptNutritionalInfo>(gptJson);
                //    foodItem.KiloCalories = gptNutritionalInfo.CaloriesValue;
                //}

                FoodItems.Add(foodItem);

                OnPropertyChanged(nameof(FoodItemsQuantity));
                OnPropertyChanged(nameof(KiloCalories));
            }
        }

        [RelayCommand]
        public async Task RegisterMeal()
        {
            if (FoodItemsQuantity == 0)
            {
                await ViewServices.PopUpManager.PopErrorAsync("Não foi inserido nenhum item na refeição. Para registrar a refeição, por favor insira um ou mais itens.");
                return;
            }

            Meal newMeal = new()
            {
                DateTime = DateTime.Now,
                Name = "Refeição",
                FoodItems = FoodItems.ToList()
            };

            AppDataHelperClass.AddMeal(newMeal);
            if(AppDataHelperClass.foods != null)
            {
                AppDataHelperClass.foods.Clear();
            }
            await ViewServices.PopUpManager.PopInfoAsync("Refeição registrada com sucesso!");
            await _navigation.PopAsync();
        }

        [RelayCommand]
        private void Appearing()
        {
            try
            {
                if (AppDataHelperClass.foods != null)
                {
                    foreach (FoodItem food in AppDataHelperClass.foods)
                    {
                        FoodItems.Add(food);
                    }
                    OnPropertyChanged(nameof(FoodItemsQuantity));
                    OnPropertyChanged(nameof(KiloCalories));
                }
            }catch(Exception ex)
            {
                ViewServices.PopUpManager.PopErrorAsync("Erro ao carregar os dados");
            }
            
            
        }

        [RelayCommand]
        private void Disappearing()
        {

        }
    }
}
