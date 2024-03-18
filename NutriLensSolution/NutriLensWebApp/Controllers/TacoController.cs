using ExceptionLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriLensClassLibrary.Models;
using NutriLensWebApp.Interfaces;

namespace NutriLensWebApp.Controllers
{
#if DEBUG
    // [AllowAnonymous]
    [Authorize]
#else
    [Authorize]
#endif
    [Route("[controller]")]
    public class TacoController : ControllerBase
    {
        [HttpGet, Route("v1/GetList")]
        public IActionResult GetList([FromServices] ITacoItemRepository tacoItemRepo)
        {
            try
            {
                List<TacoItem> tacoItemsList = tacoItemRepo.GetList();

                tacoItemsList = tacoItemsList
                    .OrderBy(x => x.Nome)
                    .ToList();

                return Ok(tacoItemsList);
            }
            catch(Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }
    }
}
