using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.UseCase.User.GetProfile;
using MyRecipeBook.Application.UseCase.User.Login.doLogin;
using MyRecipeBook.Application.UseCase.User.Register;
using MyRecipeBook.Application.UseCase.User.Update;
using MyRecipeBook.Application.UseCase.User.UpdatePassword;

namespace MyRecipeBook.Application;

public static class DepedencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection service, IConfiguration configuration)
    {
        AddUseCases(service);
        AddMapper(service);
    }

    private static void AddMapper(IServiceCollection services)
    {
        services.AddScoped(options => new AutoMapper.MapperConfiguration(opt => opt.AddProfile(new AutoMap())).CreateMapper());
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetProfileUserUseCase, GetProfileUserUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IUpdatePasswordUseCase, UpdatePasswordUseCase>();
    }
}
