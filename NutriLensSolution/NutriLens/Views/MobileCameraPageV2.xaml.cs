using CommunityToolkit.Mvvm.Input;
using NutriLens.Entities;
using NutriLens.Services;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace NutriLens.Views;

public partial class MobileCameraPageV2 : ContentPage
{
	public MobileCameraPageV2()
	{
		InitializeComponent();
	}

    private async void BtnSavePicture_Clicked(object sender, EventArgs e)
    {
        if (cameraView.SnapShot != null)
        {
            DateTime dateTimeNow = DateTime.Now;

            // Template de arquivo: nlpYYYYMMDDHHmmss.png
            string fileName = $"nlp{dateTimeNow.Year:D4}{dateTimeNow.Month:D2}{dateTimeNow.Day:D2}{dateTimeNow.Hour:D2}{dateTimeNow.Minute:D2}{dateTimeNow.Day:D2}.png";

            // Obter o ImagePath
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            await cameraView.SaveSnapShot(Camera.MAUI.ImageFormat.PNG, Path.Combine(FileSystem.AppDataDirectory, fileName));

            await ViewServices.PopUpManager.PopInfoAsync($"Imagem '{fileName}' salva com sucesso!");

            // Chama a função de upload com o caminho da imagem
            string resultadoUpload = DaoHelperClass.UploadImage(filePath);

            await ViewServices.PopUpManager.PopInfoAsync($"Imagem '{resultadoUpload}' salva no MongoDB!");

            await Navigation.PopAsync();
        }
        else
            await ViewServices.PopUpManager.PopErrorAsync("Tire uma foto antes de poder salvar!");
    }
}