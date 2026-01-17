using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Azure.Core;
using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Identity.Client;
using MyRecipeBook.Exceptions;
using Shouldly;

namespace WebApi.Test.User.Register;

public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public RegisterUserTest(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await _httpClient.PostAsJsonAsync("api/user", request);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldNotBeNull();
        responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await _httpClient.PostAsJsonAsync("api/user", request);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldContain(erro => erro.GetString()!.Equals(ResourceExceptionsMessages.NAME_EMPTY));
        errors.ShouldHaveSingleItem();
    } 

    [Fact]
    public async Task Error_Email_Empty()
    {  
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var response = await _httpClient.PostAsJsonAsync("api/user", request);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
 
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(errorMessage => errorMessage.GetString()!.Equals(ResourceExceptionsMessages.EMAIL_EMPTY))
        );
    }

    [Fact]
    public async Task Error_Email_Invalid ()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "raphael.com";

        var response = await _httpClient.PostAsJsonAsync("api/user", request);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(errorMessage => errorMessage.GetString()!.Equals(ResourceExceptionsMessages.INVALID_EMAIL))
        );
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
        var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

        var response = await _httpClient.PostAsJsonAsync("api/user", request);

        await using var ResponseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(ResponseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldSatisfyAllConditions(
            e => e.ShouldHaveSingleItem(),
            e => e.ShouldContain(errorMessage => errorMessage.GetString()!.Equals(ResourceExceptionsMessages.SHORT_PASSWORD))
        );
    }
}

