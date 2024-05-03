using CommunityToolkit.Mvvm.Input;
using ExceptionLibrary;
using NutriLens.Entities;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using NutriLensClassLibrary.Models;

namespace NutriLens.Views;

public partial class MobileCameraPageV2 : ContentPage
{
    private INavigation _navigation;

    public MobileCameraPageV2(INavigation navigation)
    {
        InitializeComponent();
        _navigation = navigation;
    }

    private async void BtnSavePicture_Clicked(object sender, EventArgs e)
    {
        await ExecuteAnalysis();
    }

    private async void BtnImportPicture_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync();
            {
                if (result == null)
                {
                    return;
                }

                ImageSource imageSource = ImageSource.FromStream(() => result.OpenReadAsync().Result);

                ImgTakenSnapShot.Source = null;

                EntitiesHelperClass.DeleteTempPictures();

                string tempPictureFilePath = NewTempPictureFilePath();

                using (var stream = await result.OpenReadAsync())
                {
                    using (var fileStream = File.Create(tempPictureFilePath))
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
                // Construct a bitmap from the button image resource.
                //Bitmap bmp1 = new Bitmap(tempPictureFilePath);

                // Save the image as a GIF.
                //bmp1.Save(result.OpenReadAsync().Result, System.Drawing.Imaging.ImageFormat.Png);

                ImgTakenSnapShot.Source = tempPictureFilePath;

                //ImgTakenSnapShot.Source = imageSource;
                //this.imageEditor.IsVisible = true;
                //this.openGallery.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            await ViewServices.PopUpManager.PopErrorAsync("Houve algum erro para importar a foto.\n\n" + ExceptionManager.ExceptionMessage(ex));
        }
    }

    private async void BtnTakeSnapshot_Clicked(object sender, EventArgs e)
    {
        try
        {
            ImgTakenSnapShot.Source = null;

            EntitiesHelperClass.DeleteTempPictures();

            string tempPictureFilePath = NewTempPictureFilePath();

            await cameraView.SaveSnapShot(Camera.MAUI.ImageFormat.PNG, tempPictureFilePath);

            ImgTakenSnapShot.Source = tempPictureFilePath;
        }
        catch (Exception ex)
        {
            await ViewServices.PopUpManager.PopErrorAsync("Houve algum erro para tirar a foto.\n\n" + ExceptionManager.ExceptionMessage(ex));
        }
    }

    private async Task ExecuteAnalysis()
    {
        try
        {
            if (!EntitiesHelperClass.HasTempPicture)
                await ViewServices.PopUpManager.PopErrorAsync("Tire uma foto antes de poder salvar!");

            byte[] img = File.ReadAllBytes(Path.Combine(FileSystem.AppDataDirectory, EntitiesHelperClass.GetTempPicture()));

            string filePath = NewPictureFilePath();

            File.WriteAllBytes(filePath, img);

            EntitiesHelperClass.ShowLoading("Sincronizando imagem na nuvem...");

            string mongoImageId = string.Empty;

            // Chama a função de upload com o caminho da imagem
            await Task.Run(() => mongoImageId = DaoHelperClass.UploadImage(filePath));

            EntitiesHelperClass.CloseLoading();

            List<FoodItem> foodItems;

            while (true)
            {
                foodItems = await EntitiesHelperClass.GetAiAnalysisByMongoImageId(mongoImageId);

                if (foodItems == null)
                {
                    if (!await ViewServices.PopUpManager.PopYesOrNoAsync("Falha na identificação dos alimentos", "Deseja tentar novamente?"))
                        return;
                }
                else
                    break;
            }

            AppDataHelperClass.DetectedFoodItems = foodItems;
            AppDataHelperClass.NewFoodPicturePath = filePath;
            EntitiesHelperClass.DeleteTempPictures();
            await Navigation.PopAsync();
            await OpenManualInput();
        }
        catch (Exception ex)
        {
            await EntitiesHelperClass.CloseLoading();
            await ViewServices.PopUpManager.PopErrorAsync("Houve algum erro para salvar/analisar a foto.\n\n" + ExceptionManager.ExceptionMessage(ex));
        }
    }

    private string NewPictureFilePath()
    {
        DateTime dateTimeNow = DateTime.Now;
        string fileName = $"nlp{dateTimeNow.Year:D4}{dateTimeNow.Month:D2}{dateTimeNow.Day:D2}{dateTimeNow.Hour:D2}{dateTimeNow.Minute:D2}{dateTimeNow.Day:D2}.png";
        return Path.Combine(FileSystem.AppDataDirectory, fileName);
    }

    private string NewTempPictureFilePath()
    {
        string fileName = $"nlpTemp{DateTime.Now.Ticks}.png";
        return Path.Combine(FileSystem.AppDataDirectory, fileName);
    }

    [RelayCommand]
    private async Task OpenManualInput()
    {
        await _navigation.PushAsync(ViewServices.ResolvePage<IManualInputPage>());
    }

}