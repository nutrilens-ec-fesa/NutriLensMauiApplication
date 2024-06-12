using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using NutriLens.Views.Popups;
using NutriLensClassLibrary.Models;
using System.Collections.ObjectModel;
using System.Text;

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

        public int FoodItemsQuantity { get => FoodItems.Count; }
        public double KiloCalories { get => FoodItems.Sum(x => x.KiloCalories); }
        public double KiloCaloriesRound { get => Math.Round(KiloCalories, 2); }
        public string EnergeticUnit { get => AppConfigHelperClass.EnergeticUnit.ToString(); }
        public bool HasPictureAnalysisSource { get => !string.IsNullOrEmpty(AppDataHelperClass.NewFoodPicturePath); }
        public string PictureAnalysisSource { get => AppDataHelperClass.NewFoodPicturePath; }

        public ManualInputPageVm(INavigation navigation)
        {
            _navigation = navigation;
            FoodItems = new ObservableCollection<FoodItem>();
        }

        //public ManualInputPageVm(INavigation navigation, List<FoodItem> foods)
        //{
        //    _navigation = navigation;
        //    FoodItems = new ObservableCollection<FoodItem>();
        //    foreach (FoodItem food in foods)
        //    {
        //        FoodItems.Add(food);
        //    }
        //}

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
        public async Task AddNewItemFromTaco(TacoItem tacoItem = null)
        {
            AddTacoFoodItemPopup addFoodItemPopup;

            if (tacoItem == null)
                addFoodItemPopup = new AddTacoFoodItemPopup(_navigation);
            else
                addFoodItemPopup = new AddTacoFoodItemPopup(_navigation, tacoItem);

            await Application.Current.MainPage.ShowPopupAsync(addFoodItemPopup);

            if (AppDataHelperClass.AddTacoItemRequested)
            {
                AppDataHelperClass.AddTacoItemRequested = false;
                await _navigation.PushAsync(ViewServices.ResolvePage<IAddCustomFoodItemPage>());
            }

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

                if (!await DcntAndIntolerancesCheck(newMeal.FoodItems))
                    return;

                AppDataHelperClass.AddMeal(newMeal);
                AppDataHelperClass.DetectedFoodItems?.Clear();
                AppDataHelperClass.NewFoodPicturePath = string.Empty;
                await ViewServices.PopUpManager.PopInfoAsync("Refeição registrada com sucesso!");
                await _navigation.PopAsync();
            }
            // Edição de uma refeição
            else
            {
                if (!await DcntAndIntolerancesCheck(FoodItems.ToList()))
                    return;

                AppDataHelperClass.MealToEdit.FoodItems = FoodItems.ToList();
                await AppDataHelperClass.UpdateMeal(AppDataHelperClass.MealToEdit);
                AppDataHelperClass.MealToEdit = null;
                await ViewServices.PopUpManager.PopInfoAsync("Refeição editada com sucesso!");
                await _navigation.PopAsync();
            }
        }

        [RelayCommand]
        private async void Appearing()
        {
            try
            {

                if (AppDataHelperClass.AddedTacoItem != null)
                {
                    await AddNewItemFromTaco(AppDataHelperClass.AddedTacoItem);
                    AppDataHelperClass.AddedTacoItem = null;
                    return;
                }

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

                    if (File.Exists(AppDataHelperClass.MealToEdit.MealPicturePath))
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
        private async Task ListItem(FoodItem item)
        {
            ShowTacoFoodItemPopup showFoodItemPopup = new ShowTacoFoodItemPopup(item);
            await Application.Current.MainPage.ShowPopupAsync(showFoodItemPopup);

        }

        [RelayCommand]
        private async Task EditItem(FoodItem item)
        {
            AddTacoFoodItemPopup addFoodItemPopup = new AddTacoFoodItemPopup(_navigation, item);
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

        /// <summary>
        /// Verifica os itens alimentícios da refeição e informa ao usuário caso haja algum item que interfira nas configurações
        /// </summary>
        /// <returns>Retorna verdadeiro caso o usuário tenha optado por continuar</returns>
        private async Task<bool> DcntAndIntolerancesCheck(List<FoodItem> foodItems)
        {
            StringBuilder glutenWarning = new();

            if (AppDataHelperClass.UserInfo.GlutenIntolerant)
            {
                List<FoodItem> glutenFoodItems = foodItems
                    .Where(x => x.TacoFoodItem.Gluten != null && (bool)x.TacoFoodItem.Gluten)
                    .ToList();

                if (glutenFoodItems.Count > 0)
                {
                    glutenWarning.Append("Atenção! Os seguintes alimentos podem ter glúten em sua composição: " + Environment.NewLine);

                    foreach (FoodItem glutenFoodItem in glutenFoodItems)
                    {
                        glutenWarning.Append(Environment.NewLine + glutenFoodItem.Name);
                    }

                    glutenWarning.Append(Environment.NewLine + Environment.NewLine + "Deseja continuar?");
                }
            }

            StringBuilder lactoseWarning = new();

            if (AppDataHelperClass.UserInfo.LactoseIntolerant)
            {
                List<FoodItem> lactoseFoodItems = foodItems
                    .Where(x => x.TacoFoodItem.Lactose != null && (bool)x.TacoFoodItem.Lactose)
                    .ToList();

                if (lactoseFoodItems.Count > 0)
                {
                    lactoseWarning.Append("Atenção! Os seguintes alimentos podem ter lactose em sua composição: " + Environment.NewLine);

                    foreach (FoodItem lactoseFoodItem in lactoseFoodItems)
                    {
                        lactoseWarning.Append(Environment.NewLine + lactoseFoodItem.Name);
                    }

                    lactoseWarning.Append(Environment.NewLine + Environment.NewLine + "Deseja continuar?");
                }
            }

            MealListClass todayMeals = new MealListClass(AppDataHelperClass.GetTodayMeals());
            MealListClass actualMeal = new MealListClass(new List<Meal> { new Meal { FoodItems = foodItems } });

            if (AppDataHelperClass.UserInfo.Diabetes)
            {
                double todayTotalCarbohydratesConsumption = todayMeals.TotalCarbohydratesConsumption();
                double actualMealCarbohydrates = actualMeal.TotalCarbohydratesConsumption();
                double totalCarbohydratesAfterMeal = todayTotalCarbohydratesConsumption + actualMealCarbohydrates;

                if (actualMealCarbohydrates > 0)
                {
                    if (todayTotalCarbohydratesConsumption > AppDataHelperClass.UserInfo.DailyCarbohydrateGoal)
                    {
                        if (!await ViewServices.PopUpManager.PopYesOrNoAsync("Aviso de diabetes", "Essa refeição possui carboidratos e você já ultrapassou o limite de carboidratos para hoje, deseja continuar?"))
                            return false;
                    }

                    else if (totalCarbohydratesAfterMeal > AppDataHelperClass.UserInfo.DailyCarbohydrateGoal && !await ViewServices.PopUpManager.PopYesOrNoAsync("Aviso de diabetes", "Essa refeição possui carboidratos e, com essa refeição, vai ultrapassar o limite estabelecido, deseja continuar?"))
                        return false;
                }
            }

            if (AppDataHelperClass.UserInfo.Hipertension)
            {
                double todayTotalSodiumConsumption = todayMeals.TotalSodiumConsumption();
                double actualMealSodium = actualMeal.TotalSodiumConsumption();
                double totalSodiumAfterMeal = todayTotalSodiumConsumption + actualMealSodium;

                if (actualMealSodium > 0)
                {
                    if (todayTotalSodiumConsumption > AppDataHelperClass.UserInfo.DailySodiumGoal)
                    {
                        if (!await ViewServices.PopUpManager.PopYesOrNoAsync("Aviso de hipertensão", "Essa refeição possui sódio e você já ultrapassou o limite de sódio para hoje, deseja continuar?"))
                            return false;
                    }
                    else if (totalSodiumAfterMeal > AppDataHelperClass.UserInfo.DailySodiumGoal && !await ViewServices.PopUpManager.PopYesOrNoAsync("Aviso de hipertensão", "Essa refeição possui sódio e, com essa refeição, vai ultrapassar o limite estabelecido, deseja continuar?"))
                        return false;
                }
            }

            if (!string.IsNullOrEmpty(glutenWarning.ToString()) && !await ViewServices.PopUpManager.PopYesOrNoAsync("Aviso de glúten", glutenWarning.ToString()))
                return false;

            if (!string.IsNullOrEmpty(lactoseWarning.ToString()) && !await ViewServices.PopUpManager.PopYesOrNoAsync("Aviso de lactose", lactoseWarning.ToString()))
                return false;

            return true;
        }
    }
}
