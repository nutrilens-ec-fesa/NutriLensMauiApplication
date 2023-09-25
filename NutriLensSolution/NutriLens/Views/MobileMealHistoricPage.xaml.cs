using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileMealHistoricPage : ContentPage, IMealHistoricPage
{
	public MobileMealHistoricPage()
	{
		InitializeComponent();
		BindingContext = new MealHistoricPageVm(Navigation);
	}
}