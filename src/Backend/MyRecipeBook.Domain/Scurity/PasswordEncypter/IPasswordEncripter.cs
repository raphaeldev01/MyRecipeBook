using System;

namespace MyRecipeBook.Domain.Scurity.Cryptography;

public interface IPasswordEncripter
{
    public string Encrypt (string password);
}
