using NutriLensClassLibrary.Models;
using NutriLensWebApp.Interfaces;
using ExceptionLibrary;
using NutriLensWebApp.Entities;
using MongoDB.Driver;

namespace NutriLensWebApp.Repositories
{
    public class UserInfoRepository : IUserInfo
    {
        public UserInfo GetUserInfoById(string id)
        {
            try
            {
                var filterBuilder = Builders<UserInfo>.Filter;

                return AppMongoDbContext.UserInfo
                    .Find(filterBuilder.Eq(x => x.Id, id))
                    .FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw new DatabaseQueryException("Houve algum problema para resgatar as informações do usuário", ex);
            }
        }
    }
}
