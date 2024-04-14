using NutriLens.ViewInterfaces;
using NutriLens.ViewModels;

namespace NutriLens.Views;

public partial class MobileManualInputPage : ContentPage, IManualInputPage
{
	public MobileManualInputPage()
	{
		InitializeComponent();
		BindingContext = new ManualInputPageVm(Navigation);
	}

	public void DeleteItem(object sender, EventArgs e)
	{
		string a = "riufheruog";
	}

    private void ImageButton_Clicked()
    {

    }
}