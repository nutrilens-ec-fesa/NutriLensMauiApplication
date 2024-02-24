﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpPost, Route("v1/UploadImage")]
        public IActionResult UploadImage([FromBody] MongoImage mongoImage,
            [FromServices] IMongoImage mongoImageRepo)
        {
            try
            {
                mongoImage.UserIdentifier = EntitiesHelperClass.AuthenticatedUserIdentifier(this);
                mongoImageRepo.InsertNew(mongoImage);
                return Created(string.Empty, null);
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