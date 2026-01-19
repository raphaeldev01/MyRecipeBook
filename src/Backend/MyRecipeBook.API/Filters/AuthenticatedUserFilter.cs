using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Communication.Responses;


// using MyRecipeBook.API.Attributes;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Domain.Scurity.AccessToken;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.API.Filters;

public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IAccessTokenValidator _accessTokenValidator;

    public AuthenticatedUserFilter(IUserReadOnlyRepository repository, IAccessTokenValidator accessTokenValidator)
    {
        _repository = repository;
        _accessTokenValidator = accessTokenValidator;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);

            var userIdentifier = _accessTokenValidator.ValidateTokenAndGeyUserIndentify(token);

            var exists = await _repository.ExistActiveUserWithUserIdentifier(userIdentifier);

            if (exists == false)
            {
                throw new MyRecipeBookException(ResourceExceptionsMessages.TOKEN_WITHOUT_PERMISSION);
            }
        }
        catch (SecurityTokenExpiredException ex)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("Token Expired")
            {
                TokenIsExpired = true
            });
        }
        catch (MyRecipeBookException ex)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ex.Message));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceExceptionsMessages.TOKEN_WITHOUT_PERMISSION));
        }
    }

    private string TokenOnRequest(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(token))
        {
            throw new MyRecipeBookException(ResourceExceptionsMessages.NO_TOKEN);
        }

        return token["Bearer ".Length..].Trim();
    }
}
