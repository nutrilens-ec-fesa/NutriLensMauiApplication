using CommunityToolkit.Mvvm.Input;
using ExceptionLibrary;
using NutriLens.Entities;
using NutriLens.Models;
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
        try
        {
            if (EntitiesHelperClass.HasTempPicture)
            {
                byte[] img = File.ReadAllBytes(Path.Combine(FileSystem.AppDataDirectory, EntitiesHelperClass.GetTempPicture()));

                string filePath = NewPictureFilePath();

                File.WriteAllBytes(filePath, img);

                EntitiesHelperClass.ShowLoading("Sincronizando imagem na nuvem...");

                string mongoImageId = string.Empty;
                
                // Chama a função de upload com o caminho da imagem
                await Task.Run(() => mongoImageId = DaoHelperClass.UploadImage(filePath));

                //await ViewServices.PopUpManager.PopInfoAsync($"Imagem '{Path.GetFileName(filePath)}' salva com sucesso!");

                EntitiesHelperClass.CloseLoading();

                EntitiesHelperClass.ShowLoading("Realizando análise dos alimentos...");

                string resultadoAnalise = string.Empty;
                string identificados = string.Empty;
                string tbcaTeste = string.Empty;
                string tacoTeste = string.Empty;

                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

                List<RecognizedImageInfoTxtModel> alimentosTxt = new List<RecognizedImageInfoTxtModel>();
                List<RecognizedImageInfoModel> alimentosJson = new List<RecognizedImageInfoModel>();
                List<FoodItem> foods = new List<FoodItem>();

                // Chama a função de upload com o caminho da imagem
                await Task.Run(() =>
                {
                    resultadoAnalise = DaoHelperClass.GetFoodVisionAnalisysByImageId(mongoImageId);
                    // Quando a análise estiver completa, sinalize o TaskCompletionSource
                    tcs.SetResult(true);
                });

                await tcs.Task;

                if (tcs.Task.IsCompleted)
                {
                    try
                    {
                        //Verifica se o texto de retorno do GPT veio em JSON ou não
                        if (resultadoAnalise.Contains("json"))
                        {
                            alimentosJson = AppDataHelperClass.GetRecognizedImageInfoModel(resultadoAnalise);
                            identificados = AppDataHelperClass.GetRecognizedImageInfoText(alimentosJson);
                            tacoTeste = AppDataHelperClass.GetStringTacoItemsByImageInfo(alimentosJson, out foods);
                        }
                        else
                        {
                            alimentosTxt = AppDataHelperClass.GetRecognizedImageInfoTxtModel(resultadoAnalise);
                            identificados = AppDataHelperClass.GetRecognizedImageInfoText(alimentosTxt);
                            tacoTeste = AppDataHelperClass.GetStringTacoItemsByImageInfo(alimentosTxt);
                            foods = AppDataHelperClass.GetFoodItems(alimentosTxt);
                        }
                    }
                    catch(Exception ex)
                    {
                        await EntitiesHelperClass.CloseLoading();
                        await ViewServices.PopUpManager.PopErrorAsync($"Houve algum problema com a análise da foto dos alimentos. {Environment.NewLine} Retorno da API: {resultadoAnalise}. {Environment.NewLine}" + ExceptionManager.ExceptionMessage(ex));
                        return;
                    }
                }

                string tacoIdentified = string.Empty;

                foreach(FoodItem foodItem in foods)
                {
                    tacoIdentified += $"{foodItem.NamePlusPortion}{Environment.NewLine}";
                }
                int telaEdicao = await ViewServices.PopUpManager.PopPersonalizedAsync("Verifique os alimentos identificados", tacoIdentified, "OK", "Editar");

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
                    await ViewServices.PopUpManager.PopPersonalizedAsync("Items TACO Detectados", tacoTeste, "OK");

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
            await EntitiesHelperClass.CloseLoading();
            await ViewServices.PopUpManager.PopErrorAsync("Houve algum erro para salvar a foto.\n\n" + ExceptionManager.ExceptionMessage(ex));
        }
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