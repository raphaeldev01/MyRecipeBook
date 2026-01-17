using System;
using Bogus;
using CommonTestUtilities.Crypt;
using MyRecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static (User user, string password) Build()
    {
        var passwordEncripter = PasswordEncrypterBuilder.Build();

        var password = new Faker().Internet.Password();

        var user = new Faker<User>()
            .RuleFor(u => u.Email, (f) => f.Internet.Email())
            .RuleFor(u => u.Password, (f) => passwordEncripter.Encrypt(password));

        return (user, password);
    }
}
