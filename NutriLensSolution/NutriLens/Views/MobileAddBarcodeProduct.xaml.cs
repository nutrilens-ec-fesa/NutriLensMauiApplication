using NutriLens.Models;
using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;
using NutriLensClassLibrary.Models;

namespace NutriLens.Views;

public partial class MobileAddBarcodeProduct : ContentPage, IAddBarcodeProductPage
{
	public MobileAddBarcodeProduct(string barcode)
	{
		InitializeComponent();
        BindingContext = new AddBarcodeProductVm(Navigation, barcode);
    }

    public MobileAddBarcodeProduct(BarcodeItemEntry barcodeItem)
    {
        InitializeComponent();
        BindingContext = new AddBarcodeProductVm(Navigation, barcodeItem);
    }
}