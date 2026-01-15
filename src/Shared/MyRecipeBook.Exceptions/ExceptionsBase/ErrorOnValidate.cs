using System;

namespace MyRecipeBook.Exceptions.ExceptionsBase;

public class ErrorOnValidate : MyRecipeBookException
{
    public List<string> ErrorMessages { get; set; }

    public ErrorOnValidate(List<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}
