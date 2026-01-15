using System;
using MyRecipeBook.Application.Services.Cryptography;

namespace CommonTestUtilities.Crypt;

public class PasswordEncrypterBuilder
{
    public static PasswordEncripter Build () => new PasswordEncripter("abc123");
    
}
