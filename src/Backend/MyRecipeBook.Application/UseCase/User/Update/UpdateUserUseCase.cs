using System;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCase.User.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnly;
    private readonly IUserUpdateOnlyRepository _userUpdateOnly;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public UpdateUserUseCase(
        IUserReadOnlyRepository userReadOnly,
        IUserUpdateOnlyRepository userUpdateOnly,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser
    )
    {
        _userReadOnly = userReadOnly;
        _userUpdateOnly = userUpdateOnly;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }

    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.User();

        await Validate(request, loggedUser.Email);

        var user = await _userUpdateOnly.GetUsetById(loggedUser.Id);

        user!.Name = request.Name;
        user!.Email = request.Email;

        _userUpdateOnly.Update(user);

        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var result = new UpdateUserValidator().Validate(request);

        if(request.Email.Equals(currentEmail) == false)
        {
            var userWithEmail = await _userReadOnly.ExistActiveUsersWithEmail(request.Email);
            if(userWithEmail)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("EMAIL", ResourceExceptionsMessages.EMAIL_EXISTS));
        }

        if(result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidate(errorMessages);
        }

        
    }
}
