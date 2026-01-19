using System;

namespace MyRecipeBook.Domain.Scurity.AccessToken;

public interface ITokenProvider 
{
    public string Value ();
}
