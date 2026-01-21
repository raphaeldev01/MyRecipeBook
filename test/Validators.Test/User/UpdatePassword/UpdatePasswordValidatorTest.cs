using System;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCase.User.UpdatePassword;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.User.UpdatePassword;

public class UpdatePasswordValidatorTest
{
    [Fact]
    public async Task Success ()
    {
        var request = RequestUpdatePasswordJsonBuilder.Build();

        var validator = new UpdatePasswordValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(true);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Error_Password_Short (int passwordLength)
    {
        var request = RequestUpdatePasswordJsonBuilder.Build(passwordLength);

        var validator = new UpdatePasswordValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(em => em.ErrorMessage.Equals(ResourceExceptionsMessages.SHORT_PASSWORD))
        );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Error_New_Password_Short (int passwordLength)
    {
        var request = RequestUpdatePasswordJsonBuilder.Build(10, passwordLength);

        var validator = new UpdatePasswordValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(em => em.ErrorMessage.Equals(ResourceExceptionsMessages.SHORT_PASSWORD))
        );
    }
}
