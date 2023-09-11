using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileFlyoutPage : ContentPage, IFlyoutPage
{
	public MobileFlyoutPage()
	{
		InitializeComponent();
		BindingContext = new FlyoutPageVm(Navigation);
	}
}