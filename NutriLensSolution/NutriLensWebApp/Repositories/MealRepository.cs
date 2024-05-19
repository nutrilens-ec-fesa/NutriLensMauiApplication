using ExceptionLibrary;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NutriLensClassLibrary.Models;
using NutriLensWebApp.Entities;
using NutriLensWebApp.Interfaces;

namespace NutriLensWebApp.Repositories
{
    public class MealRepository : IMeal
    {
        public List<Meal> GetAllMeals()
        {
            try
            {
                return AppMongoDbContext.Meal
                    .Find(Builders<Meal>.Filter.Empty)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("houve algum problema para listar as refeições", ex);
            }
        }

        public List<Meal> GetAllMealsByUserIdentifier(string userIdentifier)
        {
            try
            {
                List<Meal> userMeals = AppMongoDbContext.Meal
                    .Find(Builders<Meal>.Filter.Eq(x => x.UserInfoId, userIdentifier))
                    .ToList();

                userMeals.ForEach(x => x.DateTime = x.DateTime.ToUniversalTime().AddHours(-3));

                return userMeals;
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema na busca pelas refeições do usuário", ex);
            }
        }

        public void InsertMeals(List<Meal> meals)
        {
            try
            {
                AppMongoDbContext.Meal.InsertMany(meals);
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para inserir as refeições.", ex);
            }
        }

        public void RemoveAllMealByUserIdentifier(string userIdentifier)
        {
            try
            {
                AppMongoDbContext.Meal
                    .DeleteMany(Builders<Meal>.Filter.Eq(x => x.UserInfoId, userIdentifier));
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para deletar a refeição informada");
            }
        }

        public void RemoveMeal(string mealId)
        {
            try
            {
                AppMongoDbContext.Meal
                    .DeleteOne(Builders<Meal>.Filter.Eq(x => x.Id, mealId));
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para deletar a refeição informada");
            }
        }

        public void UpdateMeal(Meal meal)
        {
            try
            {
                AppMongoDbContext.Meal.ReplaceOne(doc => doc.Id == meal.Id, meal);
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para inserir as refeições.", ex);
            }
        }
    }
}
