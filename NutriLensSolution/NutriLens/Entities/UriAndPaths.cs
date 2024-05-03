namespace NutriLens.Entities
{
    public static class UriAndPaths
    {
        private static readonly string _appConfigFile = "appconfig.json";
        private static readonly string _appDataFile = "appdata.json";
        private static readonly string _databasePicturesDirectory = "database";

        public static readonly string appConfigurationPath = Path.Combine(FileSystem.AppDataDirectory, _appConfigFile);
        public static readonly string appDataPath = Path.Combine(FileSystem.AppDataDirectory, _appDataFile);
        public static readonly string databasePicturesPath = Path.Combine(FileSystem.AppDataDirectory, _databasePicturesDirectory);
        //public static readonly string termsOfUsePath = "https://cefsaedu-my.sharepoint.com/:b:/g/personal/082190011_faculdade_cefsa_edu_br/ET0nI8ylg71CrDFMLteqiXIB49m2RQA0EWYc9XEdNZLZCQ?e=QH25f1";
        public static readonly string termsOfUsePath = "https://drive.google.com/file/d/1vu0vIcb7wfKiv1OmlYXlOb2jdqZSXXEO/view?usp=sharing";
        public static string ApiUrl = "https://nutrilenswebapp.azurewebsites.net";
    }
}
