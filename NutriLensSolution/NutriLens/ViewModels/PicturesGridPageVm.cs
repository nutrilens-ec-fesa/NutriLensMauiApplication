using CommunityToolkit.Mvvm.ComponentModel;
using NutriLens.Entities;

namespace NutriLens.ViewModels
{
    internal class PicturesGridPageVm : ObservableObject
    {
        public Grid PicturesGrid { get; set; }

        public bool DevUser { get => AppDataHelperClass.UserInfo.DevUser; }

        public PicturesGridPageVm()
        {
            string[] files = Directory.GetFiles(FileSystem.AppDataDirectory);
            string[] pictures = files.Where(x => x.EndsWith(".png")).ToArray();

            if (pictures.Length > 0)
            {
                PicturesGrid = new Grid();

                foreach (string picture in pictures)
                {
                    PicturesGrid.Children.Add(new Image { Source = picture });
                }
            }
        }
    }
}
