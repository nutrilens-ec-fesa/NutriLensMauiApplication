using NutriLens.Entities;
using NutriLens.Services;
using ExceptionLibrary; 

namespace NutriLens.Views;

public partial class MobileCameraPageV2 : ContentPage
{
    public MobileCameraPageV2()
    {
        InitializeComponent();
    }

    private async void BtnSavePicture_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (EntitiesHelperClass.HasTempPicture)
            {
                byte[] img = File.ReadAllBytes(Path.Combine(FileSystem.AppDataDirectory, EntitiesHelperClass.GetTempPicture()));

                string filePath = NewPictureFilePath();

                File.WriteAllBytes(filePath, img);

                EntitiesHelperClass.ShowLoading("Sincronizando imagem na nuvem...");

                // Chama a função de upload com o caminho da imagem
                await Task.Run(() => DaoHelperClass.UploadImage(filePath));

                await ViewServices.PopUpManager.PopInfoAsync($"Imagem '{Path.GetFileName(filePath)}' salva com sucesso!");

                EntitiesHelperClass.CloseLoading();

                EntitiesHelperClass.ShowLoading("Realizando análise dos alimentos...");

                string resultadoAnalise = string.Empty;

                // Chama a função de upload com o caminho da imagem
                await Task.Run(() => resultadoAnalise = DaoHelperClass.GetFoodVisionAnalisysByLocalPath(filePath));

                await ViewServices.PopUpManager.PopPersonalizedAsync("Alimentos identificados", resultadoAnalise, "OK");

                EntitiesHelperClass.CloseLoading();

                EntitiesHelperClass.DeleteTempPictures();

                await Navigation.PopAsync();
            }
            else
                await ViewServices.PopUpManager.PopErrorAsync("Tire uma foto antes de poder salvar!");
        }
        catch(Exception ex)
        {
            await ViewServices.PopUpManager.PopErrorAsync("Houve algum erro para salvar a foto.\n\n" + ExceptionManager.ExceptionMessage(ex));
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
        catch(Exception ex)
        {
            await ViewServices.PopUpManager.PopErrorAsync("Houve algum erro para tirar a foto.\n\n" + ExceptionManager.ExceptionMessage(ex));
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
}