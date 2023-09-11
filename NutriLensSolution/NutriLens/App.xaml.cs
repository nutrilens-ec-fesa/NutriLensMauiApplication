using NutriLens.Models;
using NutriLens.Services;
using NutriLens.ViewInterfaces;
using PermissionsLibrary;
using PopupLibrary;

namespace NutriLens
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(ViewServices.ResolvePage<IMainMenuPage>());
        }

        private static MauiAppInfo GetAppInfo()
        {
            return new MauiAppInfo()
            {
                AppName = AppInfo.Current.Name,
                AppVersion = AppInfo.Current.Version.ToString(),
                AppTheme = AppInfo.Current.RequestedTheme.ToString(),
                DevIdiom = DeviceInfo.Current.Idiom.ToString(),
                DevModel = DeviceInfo.Current.Model,
                DevName = DeviceInfo.Current.Name,
                DevManufacturer = DeviceInfo.Current.Manufacturer,
                DevOsVersion = DeviceInfo.Current.VersionString,
                DevPlatform = DeviceInfo.Current.Platform.ToString(),
                DevType = DeviceInfo.Current.DeviceType.ToString()
            };
        }
    }
}