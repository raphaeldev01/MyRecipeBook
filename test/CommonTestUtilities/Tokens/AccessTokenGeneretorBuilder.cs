using System;
using MyRecipeBook.Domain.Scurity.AccessToken;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generetor;

namespace CommonTestUtilities.Tokens;

public class AccessTokenGeneretorBuilder
{
    public IAccessTokenGeneretor Build () => new JwtTokenGenerator(1000, "tttttttttttttttttttttttttttttttt");
}
