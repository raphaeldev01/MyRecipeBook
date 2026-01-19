using System;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Infrastructure.DataAcess;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private  MyRecipeBook.Domain.Entities.User _user = default!;
    private  string _password = string.Empty;

    public string GetEmail () => _user.Email;
    public string GetPassword () => _password;
    public string GetName () => _user.Name;
    public Guid GetGuid () => _user.UserIdentifyer;


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MyRecipeBookDbContext>));

                if(descriptor is not null) 
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<MyRecipeBookDbContext>(option =>
                {
                    option.UseInMemoryDatabase("InMemoryDbForTesting");
                    option.UseInternalServiceProvider(provider);
                });

                var scope =  services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();

                dbContext.Database.EnsureDeleted();

                startDatabase(dbContext);
            });
    }

    private void startDatabase(MyRecipeBookDbContext dbContext)
    {
        (_user, _password) = UserBuilder.Build();

        dbContext.Users.Add(_user);
        dbContext.SaveChanges();
    }
}
