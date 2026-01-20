using System;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace WebApi.Test.User.UpdateUser;

public class UpdateUserTest : MyRecipeBookClassFixture
{
    private readonly string _method = "api/user";
    private readonly string _email;
    private readonly string _name;
    private readonly Guid _userIdentifier;

    public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _email = factory.GetEmail();
        _name = factory.GetName();
        _userIdentifier = factory.GetGuid();
    }

    [Fact]
    public async Task Success()
    {
        var token = new AccessTokenGeneretorBuilder().Build().Generete(_userIdentifier);
        var request = RequestUpdateUserJsonBuilder.Build(); 

        var response = await DoPut(_method, request, token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var token = new AccessTokenGeneretorBuilder().Build().Generete(_userIdentifier);
        var request = RequestUpdateUserJsonBuilder.Build(); 
        request.Name = string.Empty;

        var response = await DoPut(_method, request, token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var Exceptions = responseData.RootElement.GetProperty("errors").EnumerateArray();
        Exceptions.ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(erroMessage => erroMessage.ToString().Equals(ResourceExceptionsMessages.NAME_EMPTY))
        );
    }
    [Fact]

    public async Task Error_Email_Empty()
    {
        var token = new AccessTokenGeneretorBuilder().Build().Generete(_userIdentifier);
        var request = RequestUpdateUserJsonBuilder.Build(); 
        request.Email = string.Empty;

        var response = await DoPut(_method, request, token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var Exceptions = responseData.RootElement.GetProperty("errors").EnumerateArray();
        Exceptions.ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(erroMessage => erroMessage.ToString().Equals(ResourceExceptionsMessages.EMAIL_EMPTY))
        );
    }

    [Fact]
    public async Task Error_Email_Invalid()
    {
        var token = new AccessTokenGeneretorBuilder().Build().Generete(_userIdentifier);
        var request = RequestUpdateUserJsonBuilder.Build(); 
        request.Email = "raphael.com";

        var response = await DoPut(_method, request, token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var Exceptions = responseData.RootElement.GetProperty("errors").EnumerateArray();
        Exceptions.ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(erroMessage => erroMessage.ToString().Equals(ResourceExceptionsMessages.INVALID_EMAIL))
        );
    }

    [Fact]
    public async Task Error_Without_Permission_Token()
    {
        var token = new AccessTokenGeneretorBuilder().Build().Generete(Guid.NewGuid());
        var request = RequestUpdateUserJsonBuilder.Build(); 

        var response = await DoPut(_method, request, token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var Exceptions = responseData.RootElement.GetProperty("errors").EnumerateArray();
        Exceptions.ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(erroMessage => erroMessage.ToString().Equals(ResourceExceptionsMessages.TOKEN_WITHOUT_PERMISSION))
        );
    }

}
