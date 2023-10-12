using Camera.MAUI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NutriLens.Services;

namespace NutriLens.ViewModels
{
    public partial class CameraPageVm : ObservableObject
    {
        private INavigation _navigation;
        private CameraView _camera;

        public ImageSource TakenPictureSource { get; set; }
        public bool TakenPictureVisible { get; set; }

        public CameraPageVm(INavigation navigation, CameraView camera) 
        {
            _navigation = navigation;
            _camera = camera;
        }

        [RelayCommand]
        private void TakePicture()
        {
            if (TakenPictureVisible)
            {

                TakenPictureVisible = false;
                OnPropertyChanged(nameof(TakenPictureVisible));

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    _camera.IsVisible = true;
                    //await Task.Delay(200);
                    //await _camera.StopCameraAsync();
                    //await _camera.StartCameraAsync();
                });
            }
            else
            {
                TakenPictureSource = _camera.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
                _camera.SaveSnapShot(Camera.MAUI.ImageFormat.PNG, Path.Combine(FileSystem.AppDataDirectory, "testimg.png"));
                OnPropertyChanged(nameof(TakenPictureSource));

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    //await _camera.StopCameraAsync();
                    //await Task.Delay(200);
                    _camera.IsVisible = false;
                });

                TakenPictureVisible = true;
                OnPropertyChanged(nameof(TakenPictureVisible));
            }
        }

        [RelayCommand]
        private void TakeAnother()
        {
            TakePicture();
        }

        [RelayCommand]
        private async void ConfirmPicture()
        {
            await ViewServices.PopUpManager.PopInfoAsync("Foto registrada com sucesso!");

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _camera.StopCameraAsync();
                //await Task.Delay(200);
                _camera.IsVisible = true;
            });

            await _navigation.PopAsync();
        }
    }
}
