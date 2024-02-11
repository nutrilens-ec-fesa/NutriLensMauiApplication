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
    public class TbcaController : ControllerBase
    {
        [HttpGet, Route("v1/GetList")]
        public IActionResult GetList([FromServices] ITbcaItemRepository tbcaItemRepo)
        {
            try
            {
                List<TbcaItem> tbcaItemsList = tbcaItemRepo.GetList();

                tbcaItemsList = tbcaItemsList
                    .OrderBy(x => x.Alimento)
                    .ToList();

                return Ok(tbcaItemsList);
            }
            catch(Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }
    }
}
