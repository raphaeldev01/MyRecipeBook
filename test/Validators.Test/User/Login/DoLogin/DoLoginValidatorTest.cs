using System;
using System.Threading.Tasks;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCase.User.Login.doLogin;
using Shouldly;

namespace Validators.Test.User.Login.DoLogin;

public class DoLoginValidatorTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestDoLoginUserJsonBuilder.Build();

        var validator = new DoLoginUserValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(true);
    }

}
