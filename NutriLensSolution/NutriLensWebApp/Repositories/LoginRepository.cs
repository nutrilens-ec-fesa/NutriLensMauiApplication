using ExceptionLibrary;
using MongoDB.Driver;
using NutriLensClassLibrary.Models;
using NutriLensWebApp.Entities;
using NutriLensWebApp.Interfaces;

namespace NutriLensWebApp.Repositories
{
    public class LoginRepository : ILogin
    {
        public Login GetLogin(string email, string hashPassword)
        {
            try
            {
                FilterDefinitionBuilder<Login> filterBuilder = Builders<Login>.Filter;

                return AppMongoDbContext.Login
                    .Find(filterBuilder.Eq(x => x.Email, email) & filterBuilder.Eq(x => x.Password, hashPassword))
                    .FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para obter o login do usuário", ex);
            }
        }

        public void InsertNewUser(Login login)
        {
            try
            {
                FilterDefinitionBuilder<Login> filterBuilder = Builders<Login>.Filter;

                if (AppMongoDbContext.Login.Find(filterBuilder.Eq(x => x.Email, login.Email)).FirstOrDefault() != null)
                    throw new AlreadyRegisteredException("Já existe um usuário com o e-mail informado.");
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para verifica login já existente", ex);
            }

            try
            {
                AppMongoDbContext.Login.InsertOne(login);
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para inserir o novo registro de login", ex);
            }

            string userInfoIdentifier = login.UserInfoIdentifier;

            try
            {
                FilterDefinitionBuilder<UserInfo> filterBuilder = Builders<UserInfo>.Filter;

                if (AppMongoDbContext.UserInfo.Find(filterBuilder.Eq(x => x.Id, userInfoIdentifier)).FirstOrDefault() != null)
                    throw new AlreadyRegisteredException("Já existem dados vinculados ao usuário informado!");
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para verificar dados já existentes do usuário", ex);
            }

            try
            {
                AnvisaLimits anvisaLimits = AnvisaLimits.GetAnvisaLimits();

                UserInfo newUserInfo = new() 
                { 
                    Id = userInfoIdentifier, 
                    KiloCaloriesDiaryObjective = 2000,
                    DailyCarbohydrateGoal = anvisaLimits.Carboidratos,
                    DailyProteinGoal = anvisaLimits.Proteinas,
                    DailyFatGoal = anvisaLimits.GordurasTotais,
                    DailyFiberGoal = anvisaLimits.FibraAlimentar,
                    DailySodiumGoal = anvisaLimits.Sodio,
                    DailyCholesterolGoal = anvisaLimits.Colesterol,
                    DailyCalciumGoal = anvisaLimits.Calcio,
                    DailyIronGoal = anvisaLimits.Ferro
                };

                AppMongoDbContext.UserInfo.InsertOne(newUserInfo);
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para inserir os novos dados de usuário", ex);
            }
        }
    }
}
