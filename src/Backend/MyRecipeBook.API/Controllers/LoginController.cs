using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCase.User.Login.doLogin;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.API.Controllers
{
    public class LoginController : MyRecipeBookBaseController
    {

        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DoLogin(
            [FromServices] IDoLoginUseCase useCase,
            [FromBody] RequestDoLoginUserJson request
        )
        {
            var result = await useCase.Execute(request);

            return Ok(result);
        }
    }
}
