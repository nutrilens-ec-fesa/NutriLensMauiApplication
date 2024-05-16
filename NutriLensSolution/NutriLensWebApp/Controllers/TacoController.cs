using ExceptionLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPut, Route("v1/UpdateLiquids")]
        public IActionResult UpdateLiquids([FromServices] ITacoItemRepository tacoItemRepository)
        {
            try
            {
                List<TacoItem> tacoItems = tacoItemRepository.GetList();

                List<int> liquidsIds = new List<int>()
                {
                    188, 209, 211, 213, 215, 217, 218, 219, 234, 252, 258, 259, 260,
                    267, 268, 269, 270, 271, 272, 446, 454, 455, 457, 458, 460, 470,
                    471, 472, 473, 474, 475, 476, 477, 478, 479, 480, 481, 482, 483,
                    518, 523
                };

                foreach (TacoItem tacoItem in tacoItems)
                {
                    tacoItem.Liquid = liquidsIds.Contains(tacoItem.id);
                }

                tacoItemRepository.UpdateTacoItems(tacoItems);

                return Ok("Elementos líquidos da tabela TACO atualizados com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPut, Route("v1/UpdateTacoOriginals")]
        public IActionResult UpdateTacoOriginals([FromServices] ITacoItemRepository tacoItemRepository)
        {
            try
            {
                List<TacoItem> tacoItems = tacoItemRepository.GetList();

                foreach (TacoItem tacoItem in tacoItems)
                {
                    tacoItem.TacoOriginal = tacoItem.id <= 597;
                }

                tacoItemRepository.UpdateTacoItems(tacoItems);

                return Ok("Elementos originais da tabela TACO atualizados com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpGet, Route("v1/GetTacoHash")]
        public IActionResult GetTacoHash([FromServices] ITacoItemRepository tacoItemRepository)
        {
            try
            {
                string tacoJson = JsonConvert.SerializeObject(tacoItemRepository.GetList());
                string tacoHash = CryptographyLibrary.CryptographyManager.Crypt(tacoJson, CryptographyLibrary.EnumCryptAlgorithm.caSHA256);
                return Ok(tacoHash);
            }
            catch(Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }
    }
}
