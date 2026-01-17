using System;
using System.Threading.Tasks;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCase.User.Login.doLogin;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.User.Login.DoLogin;

public class DoLoginValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestDoLoginUserJsonBuilder.Build();

        var validator = new DoLoginUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(true);
    }

    [Fact] 
    public void Error_Email_Empty ()
    {
        var request = RequestDoLoginUserJsonBuilder.Build();
        request.Email = string.Empty;

        var validator = new DoLoginUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(erroMessage => erroMessage.ErrorMessage.Equals(ResourceExceptionsMessages.EMAIL_EMPTY));
    }

[Fact] 
    public void Error_Email_Invalid ()
    {
        var request = RequestDoLoginUserJsonBuilder.Build();
        request.Email = "raphael.com";

        var validator = new DoLoginUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(erroMessage => erroMessage.ErrorMessage.Equals(ResourceExceptionsMessages.INVALID_EMAIL));
    }

    
    [Fact] 
    public void Error_Password_Empty ()
    {
        var request = RequestDoLoginUserJsonBuilder.Build();
        request.Password = string.Empty;

        var validator = new DoLoginUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(erroMessage => erroMessage.ErrorMessage.Equals(ResourceExceptionsMessages.PASSWORD_EMPTY));
    }
}
