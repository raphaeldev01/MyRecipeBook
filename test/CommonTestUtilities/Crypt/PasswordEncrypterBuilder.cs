using System;
using MyRecipeBook.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Crypt;

public class PasswordEncrypterBuilder
{
    public static Sha512Encrypter Build () => new Sha512Encrypter("abc123");
    
}
