using System;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using Shouldly;

namespace WebApi.Test.User.GetProfile;

public class GetProfileUserTest : MyRecipeBookClassFixture
{
    private string Method = "api/user";
    private readonly string _email;
    private readonly string _name;
    private readonly Guid _userIdentifier;

    public GetProfileUserTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _email = factory.GetEmail();
        _name = factory.GetName();
        _userIdentifier = factory.GetGuid();
    }
    
    [Fact]
    public async Task Success()
    {
        var token = new AccessTokenGeneretorBuilder().Build().Generete(_userIdentifier);

        var response = await DoGet(Method, token);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_name);
        responseData.RootElement.GetProperty("email").GetString().ShouldBe(_email);
    }
}
