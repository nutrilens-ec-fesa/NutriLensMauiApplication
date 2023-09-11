namespace NutriLens.Entities
{
    public static class UriAndPaths
    {
        private static readonly string _appConfigFile = "appconfig.json";
        public static readonly string appConfigurationPath = Path.Combine(FileSystem.AppDataDirectory, _appConfigFile);
    }
}
