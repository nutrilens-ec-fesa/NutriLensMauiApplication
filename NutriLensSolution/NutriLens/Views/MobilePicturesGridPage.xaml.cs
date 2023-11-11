using ExceptionLibrary;
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

        string[] localPictures = files.Where(x => x.EndsWith(".png") && !x.Contains("Temp")).ToArray();

        int childrenIndex = 0;

        gridPictures.Children.Insert(childrenIndex++, new Label { Text = "Imagens no dispositivo", Margin = new Thickness(0, 0, 0, 20), HorizontalOptions = LayoutOptions.Center });

        if (localPictures.Length == 0)
            gridPictures.Children.Insert(childrenIndex++, new Label { Text = "Ainda não foram tiradas fotos nesse dispositivo", HorizontalOptions = LayoutOptions.Center });
        else
        {
            foreach (string picture in localPictures)
            {
                Image img = new Image { Source = picture, Aspect = Aspect.AspectFill, Margin = new Thickness(0, 0, 0, 20), HeightRequest = 250, WidthRequest = 200 };
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

    private async void BtnSyncDatabase_Clicked(object sender, EventArgs e)
    {
        try
        {
            EntitiesHelperClass.ShowLoading("Sincronizando imagens...");

            BtnSyncDatabase.IsVisible = false;

            if (!Directory.Exists(UriAndPaths.databasePicturesPath))
                Directory.CreateDirectory(UriAndPaths.databasePicturesPath);

            await Task.Run(() =>
            {
                DaoHelperClass.DownloadImages(UriAndPaths.databasePicturesPath);
            });

            string[] cloudPictures = Directory.GetFiles(UriAndPaths.databasePicturesPath);

            gridPictures.Children.Add(new Label { Text = "Imagens em nuvem", Margin = new Thickness(0, 0, 0, 20), HorizontalOptions = LayoutOptions.Center });

            if (cloudPictures.Length == 0)
                gridPictures.Children.Add(new Label { Text = "Não foram encontradas imagens em nuvem", HorizontalOptions = LayoutOptions.Center });
            else
            {
                foreach (string picture in cloudPictures)
                {
                    gridPictures.Children.Add(new Image { Source = picture, Aspect = Aspect.AspectFill, Margin = new Thickness(0, 0, 0, 20), HeightRequest = 250, WidthRequest = 200 });
                }
            }
        }
        catch (Exception ex)
        {
            await ViewServices.PopUpManager.PopErrorAsync("Houve algum problema na obtenção das imagens em nuvem.\n\n" + ExceptionManager.ExceptionMessage(ex));
        }
        finally
        {
            EntitiesHelperClass.CloseLoading();
        }
    }
}