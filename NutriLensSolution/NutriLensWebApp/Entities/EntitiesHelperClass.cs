using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NutriLensWebApp.Entities
{
    public static class EntitiesHelperClass
    {
        public static string AuthenticatedUserIdentifier(ControllerBase controllerBase)
        {
            try
            {
                return controllerBase.User
                    .FindFirstValue(ClaimTypes.Sid);
            }
            catch (Exception ex)
            {
                throw new Exception("Houve algum problema para obter o identificador do usuário logado", ex);
            }
        }
    }
}
