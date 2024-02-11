using MongoDB.Driver;
using NutriLensClassLibrary.Models;
using NutriLensWebApp.Entities;
using NutriLensWebApp.Interfaces;
using ExceptionLibrary;

namespace NutriLensWebApp.Repositories
{
    public class BarcodeItemRepository : IBarcodeItem
    {
        public BarcodeItem GetBarcodeItem(string barcode)
        {
            try
            {
                return AppMongoDbContext.BarcodeItem
                    .Find(Builders<BarcodeItem>.Filter.Eq(x => x.Barcode, barcode))
                    .FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para encontrar o produto de código de barras", ex);
            }
        }

        public List<BarcodeItem> GetBarcodeItemsList()
        {
            try
            {
                return AppMongoDbContext.BarcodeItem
                    .Find(Builders<BarcodeItem>.Filter.Empty)
                    .ToList();
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("houve algum problema para listar os produtos de código de barras", ex);
            }
        }

        public void InsertBarcodeItem(BarcodeItem barcodeItem)
        {
            BarcodeItem existingBarcodeItem;

            try
            {
                FilterDefinition<BarcodeItem> filter = Builders<BarcodeItem>.Filter.Eq(x => x.Barcode, barcodeItem.Barcode);
                existingBarcodeItem = AppMongoDbContext.BarcodeItem.Find(filter).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para verifica código de barras já existente", ex);
            }

            if (existingBarcodeItem != null)
                throw new AlreadyRegisteredException("Já existe um produto registrado com o código de barras informado");

            try
            {
                AppMongoDbContext.BarcodeItem.InsertOne(barcodeItem);
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para inserir o novo produto de código de barras", ex);
            }
        }

        public void UpdateBarcodeItem(BarcodeItem barcodeItem)
        {
            try
            {
                AppMongoDbContext.BarcodeItem.ReplaceOne(doc => doc.Barcode == barcodeItem.Barcode, barcodeItem);
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para atualizar o produto de código de barras", ex);
            }
        }
    }
}
