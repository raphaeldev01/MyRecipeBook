using System;
using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestDoLoginUserJsonBuilder
{
    public static RequestDoLoginUserJson Build ()
    {
        return new Faker<RequestDoLoginUserJson>()
            .RuleFor(user => user.Email, (f) => f.Internet.Email())
            .RuleFor(user => user.Password, (f) => f.Internet.Password()); 
    }
}
