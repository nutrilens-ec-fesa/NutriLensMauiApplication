using NutriLensClassLibrary.Models;

namespace NutriLensWebApp.Interfaces
{
    public interface IUserInfo
    {
        public UserInfo GetUserInfoById(string id);
        public void UpdateUserInfo(UserInfo userInfo);
    }
}
