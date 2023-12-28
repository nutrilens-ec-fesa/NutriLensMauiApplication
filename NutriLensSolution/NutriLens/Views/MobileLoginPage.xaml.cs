using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileLoginPage : ContentPage, ILoginPage
{
	public MobileLoginPage()
	{
		InitializeComponent();
		BindingContext = new LoginPageVm(Navigation);
	}
}