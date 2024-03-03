using NutriLens.Entities;
using NutriLens.Services;
using ExceptionLibrary;
using NutriLens.Models;
using CommunityToolkit.Mvvm.Input;
using NutriLens.ViewInterfaces;
using NutriLensClassLibrary.Models;
using Com.Google.Android.Exoplayer2.Metadata.Scte35;

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
                string identificados = string.Empty;
                string tbcaTeste = string.Empty;

                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

                List<RecognizedImageInfoTxtModel> alimentosTxt = new List<RecognizedImageInfoTxtModel>();
                List<RecognizedImageInfoModel> alimentosJson = new List<RecognizedImageInfoModel>();
                List<FoodItem> foods = new List<FoodItem>();

                // Chama a função de upload com o caminho da imagem
                await Task.Run(() =>
                {
                    resultadoAnalise = DaoHelperClass.GetFoodVisionAnalisysByLocalPath(filePath);
                    // Quando a análise estiver completa, sinalize o TaskCompletionSource
                    tcs.SetResult(true);
                });

                await tcs.Task;

                if (tcs.Task.IsCompleted)
                {
                    //Verifica se o texto de retorno do GPT veio em JSON ou não
                    if (resultadoAnalise.Contains('['))
                    {
                        alimentosJson = AppDataHelperClass.GetRecognizedImageInfoModel(resultadoAnalise);
                        identificados = AppDataHelperClass.GetRecognizedImageInfoText(alimentosJson);
                    }
                    else
                    {
                        alimentosTxt = AppDataHelperClass.GetRecognizedImageInfoTxtModel(resultadoAnalise);
                        identificados = AppDataHelperClass.GetRecognizedImageInfoText(alimentosTxt);
                        tbcaTeste = AppDataHelperClass.GetStringTbcaItemsByImageInfo(alimentosTxt);
                        foods = AppDataHelperClass.GetFoodItems(alimentosTxt);
                    }
                    
                }

                int telaEdicao = await ViewServices.PopUpManager.PopPersonalizedAsync("Verifique os alimentos identificados", identificados, "OK", "Editar");

                if(telaEdicao == 2)
                {
                    await EntitiesHelperClass.CloseLoading();
                    EntitiesHelperClass.DeleteTempPictures();
                    AppDataHelperClass.foods = foods;
                    await Navigation.PopAsync();
                    await OpenManualInput();
                    
                }
                else
                {
                    await ViewServices.PopUpManager.PopPersonalizedAsync("Items TBCA Detectados", tbcaTeste, "OK");

                    Meal newMeal = new()
                    {
                        DateTime = DateTime.Now,
                        Name = "Refeição",
                        FoodItems = foods
                    };

                    AppDataHelperClass.AddMeal(newMeal);

                    await ViewServices.PopUpManager.PopInfoAsync("Refeição registrada com sucesso!");

                    EntitiesHelperClass.CloseLoading();

                    EntitiesHelperClass.DeleteTempPictures();


                    await Navigation.PopAsync();
                }

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

    [RelayCommand]
    private async Task OpenManualInput()
    {
        await _navigation.PushAsync(ViewServices.ResolvePage<IManualInputPage>());
    }

}