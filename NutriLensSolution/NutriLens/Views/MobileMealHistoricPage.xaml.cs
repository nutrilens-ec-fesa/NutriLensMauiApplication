using NutriLens.Entities;
using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileMealHistoricPage : ContentPage, IMealHistoricPage
{
	public MobileMealHistoricPage(MealHistoryFilter mealHistoryFilter)
	{
		InitializeComponent();
		BindingContext = new MealHistoricPageVm(Navigation, mealHistoryFilter);
	}
}