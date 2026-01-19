using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.API.Filters;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Domain.Scurity.AccessToken;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.API.Attributes;

public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
    {
    }
}
