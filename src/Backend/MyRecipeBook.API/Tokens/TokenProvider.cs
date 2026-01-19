using System;
using MyRecipeBook.Domain.Scurity.AccessToken;

namespace MyRecipeBook.API.Tokens;

public class TokenProvider : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContext;

    public TokenProvider (IHttpContextAccessor httpContextAccessor) => _httpContext = httpContextAccessor;

    public string Value()
    {
        var authorization = _httpContext.HttpContext!.Request.Headers.Authorization.ToString();

        return authorization["Bearer".Length..].Trim();
    }
}
