using System;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace WebApi.Test.User.GetProfile;

public class GetProfileUserInvalidTokenTest : MyRecipeBookClassFixture
{
    public string Method = "api/user";

    public GetProfileUserInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact] 
    public async Task Error_Token_Invalid()
    {
        var response = await DoGet(Method, "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await  JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("errors").EnumerateArray().ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(erroMessage => erroMessage.ToString().Equals(ResourceExceptionsMessages.TOKEN_WITHOUT_PERMISSION))
        );
    }

    [Fact] 
    public async Task Error_Token_Empty()
    {
        var response = await DoGet(Method);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await  JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("errors").EnumerateArray().ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(erroMessage => erroMessage.ToString().Equals(ResourceExceptionsMessages.NO_TOKEN))
        );
    }

    [Fact] 
    public async Task Error_Valid_Without_User()
    {
        var token = new AccessTokenGeneretorBuilder().Build().Generete(Guid.NewGuid());

        var response = await DoGet(Method, token.ToString());

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await  JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("errors").EnumerateArray().ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(erroMessage => erroMessage.ToString().Equals(ResourceExceptionsMessages.TOKEN_WITHOUT_PERMISSION))
        );
    }
}
