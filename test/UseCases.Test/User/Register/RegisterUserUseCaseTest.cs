using System;
using CommonTestUtilities.Crypt;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Application.UseCase.User.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using Shouldly;
using Xunit.Sdk;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var RegisterUserUseCase = CreateUseCase();

        var request = RequestRegisterUserJsonBuilder.Build();

        var result = await RegisterUserUseCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
    }

    [Fact]
    public async Task Error_Email_Alredy_Exists()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var RegisterUserUseCase = CreateUseCase(request.Email);
    
        Func<Task> act = async () => await RegisterUserUseCase.Execute(request);

        var exceptions = await act.ShouldThrowAsync<ErrorOnValidate>();
        exceptions.ShouldSatisfyAllConditions(
            e => e.ErrorMessages.ShouldHaveSingleItem(),
            e => e.ErrorMessages.ShouldContain(ResourceExceptionsMessages.EMAIL_EXISTS)
        );
    }
    
    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var RegisterUserUseCase = CreateUseCase();
    
        Func<Task> act = async () => await RegisterUserUseCase.Execute(request);

        var exceptions = await act.ShouldThrowAsync<ErrorOnValidate>();
        exceptions.ShouldSatisfyAllConditions(
            e => e.ErrorMessages.ShouldHaveSingleItem(),
            e => e.ErrorMessages.ShouldContain(ResourceExceptionsMessages.NAME_EMPTY)
        );
    }

    [Fact]
    public async Task Error_Email_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var RegisterUserUseCase = CreateUseCase();
    
        Func<Task> act = async () => await RegisterUserUseCase.Execute(request);

        var exceptions = await act.ShouldThrowAsync<ErrorOnValidate>();
        exceptions.ShouldSatisfyAllConditions(
            e => e.ErrorMessages.ShouldHaveSingleItem(),
            e => e.ErrorMessages.ShouldContain(ResourceExceptionsMessages.EMAIL_EMPTY)
        );
    }

    [Fact]
    public async Task Error_Email_Invalid()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "Raphael.com";

        var RegisterUserUseCase = CreateUseCase();
    
        Func<Task> act = async () => await RegisterUserUseCase.Execute(request);

        var exceptions = await act.ShouldThrowAsync<ErrorOnValidate>();
        exceptions.ShouldSatisfyAllConditions(
            e => e.ErrorMessages.ShouldHaveSingleItem(),
            e => e.ErrorMessages.ShouldContain(ResourceExceptionsMessages.INVALID_EMAIL)
        );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Error_Password_Invalid(int passwordLength)
    {
        var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

        var RegisterUserUseCase = CreateUseCase();
    
        Func<Task> act = async () => await RegisterUserUseCase.Execute(request);

        var exceptions = await act.ShouldThrowAsync<ErrorOnValidate>();
        exceptions.ShouldSatisfyAllConditions(
            e => e.ErrorMessages.ShouldHaveSingleItem(),
            e => e.ErrorMessages.ShouldContain(ResourceExceptionsMessages.SHORT_PASSWORD)
        );
    }

    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncrypterBuilder.Build();

        if(string.IsNullOrEmpty(email) == false)
            readOnlyRepository.ExistActiveUsersWithEmail(email);

        return new RegisterUserUseCase(readOnlyRepository.Build(), writeOnlyRepository, unitOfWork, mapper, passwordEncripter);
    }
}
