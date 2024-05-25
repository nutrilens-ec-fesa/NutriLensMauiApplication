using AppConfigLibrary;
using AppDataLibrary;
using Camera.MAUI;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using NutriLens.Entities;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using NutriLens.Views;
using PermissionsLibrary;
using Plugin.Maui.Audio;
using PopupLibrary;
using Syncfusion.Maui.Core.Hosting;

namespace NutriLens
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMediaElement()
                .UseMauiCameraView()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.UseViewServices();
            builder.Services.AddSingleton(AudioManager.Current);

            builder.Services.AddSingleton<IPopUpManager, PopUpManager>();

            builder.Services.AddSingleton<IAppConfigurationManager>(provider =>
            {
                return new AppConfigurationManager(UriAndPaths.appConfigurationPath);
            });

            builder.Services.AddSingleton<IAppDataManager>(provider =>
            {
                return new AppDataManager(UriAndPaths.appDataPath);
            });

#if ANDROID || IOS
            builder.Services.AddSingleton<IPermissionManager, PermissionManager>();
            builder.Services.AddSingleton<IMainMenuPage, MobileMainMenu>();
            builder.Services.AddSingleton<IFlyoutPage, MobileFlyoutPage>();
            builder.Services.AddSingleton<ICameraPage, MobileCameraPage>();
            builder.Services.AddSingleton<IBarCodePage, MobileBarCodePage>();
            builder.Services.AddSingleton<IManualInputPage, MobileManualInputPage>();
            builder.Services.AddSingleton<IMealHistoricPage, MobileMealHistoricPage>();
            builder.Services.AddSingleton<IPicturesGridPage, MobilePicturesGridPage>();
            builder.Services.AddSingleton<IAddBarcodeProductPage, MobileAddBarcodeProduct>();
            builder.Services.AddSingleton<IEditBarCodeProductsPage, MobileEditBarCodeProducts>();
            builder.Services.AddSingleton<IUserConfigPage, MobileUserConfigPage>();
            builder.Services.AddSingleton<ILoginPage, MobileLoginPage>();
            builder.Services.AddSingleton<IRegistrationPage, MobileRegistrationPage>();
            builder.Services.AddSingleton<IAiModelPromptPage, MobileAiModelPromptView>();
            builder.Services.AddSingleton<IGroupedMealHistoricPage, MobileGroupedMealHistoricPage>();
            builder.Services.AddSingleton<IAddCustomFoodItemPage, MobileAddCustomFoodItemPage>();
#elif WINDOWS

#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}