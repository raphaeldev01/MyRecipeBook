using System;
using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestUpdatePasswordJsonBuilder
{
    public static RequestUpdatePasswordJson Build (int passwordLength = 10, int newPasswordLength = 10) => new Faker<RequestUpdatePasswordJson>()
        .RuleFor(req => req.Password, (f) => f.Internet.Password(passwordLength))
        .RuleFor(req => req.NewPassword, (f) => f.Internet.Password(newPasswordLength));
}
