using System;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace WebApi.Test.User.UpdatePassword;

public class UpdatePasswordTest : MyRecipeBookClassFixture
{
    private readonly string _method = "api/user/change-password";
    private readonly Guid _userIdentifier;
    private readonly string _password;

    public UpdatePasswordTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetGuid();
        _password = factory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var token = new AccessTokenGeneretorBuilder().Build().Generete(_userIdentifier);
        var request = RequestUpdatePasswordJsonBuilder.Build();

        request.Password = _password;

        var response = await DoPut(_method, request, token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Password_Invalid()
    {
        var token = new AccessTokenGeneretorBuilder().Build().Generete(_userIdentifier);
        var request = RequestUpdatePasswordJsonBuilder.Build();

        var response = await DoPut(_method, request, token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errorMessages = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errorMessages.ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(em => em.ToString().Equals(ResourceExceptionsMessages.PASSWORD_INCORRECT))
        );
    }
}
