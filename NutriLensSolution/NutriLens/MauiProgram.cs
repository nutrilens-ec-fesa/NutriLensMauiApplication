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

            //builder.Services.AddSingleton<IFtpManager>(provider =>
            //{
            //    return new FtpManager(string.Empty, string.Empty, string.Empty);
            //});

            builder.Services.AddSingleton<IPopUpManager, PopUpManager>();

            builder.Services.AddSingleton<IAppConfigurationManager>(provider =>
            {
                return new AppConfigurationManager(UriAndPaths.appConfigurationPath);
            });

#if ANDROID || IOS
            builder.Services.AddSingleton<IPermissionManager, PermissionManager>();
            builder.Services.AddSingleton<IMainMenuPage, MobileMainMenu>();
            builder.Services.AddSingleton<IFlyoutPage, MobileFlyoutPage>();
            builder.Services.AddSingleton<ICameraPage, MobileCameraPage>();
#elif WINDOWS

#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}