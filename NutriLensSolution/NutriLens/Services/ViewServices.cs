using AppConfigLibrary;
using AppDataLibrary;
using DictionaryLibrary;
using PermissionsLibrary;
using Plugin.Maui.Audio;
using PopupLibrary;
using WebLibrary;

namespace NutriLens.Services;

public static class ViewServices
{
    public static IServiceCollection Services { get; private set; }

    public static MauiAppBuilder UseViewServices(this MauiAppBuilder builder)
    {
        Services = builder.Services;
        return builder;
    }

    /// <summary>
    /// Função genérica que cria uma ContentPage
    /// </summary>
    /// <typeparam name="TService">Tipo de Página</typeparam>
    /// <param name="args">Parâmetros do Content Page desejado</param>
    /// <returns>Retorna uma ContentPage</returns>
    public static ContentPage ResolvePage<TService>(params object[] args)
    {
        Type implementationType = Services
            .Where(x => x.ServiceType == typeof(TService))
            .Select(x => x.ImplementationType)
            .FirstOrDefault();

        if (implementationType != null)
            return (Activator.CreateInstance(implementationType, args)) as ContentPage;
        else
            return null;
    }

    /// <summary>
    /// Função genérica que cria uma ContentPage
    /// </summary>
    /// <typeparam name="TService">Tipo de Página</typeparam>
    /// <param name="args">Parâmetros do Content Page desejado</param>
    /// <returns>Retorna uma ContentPage</returns>
    public static FlyoutPage ResolveFlyout<TService>(params object[] args)
    {
        Type implementationType = Services
            .Where(x => x.ServiceType == typeof(TService))
            .Select(x => x.ImplementationType)
            .FirstOrDefault();

        if (implementationType != null)
            return (Activator.CreateInstance(implementationType, args)) as FlyoutPage;
        else
            return null;
    }

    public static T GetService<T>()
    {

#if WINDOWS10_0_17763_0_OR_GREATER
                return MauiWinUIApplication.Current.Services.GetService<T>();
#elif ANDROID
        return MauiApplication.Current.Services.GetService<T>();
#elif IOS || MACCATALYST
                return MauiUIApplicationDelegate.Current.Services.GetService<T>();
#else
                return null;
#endif
    }

    public static INavigation Navigation => GetService<INavigation>();

    public static IDictionaryManager DictionaryManager => GetService<IDictionaryManager>();

    public static IFtpManager FtpManager => GetService<IFtpManager>();

    public static IPopUpManager PopUpManager => GetService<IPopUpManager>();

    public static IPermissionManager PermissionManager => GetService<IPermissionManager>();

    public static IAppDataManager AppDataManager => GetService<IAppDataManager>();

    public static IAppConfigurationManager AppConfigurationManager => GetService<IAppConfigurationManager>();

    public static IAudioManager AudioManager => GetService<IAudioManager>();
}



