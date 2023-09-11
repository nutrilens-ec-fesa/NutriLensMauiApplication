using CommunityToolkit.Mvvm.ComponentModel;

namespace NutriLens.ViewModels
{
    internal class CameraPageVm : ObservableObject
    {
        private INavigation _navigation;

        public CameraPageVm(INavigation navigation) 
        {
            _navigation = navigation;
        }
    }
}
