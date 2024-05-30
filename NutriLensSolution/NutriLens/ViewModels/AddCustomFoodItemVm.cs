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

        public int FoodTypeIndex { get; set; }
        public int GlutenOptionIndex { get; set; }
        public int LactoseOptionIndex { get; set; }

        public ObservableCollection<string> PossibleFoodTypes { get; set; }
        public ObservableCollection<string> YesOrNoOptions { get; set; }

        public enum YesOrNoOptionsEnum
        {
            Uninformed,
            Yes,
            No
        }

        public enum FoodTypeEnum
        {
            Uninformed,
            Solid,
            Liquid
        }

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
                "Sólido (em gramas)",
                "Bebida (em mls)"
            };

            YesOrNoOptions = new ObservableCollection<string>()
            {
                "Não informado",
                "Sim",
                "Não"
            };

            if (CustomTacoItem.Liquid == null)
                FoodTypeIndex = (int)FoodTypeEnum.Uninformed;
            else if ((bool)CustomTacoItem.Liquid)
                FoodTypeIndex = (int)FoodTypeEnum.Liquid;
            else
                FoodTypeIndex = (int)FoodTypeEnum.Solid;

            if (CustomTacoItem.Gluten == null)
                GlutenOptionIndex = (int)YesOrNoOptionsEnum.Uninformed;
            else if ((bool)CustomTacoItem.Gluten)
                GlutenOptionIndex = (int)YesOrNoOptionsEnum.Yes;
            else
                GlutenOptionIndex = (int)YesOrNoOptionsEnum.Uninformed;

            if (CustomTacoItem.Lactose == null)
                LactoseOptionIndex = (int)YesOrNoOptionsEnum.Uninformed;
            else if ((bool)CustomTacoItem.Lactose)
                LactoseOptionIndex = (int)YesOrNoOptionsEnum.Yes;
            else
                LactoseOptionIndex = (int)YesOrNoOptionsEnum.No;
        }

        [RelayCommand]
        public async Task ConfirmProduct()
        {
            EntitiesHelperClass.ShowLoading($"Registrando produto {CustomTacoItem.Nome}");

            bool result = false;
            string verb;

            if (string.IsNullOrEmpty(CustomTacoItem.Nome))
            {
                await ViewServices.PopUpManager.PopErrorAsync("Nome do alimento não informado. Preencha o nome do alimento.");
                return;
            }

            switch ((FoodTypeEnum)FoodTypeIndex)
            {
                case FoodTypeEnum.Uninformed:
                    CustomTacoItem.Liquid = null;
                    break;
                case FoodTypeEnum.Solid:
                case FoodTypeEnum.Liquid:
                    CustomTacoItem.Liquid = (FoodTypeEnum)FoodTypeIndex == FoodTypeEnum.Liquid;
                    break;
            }

            switch ((YesOrNoOptionsEnum)GlutenOptionIndex)
            {
                case YesOrNoOptionsEnum.Uninformed:
                    CustomTacoItem.Gluten = null;
                    break;
                case YesOrNoOptionsEnum.Yes:
                case YesOrNoOptionsEnum.No:
                    CustomTacoItem.Gluten = (YesOrNoOptionsEnum)GlutenOptionIndex == YesOrNoOptionsEnum.Yes;
                    break;
            }

            switch ((YesOrNoOptionsEnum)LactoseOptionIndex)
            {
                case YesOrNoOptionsEnum.Uninformed:
                    CustomTacoItem.Lactose = null;
                    break;
                case YesOrNoOptionsEnum.Yes:
                case YesOrNoOptionsEnum.No:
                    CustomTacoItem.Lactose = (YesOrNoOptionsEnum)LactoseOptionIndex == YesOrNoOptionsEnum.Yes;
                    break;
            }

            TacoItem normalizedTacoItem = GetNormalizedTacoItem(CustomTacoItem);

            if (_addProduct)
            {
                verb = "adicionado";
                await Task.Run(() => DaoHelperClass.InsertTacoItem(normalizedTacoItem));
            }
            else
            {
                verb = "alterado";
                await Task.Run(() => DaoHelperClass.UpdateTacoItem(normalizedTacoItem));
            }

            EntitiesHelperClass.CloseLoading();

            await ViewServices.PopUpManager.PopInfoAsync($"Alimento {verb} com sucesso!");

            EntitiesHelperClass.ShowLoading("Verificando atualização dos alimentos");

            await Task.Run(async () => await AppDataHelperClass.CheckTacoUpdate());

            await EntitiesHelperClass.CloseLoading();

            await _navigation.PopAsync();
        }

        public TacoItem GetNormalizedTacoItem(TacoItemEntry unormalizedTacoItem)
        {
            if (unormalizedTacoItem.Portion == 100)
                return unormalizedTacoItem;

            double portionDouble = unormalizedTacoItem.Portion;

            TacoItem normalizedTacoItem = new()
            {
                Nome = unormalizedTacoItem.Nome,
                EnergiaKcal = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.EnergiaKcal)) * 100 / portionDouble),
                EnergiaKj = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.EnergiaKcal)) * 100 / portionDouble) * NutriLensClassLibrary.Entities.Constants.kcalToKJFactor,
                Umidade = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Umidade)) * 100 / portionDouble),
                Proteina = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Proteina)) * 100 / portionDouble),
                Lipideos = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Lipideos)) * 100 / portionDouble),
                Colesterol = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Colesterol)) * 100 / portionDouble),
                Carboidrato = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Carboidrato)) * 100) / portionDouble,
                FibraAlimentar = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.FibraAlimentar)) * 100 / portionDouble),
                Cinzas = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Cinzas)) * 100 / portionDouble),
                Calcio = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Calcio)) * 100 / portionDouble),
                Magnesio = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Magnesio)) * 100 / portionDouble),
                Manganes = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Manganes)) * 100 / portionDouble),
                Fosforo = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Fosforo)) * 100 / portionDouble),
                Ferro = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Ferro)) * 100 / portionDouble),
                Sodio = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Sodio)) * 100 / portionDouble),
                Potassio = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Potassio)) * 100 / portionDouble),
                Cobre = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Cobre)) * 100 / portionDouble),
                Zinco = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Zinco)) * 100 / portionDouble),
                Retinol = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Retinol)) * 100 / portionDouble),
                RE = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.RE)) * 100 / portionDouble),
                RAE = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.RAE)) * 100 / portionDouble),
                Tiamina = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Tiamina)) * 100 / portionDouble),
                Riboflavina = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Riboflavina)) * 100 / portionDouble),
                Piridoxina = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Piridoxina)) * 100 / portionDouble),
                Niacina = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.Niacina)) * 100 / portionDouble),
                VitaminaC = (unormalizedTacoItem.GetValue(nameof(unormalizedTacoItem.VitaminaC)) * 100 / portionDouble),
                Liquid = unormalizedTacoItem.Liquid,
                Gluten = unormalizedTacoItem.Gluten,
                Lactose = unormalizedTacoItem.Lactose,
                TacoOriginal = false
            };

            return normalizedTacoItem;
        }
    }
}
