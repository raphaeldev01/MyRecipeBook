using System;
using FluentValidation;
using MyRecipeBook.Application.SharedValidators;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCase.User.UpdatePassword;

public class UpdatePasswordValidator : AbstractValidator<RequestUpdatePasswordJson>
{
    public UpdatePasswordValidator()
    {
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestUpdatePasswordJson>());
        RuleFor(user => user.NewPassword).SetValidator(new PasswordValidator<RequestUpdatePasswordJson>());
    }
}
