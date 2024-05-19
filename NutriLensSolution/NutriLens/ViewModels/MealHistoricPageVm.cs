using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using NutriLens.Views.Popups;
using NutriLensClassLibrary.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace NutriLens.ViewModels
{
    internal partial class MealHistoricPageVm : INotifyPropertyChanged
    {
        private INavigation _navigation;

        public string HistoricPageName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Meal> Meals { get; set; }
        public ObservableCollection<MealListClass> MealsList { get; set; }
        public ObservableCollection<string> MealsListString { get; set; }
        public MealHistoricPageVm(INavigation navigation)
        {
            _navigation = navigation;
            MealsListString = new ObservableCollection<string>();
            MealsList = new ObservableCollection<MealListClass>();
        }

        [RelayCommand]
        private void Appearing()
        {
            List<Meal> meals;

            if (AppDataHelperClass.FilteredMealList != null)
            {
                HistoricPageName = AppDataHelperClass.FilteredMealList.MealListInfo;
                meals = AppDataHelperClass.FilteredMealList.MealList;
            }
            else
            {
                HistoricPageName = "Todas as refeições";
                meals = AppDataHelperClass.GetAllMeals();
            }

            if (meals.Count == 0)
            {
                ViewServices.PopUpManager.PopPersonalizedAsync("Sem refeições", "Não foram encontradas refeições registradas no dispositivo", "OK");
                _navigation.PopAsync();
                return;
            }

            Meals = new ObservableCollection<Meal>();

            foreach (Meal meal in meals.OrderByDescending(x => x.DateTime))
            {
                Meals.Add(meal);
                MealsList.Add(new MealListClass(new List<Meal>()));
                MealsList[^1].MealList.Add(meal);
            }

            //foreach (MealListClass meal in MealsList)
            //{
            //    string mealInfo = meal.MealList[0].ToString() + Environment.NewLine + Environment.NewLine;

            //    StringBuilder mealInfoBuilder = new StringBuilder(mealInfo);

            //    foreach (FoodItem foodItem in meal.MealList[0].FoodItems)
            //    {
            //        // Se for um item contido na TBCA
            //        if (foodItem.TbcaFoodItem != null)
            //        {
            //            mealInfoBuilder.Append($"   * {foodItem.TbcaFoodItem.Alimento} ({foodItem.Portion} g - {foodItem.KiloCalorieInfo}){Environment.NewLine}");
            //        }
            //        else
            //        {
            //            mealInfoBuilder.Append($"  * {foodItem.Name} ({foodItem.Portion} g - {foodItem.KiloCalorieInfo}){Environment.NewLine}");
            //        }
            //    }

            //    meal.MealPlusItemsInfo = mealInfoBuilder.ToString();
            //    MealsListString.Add(mealInfoBuilder.ToString());
            //}

            OnPropertyChanged(nameof(HistoricPageName));
            OnPropertyChanged(nameof(Meals));
            OnPropertyChanged(nameof(MealsList));
        }

        [RelayCommand]
        private async Task MealTapped(Meal item)
        {
            // TODO: Tomar ação para quando o item for clicado
            ShowTacoMealItemPopup showMealItemPopup = new ShowTacoMealItemPopup(item);
            await Application.Current.MainPage.ShowPopupAsync(showMealItemPopup);
        }

        [RelayCommand]
        private async Task EditItem(Meal item)
        {
            AppDataHelperClass.MealToEdit = item;

            if (item.FoodItems[0].BarcodeItemEntry != null)
                await _navigation.PushAsync(ViewServices.ResolvePage<IBarCodePage>());
            else
                await _navigation.PushAsync(ViewServices.ResolvePage<IManualInputPage>());
        }

        [RelayCommand]
        private async void DeleteItem(Meal item)
        {
            if (await ViewServices.PopUpManager.PopYesOrNoAsync("Deletar item", $"Deseja realmente deletar essa refeição?"))
            {
                AppDataHelperClass.RemoveMeal(item);
                Meals.Remove(item);
                UpdateMealsList();

                await ViewServices.PopUpManager.PopInfoAsync("Refeição deletada com sucesso!");
            }
        }

        /// <summary>
        /// Método criado como forma de correção de atualização nativa da ObservableCollection,
        /// pois a atualização removendo, editando e adicionando itens, gera alguns problemas
        /// de renderização (falsos duplicados, itens não renderizados, etc). Esse método
        /// basicamente remove todos os itens e os adiciona novamente, dessa forma a interface
        /// funciona da forma como deveria.
        /// </summary>
        private void UpdateMealsList()
        {
            List<Meal> updatedMeals = Meals.ToList();

            Meals.Clear();

            foreach (Meal meal in updatedMeals)
            {
                Meals.Add(meal);
            }
        }
    }
}
