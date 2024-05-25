using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileAddCustomFoodItemPage : ContentPage, IAddCustomFoodItemPage
{
	public MobileAddCustomFoodItemPage()
	{
		InitializeComponent();
		BindingContext = new AddCustomFoodItemVm(Navigation);
	}
}