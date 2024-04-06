using Microsoft.AspNetCore.Mvc;
using NutriLensClassLibrary.Models;
using NutriLensWebApp.Interfaces;

namespace NutriLensWebApp.Controllers
{
    public class PlaygroundController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Gpt4VisionTester()
        {
            return View();
        }

        public IActionResult ViewAllPictures()
        {
            return View();
        }

        public IActionResult PictureAnalysis([FromServices] IMongoImage mongoImageRepo)
        {
            List<string> imagesIds = mongoImageRepo.GetAllImagesIds();
            return View(imagesIds);
        }
    }
}
