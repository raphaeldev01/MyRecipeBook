using System;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCase.User.UpdatePassword;

public interface IUpdatePasswordUseCase
{
    public Task Execute(RequestUpdatePasswordJson json);

}
