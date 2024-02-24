using NutriLensClassLibrary.Models;
using NutriLensWebApp.Interfaces;
using ExceptionLibrary;
using NutriLensWebApp.Entities;
using MongoDB.Driver;

namespace NutriLensWebApp.Repositories
{
    public class MongoImageRepository : IMongoImage
    {
        public void DeleteById(string id)
        {
            MongoImage mongoImage = GetById(id);

            try
            {
                AppMongoDbContext.MongoImage
                    .DeleteOne(Builders<MongoImage>.Filter.Eq(x => x.Id, id));
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para deletar a foto informada");
            }
        }

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

        public MongoImage GetById(string id)
        {
            MongoImage mongoImage;

            try
            {
                mongoImage = AppMongoDbContext.MongoImage
                    .Find(Builders<MongoImage>.Filter.Eq(x => x.Id, id))
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("houve algum problema na busca pelas imagens", ex);
            }

            if (mongoImage == null)
                throw new NotFoundException("Não foi encontrada imagem a partir do id informado");
            else
                return mongoImage;
        }

        public List<MongoImage> GetImagesByUserIdentifier(string userId)
        {
            try
            {
                return AppMongoDbContext.MongoImage
                    .Find(Builders<MongoImage>.Filter.Eq(x => x.UserIdentifier, userId))
                    .ToList();
            }
            catch (Exception ex)
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
