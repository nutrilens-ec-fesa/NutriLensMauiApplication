using ExceptionLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult DetectFoodByUrl([FromBody] string url)
        {
            try
            {
                OpenAiVisionInputModel inputModel = new OpenAiVisionInputModel
                {
                    UserPrompt = "Analise essa foto.",
                    SystemPrompt = "Você identifica itens alimentícios de uma refeição, trazendo seu nome e a quantidade em gramas ou ml, em itens. Eu sei que as quantidades são aproximadas, portanto, não é necessário escrever aproximadamente ou coisa do tipo. Você é objetivo e não retorna mais nada além do solicitado.",
                    MaxTokens = 300,
                    Base64 = false,
                    Url = url
                };

                OpenAiResponse response = OpenAiQuery(OpenAiModel.Gpt4VisionPreview, inputModel);

                return Ok(response.GetResponseMessage());
            }
            catch(Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPost, Route("v1/DetectFoodByBase64Image")]
        public IActionResult DetectFoodByBase64Image([FromBody] string base64Image)
        {
            try
            {
                OpenAiVisionInputModel inputModel = new OpenAiVisionInputModel
                {
                    UserPrompt = "Analise essa foto.",
                    SystemPrompt = "Você identifica itens alimentícios de uma refeição, trazendo seu nome e a quantidade em gramas ou ml, em itens. Eu sei que as quantidades são aproximadas, portanto, não é necessário escrever aproximadamente ou coisa do tipo. Você é objetivo e não retorna mais nada além do solicitado.",
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
    }
}
