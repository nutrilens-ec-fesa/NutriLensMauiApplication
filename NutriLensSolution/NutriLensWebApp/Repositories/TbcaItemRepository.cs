﻿using ExceptionLibrary;
using MongoDB.Driver;
using NutriLensClassLibrary.Models;
using NutriLensWebApp.Entities;
using NutriLensWebApp.Interfaces;

namespace NutriLensWebApp.Repositories
{
    public class TbcaItemRepository : ITbcaItemRepository
    {
        public List<TbcaItem> GetList()
        {
            try
            {
                return AppMongoDbContext.TbcaItem
                    .Find(Builders<TbcaItem>.Filter.Empty)
                    .ToList();
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema na obtenção da lista de produtos TBCA", ex);
            }
        }
    }
}
