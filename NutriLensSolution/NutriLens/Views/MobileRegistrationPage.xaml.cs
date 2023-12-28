using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileRegistrationPage : ContentPage, IRegistrationPage
{
	public MobileRegistrationPage()
	{
		InitializeComponent();
		BindingContext = new RegistrationPageVm(Navigation);
	}
}