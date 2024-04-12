using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLensClassLibrary.Models;

namespace NutriLens.ViewModels
{
    public partial class AddBarcodeProductVm : ObservableObject
    {
        private INavigation _navigation;

        /// <summary>
        /// Produto a ser adicionado ou editado
        /// </summary>
        public BarcodeItemEntry BarcodeItem { get; set; }

        /// <summary>
        /// Texto da Navigation Bar da página
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// Booleana para indicar se é um processo de adicionar produto (true), ou de
        /// editar produto (false)
        /// </summary>
        private bool _addProduct;

        public AddBarcodeProductVm(INavigation navigation, string barcode)
        {
            _navigation = navigation;
            _addProduct = true;
            PageTitle = "Adicionar novo produto";
            BarcodeItem = new BarcodeItemEntry
            {
                Barcode = barcode
            };
        }

        public AddBarcodeProductVm(INavigation navigation, BarcodeItemEntry barcodeItem)
        {
            _navigation = navigation;
            _addProduct = false;
            PageTitle = "Editar produto";
            BarcodeItem = barcodeItem;
        }

        [RelayCommand]
        public async Task ConfirmProduct()
        {
            EntitiesHelperClass.ShowLoading($"Registrando produto {BarcodeItem.ProductName}");

            bool result = false;
            string verb;

            if (_addProduct)
            {
                verb = "adicionado";
                await Task.Run(() => DaoHelperClass.InsertBarCodeItem(BarcodeItem));
            }
            else
            {
                verb = "alterado";
                await Task.Run(() => DaoHelperClass.UpdateBarCodeItem(BarcodeItem));
            }

            EntitiesHelperClass.CloseLoading();

            await ViewServices.PopUpManager.PopInfoAsync($"Produto {verb} com sucesso!");

            await _navigation.PopAsync();
        }
    }
}
