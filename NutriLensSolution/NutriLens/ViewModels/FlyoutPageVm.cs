using CommunityToolkit.Mvvm.ComponentModel;

namespace NutriLens.ViewModels
{
    internal class FlyoutPageVm : ObservableObject
    {
        private INavigation _navigation;

        public FlyoutPageVm(INavigation navigation)
        {
            _navigation = navigation;
        }
    }
}
