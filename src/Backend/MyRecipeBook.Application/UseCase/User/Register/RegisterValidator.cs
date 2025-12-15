using System;
using FluentValidation;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCase.User.Register;

public class RegisterValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterValidator()
    {
        RuleFor(user => user.Name).NotEmpty();
        RuleFor(user => user.Email).NotEmpty();
        RuleFor(user => user.Email).EmailAddress();
        RuleFor(user => user.Password.Length).GreaterThanOrEqualTo(6);
    }
}
