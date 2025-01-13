using CatImageApi.API.Extensions;
using CatImageApi.Application.DTOs;
using CatImageApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddHttpClients(builder.Configuration);
builder.Services.RegisterDependencies();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type =>
    {
        if (type == typeof(CatAdditionResponseDTO)) return "Fetch Cats";
        if (type == typeof(CatResponseDTO)) return "Cat Info";
        if (type == typeof(PaginatedCatResponseDTO<CatResponseDTO>)) return "Cats (paginated)";
        return type.Name; 
    });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>(); 
    try
    {
        dbContext.Database.Migrate(); // Apply migrations
        Console.WriteLine("Database migration applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
        throw;
    }
}

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
