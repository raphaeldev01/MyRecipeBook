using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;

public class MyRecipeBookClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public MyRecipeBookClassFixture(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string method, object request)
    {
        return await _httpClient.PostAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoGet(string method, string? token = "")
    {
        AuthorizeRequest(token);
        return await _httpClient.GetAsync(method);
    }

    private void AuthorizeRequest(string? token = "")
    {
        if (string.IsNullOrEmpty(token))
            return;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
