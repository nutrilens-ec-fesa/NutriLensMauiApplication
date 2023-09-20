using Camera.MAUI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NutriLens.ViewModels
{
    public partial class CameraPageVm : ObservableObject
    {
        private INavigation _navigation;
        private CameraView _camera;

        public CameraPageVm(INavigation navigation, CameraView camera) 
        {
            _navigation = navigation;
            _camera = camera;
        }

        [RelayCommand]
        private void TakePicture()
        {
            ImageSource photo = _camera.SnapShot;
        }
    }
}
