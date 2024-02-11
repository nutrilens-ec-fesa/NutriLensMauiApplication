using Microsoft.AspNetCore.Mvc;
using NutriLensWebApp.Entities;
using System.Net;

namespace NutriLensWebApp.Controllers
{
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        [HttpGet, Route("v1/CheckDatabaseStatus")]
        public IActionResult CheckDatabaseStatus()
        {
            if (AppMongoDbContext.GetDatabaseConnectionStatus(out string message))
                return Ok(message);
            else
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, message);
        }
    }
}
