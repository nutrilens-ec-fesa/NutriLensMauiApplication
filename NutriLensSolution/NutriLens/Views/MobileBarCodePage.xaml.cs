using Camera.MAUI;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;
using Plugin.Maui.Audio;
using ZXing;

namespace NutriLens.Views;

public partial class MobileBarCodePage : ContentPage, IBarCodePage
{
    public MobileBarCodePage()
    {
        InitializeComponent();
        BindingContext = new BarCodePageVm(Navigation);
    }
}