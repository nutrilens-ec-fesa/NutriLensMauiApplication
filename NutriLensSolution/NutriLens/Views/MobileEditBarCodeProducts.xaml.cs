using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileEditBarCodeProducts : ContentPage, IEditBarCodeProductsPage
{
	public MobileEditBarCodeProducts()
	{
		InitializeComponent();
		BindingContext = new EditBarCodeProductsVm(Navigation);
	}
}