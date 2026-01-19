using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Application.UseCase.User.GetProfile;
using MyRecipeBook.Application.UseCase.User.Login.doLogin;
using MyRecipeBook.Application.UseCase.User.Register;

namespace MyRecipeBook.Application;

public static class DepedencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection service, IConfiguration configuration)
    {
        AddUseCases(service);
        AddPasswordEncripyter(service, configuration);
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
    }

    private static void AddPasswordEncripyter(IServiceCollection services, IConfiguration configuration)
    {
        var additionalKey = configuration.GetSection("Settings:Password:additionalKey").Value;

        services.AddScoped(options => new PasswordEncripter(additionalKey!));
    } 
}
