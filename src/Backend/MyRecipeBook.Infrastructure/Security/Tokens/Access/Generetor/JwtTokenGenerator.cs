using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Scurity.AccessToken;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Access.Generetor;

public class JwtTokenGenerator : JwtTokenHandler,  IAccessTokenGeneretor
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
            SigningCredentials = new SigningCredentials(securityKey(_signinKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateJwtSecurityToken(tokenDescripter);

        return tokenHandler.WriteToken(token);
    }
}
