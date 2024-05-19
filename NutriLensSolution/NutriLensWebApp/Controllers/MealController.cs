using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriLensWebApp.Interfaces;
using ExceptionLibrary;
using NutriLensClassLibrary.Models;
using NutriLensWebApp.Entities;
using NutriLensWebApp.Repositories;
using Newtonsoft.Json;
using CryptographyLibrary;

namespace NutriLensWebApp.Controllers
{
#if DEBUG
    [AllowAnonymous]
#else
    [Authorize]
#endif
    [Route("[controller]")]
    public class MealController : ControllerBase
    {
        [HttpGet, Route("v1/GetUserMealsHash")]
        public IActionResult GetUserMealsHash([FromServices] IMeal mealRepository)
        {
            try
            {
                List<Meal> userMeals = mealRepository.GetAllMealsByUserIdentifier(EntitiesHelperClass.AuthenticatedUserIdentifier(this));

                if (!userMeals.Any())
                    return Ok(new HashItem() { Hash = string.Empty });

                string userMealsJson = JsonConvert.SerializeObject(userMeals);
                string userMealsHash = CryptographyManager.Crypt(userMealsJson, EnumCryptAlgorithm.caSHA256);

                HashItem hashItem = new HashItem()
                {
                    Hash = userMealsHash,
                    ItemsCount = userMeals.Count
                };

                return Ok(hashItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpGet, Route("v1/GetAllUserMeals")]
        public IActionResult GetAllUserMeals([FromServices] IMeal mealRepository)
        {
            try
            {
                return Ok(mealRepository.GetAllMealsByUserIdentifier(EntitiesHelperClass.AuthenticatedUserIdentifier(this)));
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPost, Route("v1/InsertMeals")]
        public IActionResult InsertMeals([FromBody] List<Meal> meals, [FromServices] IMeal mealRepository)
        {
            try
            {
                mealRepository.InsertMeals(meals);
                return Ok(meals.Select(x => x.Id).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPut, Route("v1/UpdateMeal")]
        public IActionResult UpdateMeal([FromBody] Meal meal, [FromServices] IMeal mealRepository)
        {
            try
            {
                mealRepository.UpdateMeal(meal);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpDelete, Route("v1/RemoveMeal/{mealId}")]
        public IActionResult RemoveMeal(string mealId, [FromServices] IMeal mealRepository)
        {
            try
            {
                mealRepository.RemoveMeal(mealId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpDelete, Route("v1/RemoveAllAuthUserMeals")]
        public IActionResult RemoveAllAuthUserMeals([FromServices] IMeal mealRepository)
        {
            try
            {
                mealRepository.RemoveAllMealByUserIdentifier(EntitiesHelperClass.AuthenticatedUserIdentifier(this));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }
    }
}
