using System;

namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class ErrorOnDoLogin : MyRecipeBookException
{
    public ErrorOnDoLogin() : base(ResourceExceptionsMessages.EMAIL_OR_PASSWORD_INVALID)
    {
        
    }
}
