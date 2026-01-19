using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Scurity.AccessToken;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastructure.DataAcess;

namespace MyRecipeBook.Infrastructure.Services;

public class LoggedUser : ILoggedUser
{
    private readonly MyRecipeBookDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser (MyRecipeBookDbContext dbContext, ITokenProvider tokenProvider)  {
        _dbContext = dbContext;
        _tokenProvider =  tokenProvider;
    }

    public async Task<User> User ()
    {
        var token = _tokenProvider.Value();

        var tokenHendler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHendler.ReadJwtToken(token);

        var idenfier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        var userIdentifier = Guid.Parse(idenfier);

        var user = await _dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserIdentifyer == userIdentifier && u.Active);

        return user!;
    }
}
