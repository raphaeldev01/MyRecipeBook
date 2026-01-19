using System;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCase.User.GetProfile;

public interface IGetProfileUserUseCase
{
    public Task<ResponseProfileUserJson> Execute();
}
