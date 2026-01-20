using System;
using FluentValidation;
using MyRecipeBook.Application.SharedValidators;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCase.User.Register;

public class RegisterValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceExceptionsMessages.NAME_EMPTY);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceExceptionsMessages.EMAIL_EMPTY);
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        When(user => string.IsNullOrEmpty(user.Email) == false, () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceExceptionsMessages.INVALID_EMAIL);
        });
    }
}
