using System;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using MyRecipeBook.Application.UseCase.User.GetProfile;
using Shouldly;

namespace UseCases.Test.User.GetProfile;

public class GetProfileUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var result = await createUseCase(user).Execute();

        result.Name.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Email.ShouldNotBeNull();
        result.Email.ShouldBe(user.Email);
    }


    private GetProfileUserUseCase createUseCase (MyRecipeBook.Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetProfileUserUseCase(mapper, loggedUser);
    }
}
