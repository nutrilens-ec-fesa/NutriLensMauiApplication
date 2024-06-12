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
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema na obtenção da lista de produtos TACO", ex);
            }
        }

        public void InsertCustomTacoItem(TacoItem tacoItem)
        {
            TacoItem existingTacoItem;

            try
            {
                FilterDefinition<TacoItem> filter = Builders<TacoItem>.Filter.Eq(x => x.Nome, tacoItem.Nome);
                existingTacoItem = AppMongoDbContext.TacoItem.Find(filter).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para verificar alimento já existente", ex);
            }

            if (existingTacoItem != null)
                throw new AlreadyRegisteredException("Já existe um produto registrado com o nome informado");

            TacoItem lastInsertedTacoItem;

            try
            {
                FilterDefinition<TacoItem> filter = Builders<TacoItem>.Filter.Empty;
                existingTacoItem = AppMongoDbContext.TacoItem.Find(filter).SortByDescending(x => x.id).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para verificar último ID inserido.", ex);
            }

            try
            {
                tacoItem.id = existingTacoItem.id + 1;
                AppMongoDbContext.TacoItem.InsertOne(tacoItem);
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para inserir o novo alimento", ex);
            }
        }

        public void UpdateTacoItem(TacoItem tacoItem)
        {
            try
            {
                AppMongoDbContext.TacoItem.ReplaceOne(doc => doc.id == tacoItem.id, tacoItem);
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para atualizar o alimento", ex);
            }
        }

        public void UpdateTacoItems(List<TacoItem> tacoItems)
        {
            try
            {
                foreach (TacoItem tacoItem in tacoItems)
                {
                    AppMongoDbContext.TacoItem.ReplaceOne(doc => doc.id == tacoItem.id, tacoItem);
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para atualizar o produto de código de barras", ex);
            }
        }
    }
}
