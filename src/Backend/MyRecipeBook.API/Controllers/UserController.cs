using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Application.UseCase.User.Register;
using MyRecipeBook.Application.UseCase.User.GetProfile;
using MyRecipeBook.API.Attributes;

namespace MyRecipeBook.API.Controllers;

public class UserController : MyRecipeBookBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request
        )
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseProfileUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [AuthenticatedUser]
    public async Task<IActionResult> GetUserProfile(
        [FromServices] IGetProfileUserUseCase useCase
    )
    {
        var result = await useCase.Execute();
        return Ok(result);
    }
}