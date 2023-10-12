using NutriLens.ViewInterfaces;

namespace NutriLens.Views;

public partial class MobilePicturesGridPage : ContentPage, IPicturesGridPage
{
	public MobilePicturesGridPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        string[] files = Directory.GetFiles(FileSystem.AppDataDirectory);
        string[] pictures = files.Where(x => x.EndsWith(".png")).ToArray();

        if (pictures.Length > 0)
        {
            //gridPictures = new Grid();

            foreach (string picture in pictures)
            {
                gridPictures.Children.Add(new Image { Source = picture, Aspect = Aspect.AspectFit, HeightRequest = 300, WidthRequest = 200 });
            }
        }
    }
}