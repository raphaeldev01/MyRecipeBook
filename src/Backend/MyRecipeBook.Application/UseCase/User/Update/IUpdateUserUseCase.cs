using System;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCase.User.Update;

public interface IUpdateUserUseCase
{
    public Task Execute (RequestUpdateUserJson request);
}
