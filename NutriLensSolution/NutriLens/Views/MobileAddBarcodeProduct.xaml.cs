using NutriLens.Models;
using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileAddBarcodeProduct : ContentPage, IAddBarcodeProductPage
{
	public MobileAddBarcodeProduct(string barcode)
	{
		InitializeComponent();
        BindingContext = new AddBarcodeProductVm(Navigation, barcode);
    }

    public MobileAddBarcodeProduct(BarcodeItem barcodeItem)
    {
        InitializeComponent();
        BindingContext = new AddBarcodeProductVm(Navigation, barcodeItem);
    }
}