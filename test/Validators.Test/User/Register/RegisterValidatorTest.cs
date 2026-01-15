using System;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCase.User.Register;
using MyRecipeBook.Exceptions;
using Shouldly;
using Xunit;

namespace Validators.Test.User.Register;

public class RegisterValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterValidator();

        var request = RequestRegisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Name()
    {
        var validator = new RegisterValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        // result.Errors.ShouldSatisfyAllConditions( e => e.ShouldContain(errorMessage => errorMessage.Equals(ResourceExceptionsMessages.NAME_EMPTY)));
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(errorMessage => errorMessage.ErrorMessage.Equals(ResourceExceptionsMessages.NAME_EMPTY));
    }

    [Fact]
    public void Error_Email_Empty ()
    {
        var validator = new RegisterValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceExceptionsMessages.EMAIL_EMPTY));
    }


    [Fact]
    public void Error_Email_Invalid ()
    {
        var validator = new RegisterValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "email.com";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceExceptionsMessages.INVALID_EMAIL));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Shot (int passwordLength)
    {
        var validator = new RegisterValidator();

        var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceExceptionsMessages.SHORT_PASSWORD));
    }
}
