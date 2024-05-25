using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Services;
using NutriLensClassLibrary.Models;
using System.Collections.ObjectModel;

namespace NutriLens.ViewModels
{
    public partial class AddCustomFoodItemVm : ObservableObject
    {
        private INavigation _navigation;

        private bool _addProduct;

        public TacoItemEntry CustomTacoItem { get; set; }

        public string PageTitle { get; set; }

        public ObservableCollection<string> PossibleFoodTypes { get; set; }
        public ObservableCollection<string> YesOrNoOptions { get; set; }

        public AddCustomFoodItemVm(INavigation navigation)
        {
            _navigation = navigation;
            _addProduct = true;
            PageTitle = "Adicionar novo alimento";
            CustomTacoItem = new TacoItemEntry();

            FillPickers();
        }

        public AddCustomFoodItemVm(INavigation navigation, TacoItemEntry tacoItemEntry)
        {
            _navigation = navigation;
            _addProduct = false;
            PageTitle = "Editar alimento";
            CustomTacoItem = tacoItemEntry;

            FillPickers();
        }

        public void FillPickers()
        {
            PossibleFoodTypes = new ObservableCollection<string>()
            {
                "Não informado",
                "Comestível",
                "Bebida"
            };

            YesOrNoOptions = new ObservableCollection<string>()
            {
                "Não informado",
                "Sim",
                "Não"
            };

            if (CustomTacoItem.Liquid != null && (bool)CustomTacoItem.Liquid)
                CustomTacoItem.FoodType = "Bebida";
            else
                CustomTacoItem.FoodType = "Comestível";

            if (CustomTacoItem.Gluten != null && (bool)CustomTacoItem.Gluten)
                CustomTacoItem.Gluten = false; // "TODO";
        }

        [RelayCommand]
        public async Task ConfirmProduct()
        {
            EntitiesHelperClass.ShowLoading($"Registrando produto {CustomTacoItem.Nome}");

            bool result = false;
            string verb;

            if (_addProduct)
            {
                verb = "adicionado";
                await Task.Run(() => DaoHelperClass.InsertTacoItem(CustomTacoItem));
            }
            else
            {
                verb = "alterado";
                await Task.Run(() => DaoHelperClass.UpdateTacoItem(CustomTacoItem));
            }

            EntitiesHelperClass.CloseLoading();

            await ViewServices.PopUpManager.PopInfoAsync($"Alimento {verb} com sucesso!");

            await _navigation.PopAsync();
        }
    }
}
