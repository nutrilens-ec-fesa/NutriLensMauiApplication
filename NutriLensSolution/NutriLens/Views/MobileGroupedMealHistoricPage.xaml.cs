using NutriLens.Entities;
using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileGroupedMealHistoricPage : ContentPage, IGroupedMealHistoricPage
{
	public MobileGroupedMealHistoricPage(MealHistoryFilter mealHistoryFilter)
    {
        InitializeComponent();
        BindingContext = new GroupedMealHistoricPageVm(Navigation, mealHistoryFilter);
    }
}
