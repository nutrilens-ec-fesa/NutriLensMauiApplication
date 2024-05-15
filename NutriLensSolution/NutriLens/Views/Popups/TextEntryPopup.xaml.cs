using CommunityToolkit.Maui.Views;

namespace NutriLens.Views.Popups;

public partial class TextEntryPopup : Popup
{
	public string Entry { get; set; }

	public TextEntryPopup(string entryTitle)
	{
		InitializeComponent();
		lblEntryTitle.Text = entryTitle;
	}

    private void btnConfirmEntry_Clicked(object sender, EventArgs e)
    {
		Entry = entry.Text;
		CloseAsync();
    }
}