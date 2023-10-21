using CommunityToolkit.Maui.Views;

namespace NutriLens.Views.Popups;

public partial class LoadingPopup : Popup
{
	public LoadingPopup(string message)
	{
		InitializeComponent();
		lblMessage.Text = message;
	}

}