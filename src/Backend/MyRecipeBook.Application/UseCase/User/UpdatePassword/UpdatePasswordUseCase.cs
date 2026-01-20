using System;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Domain.Scurity.Cryptography;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCase.User.UpdatePassword;

public class UpdatePasswordUseCase : IUpdatePasswordUseCase
{
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUserUpdateOnlyRepository _userUpdate;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePasswordUseCase(
        IPasswordEncripter passwordEncripter,
        IUserUpdateOnlyRepository userUpdateOnly,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork
    )
    {
        _loggedUser = loggedUser;
        _passwordEncripter = passwordEncripter;
        _userUpdate = userUpdateOnly;
        _unitOfWork = unitOfWork;
    }


    public async Task Execute(RequestUpdatePasswordJson request)
    {
        var userLogged = await _loggedUser.User();
        var user = await _userUpdate.GetUserById(userLogged.Id);

        await Validate(request, user!);

        user!.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _userUpdate.Update(user);

        await _unitOfWork.Commit();
    }

    private async Task Validate (RequestUpdatePasswordJson request, Domain.Entities.User user)
    {
        var result = new UpdatePasswordValidator().Validate(request);

        if(result.IsValid == false)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidate(errors);
        }

        if(_passwordEncripter.Encrypt(request.Password).Equals(user.Password) == false)
        {
            var errors = new List<string>()
            {
                ResourceExceptionsMessages.PASSWORD_INCORRECT
            };

            throw new ErrorOnValidate(errors);
        }
    }    
}
