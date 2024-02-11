using NutriLensClassLibrary.Models;

namespace NutriLensWebApp.Interfaces
{
    public interface ILogin
    {
        public Login GetLogin(string email, string hashPassword);
        public void InsertNewUser(Login login);
    }
}
