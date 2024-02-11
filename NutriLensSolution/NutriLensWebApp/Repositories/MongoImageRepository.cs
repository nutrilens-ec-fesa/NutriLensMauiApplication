using NutriLensClassLibrary.Models;
using NutriLensWebApp.Interfaces;
using ExceptionLibrary;
using NutriLensWebApp.Entities;
using MongoDB.Driver;

namespace NutriLensWebApp.Repositories
{
    public class MongoImageRepository : IMongoImage
    {
        public List<MongoImage> GetAllImagesList()
        {
            try
            {
                return AppMongoDbContext.MongoImage
                    .Find(Builders<MongoImage>.Filter.Empty)
                    .ToList();
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("houve algum problema na busca pelas imagens", ex);
            }
        }

        public void InsertNew(MongoImage image)
        {
            try
            {
                AppMongoDbContext.MongoImage.InsertOne(image);
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("houve algum problema para inserir a nova imagem", ex);
            }
        }
    }
}
