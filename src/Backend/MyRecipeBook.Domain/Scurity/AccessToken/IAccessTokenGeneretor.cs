using System;

namespace MyRecipeBook.Domain.Scurity.AccessToken;

public interface IAccessTokenGeneretor
{
    public string Generete (Guid guid); 
}
