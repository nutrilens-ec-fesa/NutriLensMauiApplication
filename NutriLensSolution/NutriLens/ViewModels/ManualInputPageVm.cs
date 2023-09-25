using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLens.Views.Popups;
using System.Collections.ObjectModel;

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
        
        public int FoodItemsQuantity  => FoodItems.Count; 
        public double KiloCalories => FoodItems.Sum(x => x.KiloCalories); 
        public string EnergeticUnit => AppConfigHelperClass.EnergeticUnit.ToString();

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public ManualInputPageVm(INavigation navigation)
        {
            _navigation = navigation;
            FoodItems = new ObservableCollection<FoodItem>();
            
            // Mocked FoodItems
            //FoodItems = new ObservableCollection<FoodItem>
            //{
            //    new FoodItem { Name = "Arroz", Portion = "100g", KiloCalories = 200 },
            //    new FoodItem { Name = "Feijão", Portion = "100g", KiloCalories = 200 },
            //    new FoodItem { Name = "Peito de frango", Portion = "100g", KiloCalories = 450 }
            //};
        }

        [RelayCommand]
        public async Task AddNewItem()
        {
            AddFoodItemPopup addFoodItemPopup = new AddFoodItemPopup();
            await Application.Current.MainPage.ShowPopupAsync(addFoodItemPopup);

            if (addFoodItemPopup.Confirmed)
            {
                FoodItems.Add(new()
                { 
                    Name = addFoodItemPopup.InputItem,
                    Portion = addFoodItemPopup.InputPortion,
                    KiloCalories = addFoodItemPopup.InputCalories
                });

                OnPropertyChanged(nameof(FoodItemsQuantity));
                OnPropertyChanged(nameof(KiloCalories));
            }
        }

        [RelayCommand]
        public async Task RegisterMeal()
        {
            if(FoodItemsQuantity == 0)
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

            await ViewServices.PopUpManager.PopInfoAsync("Refeição registrada com sucesso!");
            await _navigation.PopAsync();
        }
    }
}
