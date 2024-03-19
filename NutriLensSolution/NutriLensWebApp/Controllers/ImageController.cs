using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriLensWebApp.Entities;
using ExceptionLibrary;
using NutriLensWebApp.Interfaces;
using NutriLensClassLibrary.Models;

namespace NutriLensWebApp.Controllers
{
#if DEBUG
    [AllowAnonymous]
    //[Authorize]
#else
    [Authorize]
#endif
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        [HttpGet, Route("v1/GetAllImages")]
        public IActionResult GetAllImages([FromServices] IMongoImage mongoImageRepo)
        {
            try
            {
                return Ok(mongoImageRepo.GetAllImagesList());
            }
            catch(Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpGet, Route("v1/GetAllImagesByAuthUser")]
        public IActionResult GetAllImagesByAuthUser([FromServices] IMongoImage mongoImageRepo)
        {
            try
            {
                return Ok(mongoImageRepo.GetImagesByUserIdentifier(EntitiesHelperClass.AuthenticatedUserIdentifier(this)));
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPost, Route("v1/UploadImage")]
        public IActionResult UploadImage([FromBody] MongoImage mongoImage,
            [FromServices] IMongoImage mongoImageRepo)
        {
            try
            {
                mongoImage.DateTime = DateTime.Now;
                mongoImage.UserIdentifier = EntitiesHelperClass.AuthenticatedUserIdentifier(this);
                mongoImageRepo.InsertNew(mongoImage);
                return Created(mongoImage.Id, mongoImage.Id);
            }
            catch(Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpDelete, Route("v1/DeleteImageById/{imageId}")]
        public IActionResult DeleteImage(string imageId,
            [FromServices] IMongoImage mongoImageRepo)
        {
            try
            {
                mongoImageRepo.DeleteById(imageId);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }
    }
}
