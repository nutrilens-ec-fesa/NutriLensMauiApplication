using NutriLensClassLibrary.Models;

namespace NutriLensWebApp.Interfaces
{
    public interface IMongoImage
    {
        public List<MongoImage> GetAllImagesList();
        public void InsertNew(MongoImage image);
    }
}
