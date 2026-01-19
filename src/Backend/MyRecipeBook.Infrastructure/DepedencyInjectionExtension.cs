using System;
using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Domain.Scurity.AccessToken;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastructure.DataAcess;
using MyRecipeBook.Infrastructure.DataAcess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generetor;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using MyRecipeBook.Infrastructure.Services;

namespace MyRecipeBook.Infrastructure;

public static class DepedencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
    {
        AddLoggedUser(service);
        AddTokens(service, configuration);
        AddRepositories(service);

        if (configuration.IsUnitTestEnviroment()) return;

        AddDbContext(service, configuration);
        AddFluentMigrator(service, configuration);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {

        var serverVersion = new MySqlServerVersion(new Version(8, 0, 2));

        services.AddDbContext<MyRecipeBookDbContext>(options =>
        {
            options.UseMySql(configuration.ConnectionString(), serverVersion);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All();
        });
    }

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signinKey = configuration.GetValue<string>("Settings:Jwt:SigninKey");

        services.AddScoped<IAccessTokenGeneretor>(Options => new JwtTokenGenerator(expirationTimeMinutes, signinKey!));
        services.AddScoped<IAccessTokenValidator>(Options => new JwtTokenValidator(signinKey!));
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();
     
}
