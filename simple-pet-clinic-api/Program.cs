using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using simple_pet_clinic_api;
using simple_pet_clinic_api.Data;

var builder = RuntimeFeature.IsDynamicCodeSupported 
    ? WebApplication.CreateBuilder(args) 
    : WebApplication.CreateSlimBuilder(args);

builder.Services.AddOpenApi();
builder.AddApplication();

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();

app.MapGet("/", () => "API is running!");
app.MapGet("/db-check", async (AppDbContext db) => 
    await db.Database.CanConnectAsync() ? "Database Connected!" : "Connection Failed!");

await SeedDatabase();

app.Run();

async Task SeedDatabase()
{
    using (IServiceScope scope = app.Services.CreateScope())
    {
        IDbInitializer dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        
        await dbInitializer.InitializeAsync();
    }
}