namespace NutriLens.Models
{
    public class MauiAppInfo
    {
        #region PROPERTIES FROM APP

        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public string AppTheme { get; set; }

        #endregion

        #region PROPERTIES FROM DEVICE

        public string DevModel { get; set; }
        public string DevManufacturer { get; set; }
        public string DevName { get; set; }
        public string DevOsVersion { get; set; }
        public string DevIdiom { get; set; }
        public string DevPlatform { get; set; }
        public string DevType { get; set; }

        #endregion

        public MauiAppInfo()
        {
            AppName = string.Empty;
            AppVersion = string.Empty;
            AppTheme = string.Empty;
            DevModel = string.Empty;
            DevManufacturer = string.Empty;
            DevName = string.Empty;
            DevOsVersion = string.Empty;
            DevIdiom = string.Empty;
            DevPlatform = string.Empty;
            DevType = string.Empty;
        }
    }
}