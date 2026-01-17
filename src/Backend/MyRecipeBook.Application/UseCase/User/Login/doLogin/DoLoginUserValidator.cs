using System;
using System.Data;
using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCase.User.Login.doLogin;

public class DoLoginUserValidator : AbstractValidator<RequestDoLoginUserJson>
{
    public DoLoginUserValidator()
    {
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceExceptionsMessages.EMAIL_EMPTY);
        RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceExceptionsMessages.PASSWORD_EMPTY);

        When(user => string.IsNullOrEmpty(user.Email) == false, () =>
        {
            RuleFor(user => user.Email).EmailAddress();  
        });
    }
}
