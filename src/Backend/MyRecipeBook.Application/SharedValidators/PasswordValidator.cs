using System;
using FluentValidation;
using FluentValidation.Validators;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.SharedValidators;

public class PasswordValidator<T> : PropertyValidator<T, string>
{

    public override bool IsValid(ValidationContext<T> context, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceExceptionsMessages.SHORT_PASSWORD);
            return false;
        }
        if (value.Length < 6)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceExceptionsMessages.SHORT_PASSWORD);
            return false;

        }

        return true;
    }

    public override string Name => "PasswordValidator";

    protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";

}
