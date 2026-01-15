using Microsoft.AspNetCore.OpenApi;
using MyRecipeBook.API.Filters;
using MyRecipeBook.Application;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Infrastructure.DataAcess.Migrations;
using MyRecipeBook.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers(); 

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionsFilter)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Gera o documento JSON no endpoint /openapi/v1.json
    app.MapOpenApi();

    // Configura a Interface Visual do Swagger
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "MyRecipeBook API .NET 10");
    });
}

// Console.WriteLine("teste");

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

MigrateDatabase();


app.Run();

void MigrateDatabase ()
{
    var connectionString = builder.Configuration.ConnectionString();

    var serviceScoped = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DatabaseMigrations.Migrate(connectionString, serviceScoped.ServiceProvider);
}