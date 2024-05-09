using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
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
        public double KiloCaloriesRound => Math.Round(KiloCalories, 2);
        public string EnergeticUnit => AppConfigHelperClass.EnergeticUnit.ToString();
        public bool HasPictureAnalysisSource { get => !string.IsNullOrEmpty(AppDataHelperClass.NewFoodPicturePath); }
        public string PictureAnalysisSource { get => AppDataHelperClass.NewFoodPicturePath; }

        public ManualInputPageVm(INavigation navigation)
        {
            _navigation = navigation;
            FoodItems = new ObservableCollection<FoodItem>();
        }

        public ManualInputPageVm(INavigation navigation, List<FoodItem> foods)
        {
            _navigation = navigation;
            FoodItems = new ObservableCollection<FoodItem>();
            foreach (FoodItem food in foods)
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
                OnPropertyChanged(nameof(KiloCaloriesRound));
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
                        KiloCalories = addFoodItemPopup.InputCalories,
                        TacoFoodItem = TacoFoodItemParseHelperClass.GetTacoFoodItem(addFoodItemPopup.SelectedItem, addFoodItemPopup.InputPortion)
                    };
                }
                else
                    return;

                FoodItems.Add(foodItem);

                UpdateFoodItemsList();
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

            // Se for uma nova refeição
            if (AppDataHelperClass.MealToEdit == null)
            {
                Meal newMeal = new()
                {
                    DateTime = DateTime.Now,
                    Name = "Refeição",
                    FoodItems = FoodItems.ToList(),
                    MealPicturePath = AppDataHelperClass.NewFoodPicturePath
                };

                AppDataHelperClass.AddMeal(newMeal);
                AppDataHelperClass.DetectedFoodItems?.Clear();
                AppDataHelperClass.NewFoodPicturePath = string.Empty;
                await ViewServices.PopUpManager.PopInfoAsync("Refeição registrada com sucesso!");
                await _navigation.PopAsync();
            }
            // Edição de uma refeição
            else
            {
                AppDataHelperClass.MealToEdit.FoodItems = FoodItems.ToList();
                AppDataHelperClass.SaveChangesOnMeals();
                AppDataHelperClass.MealToEdit = null;
                await ViewServices.PopUpManager.PopInfoAsync("Refeição editada com sucesso!");
                await _navigation.PopAsync();
            }
        }

        [RelayCommand]
        private void Appearing()
        {
            try
            {
                if (AppDataHelperClass.DetectedFoodItems != null && AppDataHelperClass.DetectedFoodItems.Count > 0)
                {
                    AppDataHelperClass.MealToEdit = null;

                    foreach (FoodItem food in AppDataHelperClass.DetectedFoodItems)
                    {
                        FoodItems.Add(food);
                    }
                    OnPropertyChanged(nameof(FoodItemsQuantity));
                    OnPropertyChanged(nameof(KiloCaloriesRound));
                }
                else if (AppDataHelperClass.MealToEdit != null)
                {
                    AppDataHelperClass.DetectedFoodItems = null;
                    AppDataHelperClass.NewFoodPicturePath = AppDataHelperClass.MealToEdit.MealPicturePath;

                    foreach (FoodItem food in AppDataHelperClass.MealToEdit.FoodItems)
                    {
                        FoodItems.Add(food);
                    }

                    OnPropertyChanged(nameof(FoodItemsQuantity));
                    OnPropertyChanged(nameof(KiloCaloriesRound));
                    OnPropertyChanged(nameof(HasPictureAnalysisSource));
                    OnPropertyChanged(nameof(PictureAnalysisSource));
                }
                else
                {
                    AppDataHelperClass.DetectedFoodItems = null;
                    AppDataHelperClass.MealToEdit = null;
                    AppDataHelperClass.NewFoodPicturePath = string.Empty;
                }

            }
            catch (Exception ex)
            {
                ViewServices.PopUpManager.PopErrorAsync("Erro ao carregar os dados");
            }


        }

        [RelayCommand]
        private void Disappearing()
        {

        }

        [RelayCommand]
        private async Task EditItem(FoodItem item)
        {
            AddTacoFoodItemPopup addFoodItemPopup = new AddTacoFoodItemPopup(item);
            await Application.Current.MainPage.ShowPopupAsync(addFoodItemPopup);

            if (addFoodItemPopup.Confirmed)
            {
                if (addFoodItemPopup.SelectedItem != null)
                {
                    item.Name = addFoodItemPopup.SelectedItem.Nome;
                    item.Portion = addFoodItemPopup.InputPortion;
                    item.KiloCalories = addFoodItemPopup.InputCalories;

                    UpdateFoodItemsList();
                }
                else
                    return;
            }
        }

        [RelayCommand]
        private async void DeleteItem(FoodItem item)
        {
            if (await ViewServices.PopUpManager.PopYesOrNoAsync("Deletar item", $"Deseja realmente deletar \"{item.Name}\" da refeição?"))
            {
                FoodItems.Remove(item);
                UpdateFoodItemsList();
            }
        }

        /// <summary>
        /// Método criado como forma de correção de atualização nativa da ObservableCollection,
        /// pois a atualização removendo, editando e adicionando itens, gera alguns problemas
        /// de renderização (falsos duplicados, itens não renderizados, etc). Esse método
        /// basicamente remove todos os itens e os adiciona novamente, dessa forma a interface
        /// funciona da forma como deveria.
        /// </summary>
        private void UpdateFoodItemsList()
        {
            List<FoodItem> updatedFooditems = FoodItems.ToList();

            FoodItems.Clear();

            foreach (FoodItem foodItem in updatedFooditems)
            {
                FoodItems.Add(foodItem);
            }

            OnPropertyChanged(nameof(FoodItemsQuantity));
            OnPropertyChanged(nameof(KiloCaloriesRound));
        }
    }
}
