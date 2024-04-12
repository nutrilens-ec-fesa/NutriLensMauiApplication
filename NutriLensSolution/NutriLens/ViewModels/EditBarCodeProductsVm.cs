using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Models;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using NutriLensClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.QrCode.Internal;

namespace NutriLens.ViewModels
{
    public partial class EditBarCodeProductsVm : ObservableObject
    {
        private INavigation _navigation;

        public ObservableCollection<BarcodeItemEntry> BarCodeProducts { get; set; }

        public EditBarCodeProductsVm(INavigation navigation)
        {
            _navigation = navigation;
            BarCodeProducts = new ObservableCollection<BarcodeItemEntry>();
        }

        [RelayCommand]
        private async Task Appearing()
        {
            EntitiesHelperClass.ShowLoading("Carregando produtos...");

            List<BarcodeItemEntry> databaseProducts = null;

            await Task.Run(() => databaseProducts = DaoHelperClass.GetAllBarCodeItems());

            await EntitiesHelperClass.CloseLoading();

            BarCodeProducts.Clear();

            foreach (BarcodeItemEntry item in databaseProducts.OrderBy(x => x.ProductName))
            {
                BarCodeProducts.Add(item);
            }
        }

        [RelayCommand]
        private async void ProductTapped(BarcodeItemEntry barcodeItem)
        {
            if (await ViewServices.PopUpManager.PopYesOrNoAsync("Editar produto", $"Deseja editar o produto '{barcodeItem.ProductName}'?"))
                await _navigation.PushAsync(ViewServices.ResolvePage<IAddBarcodeProductPage>(barcodeItem));
        }
    }
}
