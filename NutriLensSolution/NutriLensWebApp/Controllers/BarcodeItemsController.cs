using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriLensWebApp.Interfaces;
using ExceptionLibrary;
using NutriLensClassLibrary.Models;
using Microsoft.AspNetCore.Http.Extensions;

namespace NutriLensWebApp.Controllers
{
#if DEBUG
    [AllowAnonymous]
    //[Authorize]
#else
    [Authorize]
#endif
    [Route("[controller]")]
    public class BarcodeItemsController : Controller
    {
        [HttpGet, Route("v1/GetList")]
        public IActionResult GetList([FromServices] IBarcodeItem barcodeItemRepo)
        {
            try
            {
                return Ok(barcodeItemRepo.GetBarcodeItemsList());
            }
            catch(Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpGet, Route("v1/GetByBarcode/{barcode}")]
        public IActionResult GetByBarcode(string barcode,
            [FromServices] IBarcodeItem barcodeItemRepo)
        {
            try
            {
                BarcodeItem barcodeItem = barcodeItemRepo.GetBarcodeItem(barcode);

                if (barcodeItem == null)
                    return NotFound("Não foi encontrado produto a partir do código de barras informado");
                else 
                    return Ok(barcodeItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPost, Route("v1/InsertNew")]
        public IActionResult InsertNew([FromBody] BarcodeItem barcodeItem,
            [FromServices] IBarcodeItem barcodeItemRepo)
        {
            try
            {
                barcodeItemRepo.InsertBarcodeItem(barcodeItem);
                return CreatedAtAction(nameof(GetByBarcode), new { barcode = barcodeItem.Barcode }, barcodeItem);
            }
            catch(Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPut, Route("v1/Update")]
        public IActionResult Update([FromBody] BarcodeItem barcodeItem,
            [FromServices] IBarcodeItem barcodeItemRepo)
        {
            try
            {
                barcodeItemRepo.UpdateBarcodeItem(barcodeItem);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }
    }
}
