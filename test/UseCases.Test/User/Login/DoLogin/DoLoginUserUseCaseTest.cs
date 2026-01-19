using System;
using CommonTestUtilities.Crypt;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Application.UseCase.User.Login.doLogin;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.Login.DoLogin;

public class DoLoginUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(new RequestDoLoginUserJson()
        {
            Email = user.Email,
            Password = password
        });

        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Tokens.ShouldNotBeNull();
        result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_Email_or_Password_Incorrect()
    {
        var request = RequestDoLoginUserJsonBuilder.Build();

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        var exceptions = await act.ShouldThrowAsync<ErrorOnDoLogin>();
        exceptions.ErrorMessages.ShouldContain(erroMessage => erroMessage.Equals(ResourceExceptionsMessages.EMAIL_OR_PASSWORD_INVALID));
    }

    private DoLoginUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
    {
        var passwordEncripter = PasswordEncrypterBuilder.Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var AccessTokenGeneretor = new AccessTokenGeneretorBuilder();

        if (user is not null)
            readOnlyRepository.GetByEmailAndPassword(user);

        return new DoLoginUseCase(readOnlyRepository.Build(), passwordEncripter, AccessTokenGeneretor.Build());
    }
}
