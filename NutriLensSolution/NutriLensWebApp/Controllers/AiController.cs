using ExceptionLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriLensClassLibrary.Models;
using NutriLensWebApp.Interfaces;
using NutriLensWebApp.Models;
using static NutriLensWebApp.Entities.OpenAiEntity;

namespace NutriLensWebApp.Controllers
{
#if DEBUG
    [AllowAnonymous]
    //[Authorize]
#else
    [Authorize]
#endif
    [Route("[controller]")]
    public class AiController : ControllerBase
    {
        [HttpPost, Route("v1/DetectFoodByUrl")]
        public IActionResult DetectFoodByUrl([FromBody] string url, [FromServices] IOpenAiPrompt openAiPromptRepo)
        {
            // UserPrompt = "Analise essa foto. Retorne em json no formato: [{Item:item,Quantidade:g},{Item:item,Quantidade:g}]",
            // SystemPrompt = "Você identifica itens alimentícios de uma refeição, trazendo em json uma lista de item e quantidade em gramas ou ml, de cada item. Eu sei que as quantidades são aproximadas, portanto, não é necessário escrever aproximadamente ou coisa do tipo. Você é objetivo e não retorna mais nada além do solicitado.",

            try
            {
                OpenAiPrompt openAiPrompt = openAiPromptRepo.GetLast();

                OpenAiVisionInputModel inputModel = new()
                {
                    UserPrompt = openAiPrompt.UserPrompt,
                    SystemPrompt = openAiPrompt.SystemPrompt,
                    MaxTokens = 300,
                    Base64 = false,
                    Url = url
                };

                OpenAiResponse response = OpenAiQuery(OpenAiModel.Gpt4VisionPreview, inputModel);

                return Ok(response.GetResponseMessage());
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPost, Route("v1/DetectFoodByBase64Image")]
        public IActionResult DetectFoodByBase64Image([FromBody] string base64Image, [FromServices] IOpenAiPrompt openAiPromptRepo)
        {
            try
            {
                OpenAiPrompt openAiPrompt = openAiPromptRepo.GetLast();

                OpenAiVisionInputModel inputModel = new()
                {
                    UserPrompt = openAiPrompt.UserPrompt,
                    SystemPrompt = openAiPrompt.SystemPrompt,
                    MaxTokens = 300,
                    Base64 = false,
                    Url = $"data:image/jpeg;base64,{base64Image}"
                };

                OpenAiResponse response = OpenAiQuery(OpenAiModel.Gpt4VisionPreview, inputModel);

                return Ok(response.GetResponseMessage());
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPost, Route("v1/DetectFoodByMongoImageId/{mongoImageId}")]
        public IActionResult DetectFoodByMongoImageId(string mongoImageId, 
            [FromServices] IOpenAiPrompt openAiPromptRepo,
            [FromServices] IMongoImage mongoImageRepo)
        {
            try
            {
                OpenAiPrompt openAiPrompt = openAiPromptRepo.GetLast();

                MongoImage mongoImage = mongoImageRepo.GetById(mongoImageId);

                string mongoImageBase64 = Convert.ToBase64String(mongoImage.ImageBytes);

                OpenAiVisionInputModel inputModel = new()
                {
                    UserPrompt = openAiPrompt.UserPrompt,
                    SystemPrompt = openAiPrompt.SystemPrompt,
                    MaxTokens = 300,
                    Base64 = false,
                    Url = $"data:image/jpeg;base64,{mongoImageBase64}"
                };

                OpenAiResponse response = OpenAiQuery(OpenAiModel.Gpt4VisionPreview, inputModel);

                return Ok(response.GetResponseMessage());
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPost, Route("v1/Gpt4VisionTest"), AllowAnonymous]
        public IActionResult Gpt4VisionTest([FromBody] OpenAiVisionInputModel inputModel)
        {
            try
            {
                OpenAiResponse response = OpenAiQuery(OpenAiModel.Gpt4VisionPreview, inputModel);
                return Ok(response.GetResponseMessage());
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpGet, Route("v1/GetActualGpt4VisionPrompt"), AllowAnonymous]
        public IActionResult GetActualGpt4VisionPrompt([FromServices] IOpenAiPrompt openAiPromptRepo)
        {
            try
            {
                return Ok(openAiPromptRepo.GetLast());
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPost, Route("v1/InsertNewGpt4VisionPrompt"), AllowAnonymous]
        public IActionResult InsertNewPrompt([FromBody] OpenAiPrompt openAiPrompt,
            [FromServices] IOpenAiPrompt openAiPromptRepo)
        {
            try
            {
                openAiPromptRepo.InsertNew(openAiPrompt);
                return Created(string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }
    }
}
