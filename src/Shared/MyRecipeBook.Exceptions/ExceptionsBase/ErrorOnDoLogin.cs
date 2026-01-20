using System;

namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class ErrorOnDoLogin : MyRecipeBookException
{
    public List<string> ErrorMessages { get; set; }

    public ErrorOnDoLogin(List<string>? errorMessages = null) : base(string.Empty)
    {
        if(errorMessages != null) ErrorMessages = errorMessages!;
        else
        {
            var errorInvalid = new List<string>
            {
                ResourceExceptionsMessages.EMAIL_OR_PASSWORD_INVALID
            };
            ErrorMessages = errorInvalid;
        }

    }
}
