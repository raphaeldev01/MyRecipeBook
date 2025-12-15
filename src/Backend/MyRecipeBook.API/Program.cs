using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers(); 


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

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();


app.Run();


