using Camera.MAUI;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileBarCodePage : ContentPage, IBarCodePage
{
    public MobileBarCodePage()
    {
        InitializeComponent();
        BindingContext = new BarCodePageVm(Navigation);
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

    private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ViewServices.PopUpManager.PopInfoAsync($"{args.Result[0].BarcodeFormat}: {args.Result[0].Text}");
        });
    }
}