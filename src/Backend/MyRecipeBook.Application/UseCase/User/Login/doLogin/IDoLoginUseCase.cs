using System;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCase.User.Login.doLogin;

public interface IDoLoginUseCase
{
    public Task<ResponseRegisterUserJson> Execute(RequestDoLoginUserJson request);
}
