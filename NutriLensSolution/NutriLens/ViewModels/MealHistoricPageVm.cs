using CommunityToolkit.Mvvm.ComponentModel;

namespace NutriLens.ViewModels
{
    public partial class MealHistoricPageVm : ObservableObject
    {
        private INavigation _navigation;

        public MealHistoricPageVm(INavigation navigation)
        {
            _navigation = navigation;
        }
    }
}
