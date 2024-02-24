using NutriLensClassLibrary.Models;

namespace NutriLensWebApp.Interfaces
{
    public interface IMongoImage
    {
        public List<MongoImage> GetAllImagesList();
        public void InsertNew(MongoImage image);
        public List<MongoImage> GetImagesByUserIdentifier(string userId);
        public MongoImage GetById(string id);
        public void DeleteById(string id);
    }
}
