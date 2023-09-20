using AppConfigLibrary;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using NutriLens.Entities;
using NutriLens.Services;
using PopupLibrary;
using PermissionsLibrary;
using WebLibrary;
using NutriLens.ViewInterfaces;
using NutriLens.Views;
using Camera.MAUI;
using Plugin.Maui.Audio;
using Microsoft.Extensions.DependencyInjection;
using AppDataLibrary;

namespace NutriLens
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCameraView()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.UseMauiCommunityToolkit();
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
#elif WINDOWS

#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}