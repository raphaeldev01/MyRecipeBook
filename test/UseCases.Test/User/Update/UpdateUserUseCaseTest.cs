using System;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCase.User.Update;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = createUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();

        user.Name.ShouldBe(request.Name);
        user.Email.ShouldBe(request.Email);
    }

    [Fact]
    public async Task Error_Email_Invalid()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "raphael.com";

        var useCase = createUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        var exceptions = await act.ShouldThrowAsync<ErrorOnValidate>();
        exceptions.ShouldSatisfyAllConditions(
            e => e.ErrorMessages.ShouldHaveSingleItem(),
            e => e.ErrorMessages.ShouldContain(ResourceExceptionsMessages.INVALID_EMAIL)
        );
    }

    [Fact]
    public async Task Error_Email_Existis()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();

        var useCase = createUseCase(user, request.Email);

        Func<Task> act = async () => await useCase.Execute(request);

        var exceptions = await act.ShouldThrowAsync<ErrorOnValidate>();
        exceptions.ShouldSatisfyAllConditions(
            e => e.ErrorMessages.ShouldHaveSingleItem(),
            e => e.ErrorMessages.ShouldContain(ResourceExceptionsMessages.EMAIL_EXISTS)
        );
    }

    [Fact]
    public async Task Error_Email_Empty()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;

        var useCase = createUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        var exceptions = await act.ShouldThrowAsync<ErrorOnValidate>();
        exceptions.ShouldSatisfyAllConditions(
            e => e.ErrorMessages.ShouldHaveSingleItem(),
            e => e.ErrorMessages.ShouldContain(ResourceExceptionsMessages.EMAIL_EMPTY)
        );
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = createUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        var exceptions = await act.ShouldThrowAsync<ErrorOnValidate>();
        exceptions.ShouldSatisfyAllConditions(
            e => e.ErrorMessages.ShouldHaveSingleItem(),
            e => e.ErrorMessages.ShouldContain(ResourceExceptionsMessages.NAME_EMPTY)
        );
    }

    private UpdateUserUseCase createUseCase(MyRecipeBook.Domain.Entities.User user, string? email = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var readOnly = new UserReadOnlyRepositoryBuilder();
        var updateOnly = new UserUpdateOnlyRepositoryBuilder().GetUserById(user).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if(string.IsNullOrEmpty(email) == false)
            readOnly.ExistActiveUsersWithEmail(email); 

        return new UpdateUserUseCase(readOnly.Build(), updateOnly, unitOfWork, loggedUser);
    }
}
