using ExceptionLibrary;
using GenerativeAI.Models;
using GenerativeAI.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriLensClassLibrary.Models;
using NutriLensWebApp.Entities;
using NutriLensWebApp.Interfaces;
using NutriLensWebApp.Models;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using WebLibrary;
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
        #region GPT 4

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

                OpenAiResponse response = OpenAiQuery(OpenAiModel.Gpt4Turbo, inputModel);

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

                OpenAiResponse response = OpenAiQuery(OpenAiModel.Gpt4Turbo, inputModel);

                return Ok(response.GetResponseMessage());
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPost, Route("v1/DetectFoodByMongoImageId/{mongoImageId}"), AllowAnonymous]
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

                OpenAiResponse response = OpenAiQuery(OpenAiModel.Gpt4Turbo, inputModel);

                mongoImage.GptRawResult = "[OpenAi] " + response.GetResponseMessage();

                mongoImageRepo.UpdateImage(mongoImage);

                return Ok(mongoImage.GptRawResult);
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
                OpenAiResponse response = OpenAiQuery(OpenAiModel.Gpt4Turbo, inputModel);
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

        [HttpPost, Route("v1/GetFoodItemsJsonByMealDescription")]
        public IActionResult GetFoodItemsJsonByMealDescription([FromBody] StringObject mealDescription)
        {
            try
            {
                OpenAiInputModel inputModel = new()
                {
                    UserPrompt = mealDescription.Value,
                    SystemPrompt = "Você obtém uma descrição de uma refeição, e, para cada item mencionado, " +
                    "você monta um objeto composto por Item e Quantidade e devolve uma lista desses objetos. " +
                    "O usuário pode ou não falar a quantidade em gramas de cada um dos itens, caso ele não mencione, " +
                    "considere a medida em gramas típica para o item em uma refeição.",
                    MaxTokens = 300
                };

                OpenAiResponse response = OpenAiQuery(OpenAiModel.Gpt4Turbo, inputModel);

                return Ok(response.GetResponseMessage());
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        #endregion

        #region Gemini

        [HttpPost, Route("v1/DetectFoodByMongoImageId/gemini/{mongoImageId}"), AllowAnonymous]
        public async Task<IActionResult> DetectFoodByMongoImageIdGemini(string mongoImageId,
            [FromServices] IOpenAiPrompt openAiPromptRepo,
            [FromServices] IMongoImage mongoImageRepo)
        {
            try
            {

                OpenAiPrompt openAiPrompt = openAiPromptRepo.GetLast();

                MongoImage mongoImage = mongoImageRepo.GetById(mongoImageId);

                string prompt = openAiPrompt.SystemPrompt;

                //string prompt = "List of items in brazilian portuguese, and with an estimative of quantity in grams, format json. A list with only two proprierties, Item and Quantidade ";

                string result = await GeminiAiEntity.GeminiAiQuery(prompt, mongoImage.ImageBytes);

                mongoImage.GeminiRawResult = "[Gemini] " + result;

                mongoImageRepo.UpdateImage(mongoImage);

                return Ok(mongoImage.GeminiRawResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPost, Route("v1/GeminiVisionTest"), AllowAnonymous]
        public async Task<IActionResult> GeminiVisionTest([FromBody] GeminiVisionInputModel inputModel)
        {
            try
            {
                string prompt = inputModel.Prompt;
                byte[] imageBytes = Convert.FromBase64String(inputModel.Url.Substring(inputModel.Url.IndexOf(',') + 1));
                string result = await GeminiAiEntity.GeminiAiQuery(prompt, imageBytes);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPost, Route("v1/GetFoodItemsJsonByMealDescription/gemini")]
        public async Task<IActionResult> GetFoodItemsJsonByMealDescriptionGemini([FromBody] StringObject mealDescription)
        {
            try
            {

                string prompt = $"Considere a descrição de refeição: '{mealDescription.Value}'. Para cada item mencionado, " +
                "você monta um objeto composto por Item e Quantidade e devolve uma lista desses objetos, em json. " +
                "O usuário pode ou não falar a quantidade em gramas de cada um dos itens, caso ele não mencione, " +
                "considere a medida em gramas típica para o item em uma refeição.";

                string result = await GeminiAiEntity.GeminiAiQuery(prompt);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        #endregion

        #region Both

        [HttpPost, Route("v2/DetectFoodByMongoImageId/{mongoImageId}"), AllowAnonymous]
        public async Task<IActionResult> DetectFoodByMongoImageIdV2(string mongoImageId,
            [FromServices] IOpenAiPrompt openAiPromptRepo,
            [FromServices] IMongoImage mongoImageRepo)
        {
            try
            {
                OpenAiPrompt openAiPrompt = openAiPromptRepo.GetLast();
                MongoImage mongoImage = mongoImageRepo.GetById(mongoImageId);
                string mongoImageBase64 = Convert.ToBase64String(mongoImage.ImageBytes);

                OpenAiResponse gptResponse = null;
                string geminiResponse = string.Empty;

                TaskCompletionSource<bool> tcsGpt = new TaskCompletionSource<bool>();

                Task.Run(() =>
                {
                    try
                    {
                        OpenAiVisionInputModel inputModel = new()
                        {
                            UserPrompt = openAiPrompt.UserPrompt,
                            SystemPrompt = openAiPrompt.SystemPrompt,
                            MaxTokens = 300,
                            Base64 = false,
                            Url = $"data:image/jpeg;base64,{mongoImageBase64}"
                        };

                        gptResponse = OpenAiQuery(OpenAiModel.Gpt4Turbo, inputModel);
                        tcsGpt.SetResult(true);
                    }
                    catch (Exception ex)
                    {
                        tcsGpt.SetResult(false);
                    }

                });

                TaskCompletionSource<bool> tcsGemini = new TaskCompletionSource<bool>();

                Task.Run(async () =>
                {
                    try
                    {
                        string prompt = openAiPrompt.SystemPrompt;
                        geminiResponse = await GeminiAiEntity.GeminiAiQuery(prompt, mongoImage.ImageBytes);
                        tcsGemini.SetResult(true);
                    }
                    catch (Exception ex)
                    {
                        tcsGemini.SetResult(false);
                    }
                });

                // Aguarda as consultas terminarem
                do
                {
                    await Task.Delay(1000);

                    Console.WriteLine("GPT: " + tcsGpt.Task.IsCompleted);
                    Console.WriteLine("Gemini: " + tcsGemini.Task.IsCompleted);

                } while (!tcsGpt.Task.IsCompleted || !tcsGemini.Task.IsCompleted);

                mongoImage.GptRawResult = "[OpenAi] " + gptResponse.GetResponseMessage();
                mongoImage.GeminiRawResult = "[Gemini] " + geminiResponse;

                mongoImageRepo.UpdateImage(mongoImage);

                return Ok(new AiResult(mongoImage.GptRawResult, mongoImage.GeminiRawResult));
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        [HttpPost, Route("v2/GetFoodItemsJsonByMealDescription")]
        public async Task<IActionResult> GetFoodItemsJsonByMealDescriptionV2([FromBody] StringObject mealDescription)
        {
            try
            {
                OpenAiResponse gptResponse = null;
                string geminiResponse = null;

                TaskCompletionSource<bool> tcsGpt = new TaskCompletionSource<bool>();

                Task.Run(() =>
                {
                    try
                    {
                        OpenAiInputModel inputModel = new()
                        {
                            UserPrompt = mealDescription.Value,
                            SystemPrompt = "Você obtém uma descrição de uma refeição, e, para cada item mencionado, " +
                            "você monta um objeto composto por Item e Quantidade e devolve uma lista desses objetos. " +
                            "O usuário pode ou não falar a quantidade em gramas de cada um dos itens, caso ele não mencione, " +
                            "considere a medida em gramas típica para o item em uma refeição.",
                            MaxTokens = 300
                        };
                        gptResponse = OpenAiQuery(OpenAiModel.Gpt4Turbo, inputModel);
                        tcsGpt.SetResult(true);
                    }
                    catch (Exception ex)
                    {
                        tcsGpt.SetResult(false);
                    }

                });

                TaskCompletionSource<bool> tcsGemini = new TaskCompletionSource<bool>();

                Task.Run(async () =>
                {
                    try
                    {
                        string prompt = $"Considere a descrição de refeição: '{mealDescription.Value}'. Para cada item mencionado, " +
                        "você monta um objeto composto por Item e Quantidade e devolve uma lista desses objetos, em json. " +
                        "O usuário pode ou não falar a quantidade em gramas de cada um dos itens, caso ele não mencione, " +
                        "considere a medida em gramas típica para o item em uma refeição.";

                        geminiResponse = await GeminiAiEntity.GeminiAiQuery(prompt);
                        tcsGemini.SetResult(true);
                    }
                    catch (Exception ex)
                    {
                        tcsGemini.SetResult(false);
                    }
                });

                // Aguarda as consultas terminarem
                do
                {
                    await Task.Delay(1000);

                    Console.WriteLine("GPT: " + tcsGpt.Task.IsCompleted);
                    Console.WriteLine("Gemini: " + tcsGemini.Task.IsCompleted);

                } while (!tcsGpt.Task.IsCompleted || !tcsGemini.Task.IsCompleted);

                return Ok(new AiResult(gptResponse.GetResponseMessage(), geminiResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }

        #endregion

        [HttpGet, Route("v1/GetFoodItemsByInput/{input}")]
        public IActionResult GetFoodItemsByInput(string input)
        {
            try
            {
                return Ok(JsonToFoodItemsParser.Parse(input));
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionManager.ExceptionMessage(ex));
            }
        }
    }
}
