using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileCameraPage : ContentPage, ICameraPage
{
	public MobileCameraPage()
	{
		InitializeComponent();
		BindingContext = new CameraPageVm(Navigation, cameraView);
	}

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        cameraView.Camera = cameraView.Cameras.First();

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await cameraView.StopCameraAsync();
            await cameraView.StartCameraAsync();
        });

    }

}