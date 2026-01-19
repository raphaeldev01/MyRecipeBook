using System;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Microsoft.Identity.Client;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace WebApi.Test.User.Login.DoLogin;

public class DoLoginUserTest : MyRecipeBookClassFixture
{
    private readonly string Method = "api/login";

    private readonly string _email;
    private readonly string _password;
    private readonly string _name;

    public DoLoginUserTest(CustomWebApplicationFactory factory) : base (factory)
    {
        _name = factory.GetName();
        _email = factory.GetEmail();
        _password = factory.GetPassword();
    } 

    [Fact]
    public async Task Success ()
    {
        var request = new RequestDoLoginUserJson()
        {
            Email = _email,
            Password = _password
        };

        var response = await DoPost(Method, request);

        await using var responseData = await response.Content.ReadAsStreamAsync();

        var responseBody = await JsonDocument.ParseAsync(responseData);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        responseBody.RootElement.GetProperty("name").GetString().ShouldNotBeNull();
        responseBody.RootElement.GetProperty("name").GetString().ShouldBe(_name);
        responseBody.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_Email_or_Password_Invalid()
    {
        var request = RequestDoLoginUserJsonBuilder.Build();

        var response = await DoPost(Method, request);

        await using var responseData = await response.Content.ReadAsStreamAsync();

        var responseBody = await JsonDocument.ParseAsync(responseData);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        responseBody.RootElement.GetProperty("errors").EnumerateArray().ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(errorMessages => errorMessages.ToString().Equals(ResourceExceptionsMessages.EMAIL_OR_PASSWORD_INVALID))
        );
    }

    [Fact]
    public async Task Error_Email_Empty()
    {
        var request = RequestDoLoginUserJsonBuilder.Build();
        request.Email = string.Empty;

        var response = await DoPost(Method, request);

        await using var responseData = await response.Content.ReadAsStreamAsync();

        var responseBody = await JsonDocument.ParseAsync(responseData);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        responseBody.RootElement.GetProperty("errors").EnumerateArray().ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(errorMessages => errorMessages.ToString().Equals(ResourceExceptionsMessages.EMAIL_EMPTY))
        );
    }

    [Fact]
    public async Task Error_Password_Empty()
    {
        var request = RequestDoLoginUserJsonBuilder.Build();
        request.Password = string.Empty;

        var response = await DoPost(Method, request);

        await using var responseData = await response.Content.ReadAsStreamAsync();

        var responseBody = await JsonDocument.ParseAsync(responseData);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        responseBody.RootElement.GetProperty("errors").EnumerateArray().ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(errorMessages => errorMessages.ToString().Equals(ResourceExceptionsMessages.PASSWORD_EMPTY))
        );
    }
}
