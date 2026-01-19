using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Scurity.AccessToken;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Access.Generetor;

public class JwtTokenGenerator : IAccessTokenGeneretor
{
    private readonly uint _expirationTimeMinutes;
    private readonly string _signinKey;

    public JwtTokenGenerator(uint expirationTimeMinutes, string signinKey)
    {
        _expirationTimeMinutes = expirationTimeMinutes;
        _signinKey = signinKey;
    }

    public string Generete(Guid guid)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Sid, guid.ToString())
        };

        var tokenDescripter = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(securityKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateJwtSecurityToken(tokenDescripter);

        return tokenHandler.WriteToken(token);
    }

    private SecurityKey securityKey()
    {
        var bytes = Encoding.UTF8.GetBytes(_signinKey);

        return new SymmetricSecurityKey(bytes);
    }
}
