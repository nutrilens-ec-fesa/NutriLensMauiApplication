namespace NutriLens.Entities
{
    public static class UriAndPaths
    {
        private static readonly string _appConfigFile = "appconfig.json";
        private static readonly string _appDataFile = "appdata.json";

        public static readonly string appConfigurationPath = Path.Combine(FileSystem.AppDataDirectory, _appConfigFile);
        public static readonly string appDataPath = Path.Combine(FileSystem.AppDataDirectory, _appDataFile);
    }
}
