using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Access;

public abstract class JwtTokenHandler
{
    protected SecurityKey securityKey(string siginKey)
    {
        var bytes = Encoding.UTF8.GetBytes(siginKey);

        return new SymmetricSecurityKey(bytes);
    }
}
