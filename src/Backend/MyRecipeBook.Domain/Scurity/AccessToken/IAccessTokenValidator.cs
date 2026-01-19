using System;

namespace MyRecipeBook.Domain.Scurity.AccessToken;

public interface IAccessTokenValidator
{
    public Guid ValidateTokenAndGeyUserIndentify (string token);
}
