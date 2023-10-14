using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using NutriLens.Entities;
using NutriLens.Services;
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

        string[] localPictures = files.Where(x => x.EndsWith(".png")).ToArray();

        int childrenIndex = 0;

        gridPictures.Children.Insert(childrenIndex++, new Label { Text = "Imagens no dispositivo", HorizontalOptions = LayoutOptions.Center });

        if (localPictures.Length == 0)
            gridPictures.Children.Insert(childrenIndex++, new Label { Text = "Ainda não foram tiradas fotos nesse dispositivo", HorizontalOptions = LayoutOptions.Center });
        else
        {
            foreach (string picture in localPictures)
            {
                Image img = new Image { Source = picture, Aspect = Aspect.AspectFit, HeightRequest = 300, WidthRequest = 200 };
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += async (s, e) =>
                {
                    if (await ViewServices.PopUpManager.PopYesOrNoAsync("Deletar imagem", "Deseja deletar a imagem?"))
                    {
                        File.Delete(picture);
                        gridPictures.Remove(img);
                    }
                };
                tapGestureRecognizer.NumberOfTapsRequired = 2;
                img.GestureRecognizers.Add(tapGestureRecognizer);
                gridPictures.Children.Insert(childrenIndex++, img);
            }
        }
    }

    private void BtnSyncDatabase_Clicked(object sender, EventArgs e)
    {
        BtnSyncDatabase.IsVisible = false;

        if (!Directory.Exists(UriAndPaths.databasePicturesPath))
            Directory.CreateDirectory(UriAndPaths.databasePicturesPath);

        DaoHelperClass.DownloadImages(UriAndPaths.databasePicturesPath);

        string[] cloudPictures = Directory.GetFiles(UriAndPaths.databasePicturesPath);

        gridPictures.Children.Add(new Label { Text = "Imagens em nuvem", HorizontalOptions = LayoutOptions.Center });

        if (cloudPictures.Length == 0)
            gridPictures.Children.Add(new Label { Text = "Não foram encontradas imagens em núvem", HorizontalOptions = LayoutOptions.Center });
        else
        {
            foreach (string picture in cloudPictures)
            {
                gridPictures.Children.Add(new Image { Source = picture, Aspect = Aspect.AspectFit, HeightRequest = 300, WidthRequest = 200 });
            }
        }
    }
}