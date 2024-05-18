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

        [HttpPut, Route("v1/UpdateTacoCategories")]
        public IActionResult UpdateTacoCategories([FromServices] ITacoItemRepository tacoItemRepository)
        {
            try
            {
                List<TacoItem> tacoItems = tacoItemRepository.GetList();

                foreach (TacoItem tacoItem in tacoItems)
                {
                    if (InRange(tacoItem.id, 1, 63))
                        tacoItem.Category = (int)TacoCategory.CEREAIS_E_DERIVADOS;
                    else if (InRange(tacoItem.id, 64, 162))
                        tacoItem.Category = (int)TacoCategory.VERDURAS_HORTALICAS_E_DERIVADOS;
                    else if (InRange(tacoItem.id, 163, 258))
                        tacoItem.Category = (int)TacoCategory.FRUTAS_E_DERIVADOS;
                    else if (InRange(tacoItem.id, 259, 272))
                        tacoItem.Category = (int)TacoCategory.GORDURAS_E_OLEOS;
                    else if (InRange(tacoItem.id, 273, 445))
                        tacoItem.Category = (int)TacoCategory.PESCADOS_E_FRUTOS_DO_MAR;
                    else if (InRange(tacoItem.id, 446, 469))
                        tacoItem.Category = (int)TacoCategory.LEITE_E_DERIVADOS;
                    else if (InRange(tacoItem.id, 470, 483))
                        tacoItem.Category = (int)TacoCategory.BEBIDAS;
                    else if (InRange(tacoItem.id, 484, 490))
                        tacoItem.Category = (int)TacoCategory.OVOS_E_DERIVADOS;
                    else if (InRange(tacoItem.id, 491, 510))
                        tacoItem.Category = (int)TacoCategory.PRODUTOS_ACUCARADOS;
                    else if (InRange(tacoItem.id, 511, 519))
                        tacoItem.Category = (int)TacoCategory.MISCELANEAS;
                    else if (InRange(tacoItem.id, 520, 524))
                        tacoItem.Category = (int)TacoCategory.OUTROS_ALIMENTOS_INDUSTRIALIZADOS;
                    else if (InRange(tacoItem.id, 525, 556))
                        tacoItem.Category = (int)TacoCategory.ALIMENTOS_PREPARADOS;
                    else if (InRange(tacoItem.id, 557, 586))
                        tacoItem.Category = (int)TacoCategory.LEITE_E_DERIVADOS;
                    else if (InRange(tacoItem.id, 587, 597))
                        tacoItem.Category = (int)TacoCategory.VERDURAS_HORTALICAS_E_DERIVADOS;
                    else
                        tacoItem.Category = (int)TacoCategory.INDEFINIDO;
                }

                tacoItemRepository.UpdateTacoItems(tacoItems);

                return Ok("Elementos originais da tabela TACO atualizados com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPut, Route("v1/UpdateGlutenItems")]
        public IActionResult UpdateGlutenItems([FromServices] ITacoItemRepository tacoItemRepository)
        {
            try
            {
                List<TacoItem> tacoItems = tacoItemRepository.GetList();

                List<int> glutenIds = new List<int>()
                {
                    3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18,
                    25, 26, 31, 32, 33, 34, 35, 37, 38, 39, 40, 41,
                    48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60,
                    121, 122, 140, 141, 387, 388, 389, 390,
                    440, 441, 442, 542
                };

                foreach (TacoItem tacoItem in tacoItems)
                {
                    tacoItem.Gluten = glutenIds.Contains(tacoItem.id);
                }

                tacoItemRepository.UpdateTacoItems(tacoItems);

                return Ok("Elementos com glúten da tabela TACO atualizados com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPut, Route("v1/UpdateLactoseItems")]
        public IActionResult UpdateLactoseItems([FromServices] ITacoItemRepository tacoItemRepository)
        {
            try
            {
                List<TacoItem> tacoItems = tacoItemRepository.GetList();

                List<int> lactoseIds = new List<int>()
                {
                    9, 10, 11, 12, 14, 15, 16, 17, 18, 20, 140, 141, 447, 448, 449, 450, 451, 452, 453, 454,
                    455, 456, 457, 458, 459, 460, 461, 462, 463, 464, 465, 466, 467, 468, 469, 491, 495, 496,
                    497, 498, 501, 504, 505, 506, 509, 512
                };

                foreach (TacoItem tacoItem in tacoItems)
                {
                    tacoItem.Lactose = lactoseIds.Contains(tacoItem.id);
                }

                tacoItemRepository.UpdateTacoItems(tacoItems);

                return Ok("Elementos com lactose da tabela TACO atualizados com sucesso!");
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

        [NonAction]
        public bool InRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}
