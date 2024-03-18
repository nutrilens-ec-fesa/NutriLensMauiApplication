using ExceptionLibrary;
using MongoDB.Driver;
using NutriLensClassLibrary.Models;
using NutriLensWebApp.Entities;
using NutriLensWebApp.Interfaces;

namespace NutriLensWebApp.Repositories
{
    public class TacoItemRepository : ITacoItemRepository
    {
        public List<TacoItem> GetList()
        {
            try
            {
                return AppMongoDbContext.TacoItem
                    .Find(Builders<TacoItem>.Filter.Empty)
                    .ToList();
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema na obtenção da lista de produtos TBCA", ex);
            }
        }
    }
}
