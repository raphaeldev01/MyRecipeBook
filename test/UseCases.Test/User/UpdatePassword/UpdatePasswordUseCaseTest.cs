using System;
using CommonTestUtilities.Crypt;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCase.User.UpdatePassword;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.UpdatePassword;

public class UpdatePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, password) = UserBuilder.Build();
        var request = RequestUpdatePasswordJsonBuilder.Build();

        request.Password = password;

        var useCase = createUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Password_Incorrect()
    {
        var (user, password) = UserBuilder.Build();
        var request = RequestUpdatePasswordJsonBuilder.Build();

        var useCase = createUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        var Exceptions = await act.ShouldThrowAsync<ErrorOnValidate>();

        Exceptions.ErrorMessages.ShouldHaveSingleItem();
        Exceptions.ErrorMessages.ShouldContain(em => em.ToString().Equals(ResourceExceptionsMessages.PASSWORD_INCORRECT));
    }


    private UpdatePasswordUseCase createUseCase (MyRecipeBook.Domain.Entities.User user)
    {
        var passwordEncripter = PasswordEncrypterBuilder.Build();
        var userUpdateOnly = new UserUpdateOnlyRepositoryBuilder().GetUserById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new UpdatePasswordUseCase(passwordEncripter, userUpdateOnly, loggedUser, unitOfWork);
    }
}
