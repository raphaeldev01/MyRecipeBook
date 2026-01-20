using System;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCase.User.Update;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.User.Update;

public class UpdateValidatorTest
{
    [Fact]
    public async Task Seccess()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var validator = new UpdateUserValidator();
        
        var result = validator.Validate(request);

        result.IsValid.ShouldBe(true);
    }

    [Fact]
    public async Task Error_Email_Empty()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;

        var validator = new UpdateUserValidator();
        
        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(erroMessage => erroMessage.ErrorMessage.Equals(ResourceExceptionsMessages.EMAIL_EMPTY))
        );
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var validator = new UpdateUserValidator();
        
        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(erroMessage => erroMessage.ErrorMessage.Equals(ResourceExceptionsMessages.NAME_EMPTY))
        );
    }

    [Fact]
    public async Task Error_Email_Invalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "raphael.com";

        var validator = new UpdateUserValidator();
        
        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(erroMessage => erroMessage.ErrorMessage.Equals(ResourceExceptionsMessages.INVALID_EMAIL))
        );
    }
}
