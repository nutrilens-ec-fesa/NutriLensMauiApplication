using ExceptionLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriLensClassLibrary.Models;
using NutriLensWebApp.Interfaces;

namespace NutriLensWebApp.Controllers
{
#if DEBUG
    [AllowAnonymous]
    //[Authorize]
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
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpGet, Route("v1/GetTacoDictionary")]
        public IActionResult GetTacoDictionary([FromServices] ITacoItemRepository tacoItemRepo)
        {
            List<TacoItem> tacoItems = tacoItemRepo.GetList();

            Dictionary<string, int> tacoDictionary = new Dictionary<string, int>();

            foreach (TacoItem tacoItem in tacoItems)
            {
                foreach (string word in tacoItem.Nome.Split(' '))
                {
                    if (tacoDictionary.TryGetValue(word, out _))
                        tacoDictionary[word]++;
                    else
                        tacoDictionary.Add(word, 1);
                }
            }

            return Ok(tacoDictionary.OrderByDescending(x => x.Value).ToList());
        }

        [HttpPost, Route("v1/GetTacoMatchBySimpleFooditem")]
        public IActionResult GetTacoMatchBySimpleFooditem([FromBody] SimpleFoodItem simpleFoodItem, [FromServices] ITacoItemRepository tacoItemRepo)
        {
            try
            {
                TacoFoodItemParseHelperClass.TacoFoodItems = tacoItemRepo.GetList();
                return Ok(TacoFoodItemParseHelperClass.Parse(simpleFoodItem));
            }
            catch(Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }
    }
}
