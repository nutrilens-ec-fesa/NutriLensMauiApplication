using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace NutriLens.Views.Popups;

public partial class AddFoodItemPopup : Popup
{
    public bool Confirmed { get; set; }

    public string InputItem { get => inputItem.Text; }
    public string InputPortion { get => inputPortion.Text; }
    public double InputCalories { get => double.Parse(inputCalories.Text); }

    public AddFoodItemPopup()
    {
        InitializeComponent();
    }

    private async void BtnConfirmItem_Clicked(object sender, EventArgs e)
    {
        Confirmed = true;
        await CloseAsync();
    }
}