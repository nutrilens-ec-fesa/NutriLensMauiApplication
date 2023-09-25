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
}