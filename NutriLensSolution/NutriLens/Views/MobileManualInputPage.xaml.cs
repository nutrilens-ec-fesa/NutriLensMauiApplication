using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileManualInputPage : ContentPage, IManualInputPage
{
	public MobileManualInputPage()
	{
		InitializeComponent();
		BindingContext = new ManualInputPageVm(Navigation);
	}
}