using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExceptionLibrary;
using NutriLensWebApp.Interfaces;
using NutriLensClassLibrary.Models;
using NutriLensWebApp.Entities;

namespace NutriLensWebApp.Controllers
{
#if DEBUG
    // [AllowAnonymous]
    [Authorize]
#else
    [Authorize]
#endif
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost, Route("v1/InsertNewUser"), AllowAnonymous]
        public IActionResult InsertNewUser([FromBody] Login login,
            [FromServices] ILogin loginRepo)
        {
            try
            {
                loginRepo.InsertNewUser(login);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpGet, Route("v1/GetUserInfo")]
        public IActionResult GetUserInfo([FromServices] IUserInfo userInfoRepo)
        {
            try
            {
                UserInfo userInfo = userInfoRepo.GetUserInfoById(EntitiesHelperClass.AuthenticatedUserIdentifier(this));

                if (userInfo == null)
                    return NotFound("Não foi encontrada informação do usuário autenticado");
                else
                    return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPut, Route("v1/UpdateUserInfo")]
        public IActionResult UpdateUserInfo([FromBody] UserInfo userInfo,
            [FromServices] IUserInfo userInfoRepo)
        {
            try
            {
                userInfoRepo.UpdateUserInfo(userInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }
    }
}
