using System;
using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCase.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(u => u.Name).NotEmpty().WithMessage(ResourceExceptionsMessages.NAME_EMPTY);
        RuleFor(u => u.Email).NotEmpty().WithMessage(ResourceExceptionsMessages.EMAIL_EMPTY);

        When(u => string.IsNullOrEmpty(u.Email) == false, () => {
            RuleFor(u => u.Email).EmailAddress().WithMessage(ResourceExceptionsMessages.INVALID_EMAIL);
        });
    }
}
