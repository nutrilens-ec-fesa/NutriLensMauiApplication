using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileUserConfigPage : ContentPage, IUserConfigPage
{
	public MobileUserConfigPage()
	{
		InitializeComponent();
		BindingContext = new UserConfigPageVm(Navigation);
	}
}