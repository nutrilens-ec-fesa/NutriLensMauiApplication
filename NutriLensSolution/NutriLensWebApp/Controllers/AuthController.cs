using Microsoft.AspNetCore.Mvc;
using NutriLensClassLibrary.Models;
using ExceptionLibrary;
using NutriLensWebApp.Interfaces;
using NutriLensWebApp.Services;
using WebLibrary;

namespace NutriLensWebApp.Controllers
{
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost, Route("v1/Login")]
        public IActionResult Login([FromBody] Login loginObject,
            [FromServices] ILogin loginRepo)
        {
            try
            {
                Login login = loginRepo.GetLogin(loginObject.Email, loginObject.Password);

                if (login == null)
                    return NotFound("Usuário não encontrado");
                else
                    return Ok(TokenService.GenerateToken(login));
            }
            catch(Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPost, Route("v1/LoginByUserInfoId")]
        public IActionResult LoginByUserInfoId([FromBody] string userInfoId,
            [FromServices] IUserInfo userInfoRepo)
        {
            try
            {
                UserInfo userInfo = userInfoRepo.GetUserInfoById(userInfoId);

                if (userInfo == null)
                    return NotFound("Usuário não encontrado");
                else
                    return Ok(TokenService.GenerateToken(userInfo));
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }
    }
}
