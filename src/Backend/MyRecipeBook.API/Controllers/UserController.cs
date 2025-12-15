using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Application.UseCase.User.Register;

namespace MyRecipeBook.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    public IActionResult Register(RequestRegisterUserJson request)
    {
        var useCase = new RegisterUserUseCase();

        var result = useCase.Execute(request);

        return Created(string.Empty, result);
    }
}