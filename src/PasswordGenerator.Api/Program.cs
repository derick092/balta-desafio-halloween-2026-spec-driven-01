using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Passwords")
    ?? "Data Source=passwords.db";
builder.Services.AddDbContext<PasswordDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddSingleton<StrongPasswordGenerator>();
builder.Services.AddScoped<PasswordApplicationService>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var database = scope.ServiceProvider.GetRequiredService<PasswordDbContext>();
    await database.Database.EnsureCreatedAsync();
}

app.MapPost("/passwords", async (
    PasswordApplicationService passwords,
    CancellationToken cancellationToken) =>
{
    var id = await passwords.GenerateAsync(cancellationToken);
    return Results.Created($"/passwords/{id}", id);
});

app.MapGet("/passwords/{id}", async Task<IResult> (
    string id,
    PasswordApplicationService passwords,
    CancellationToken cancellationToken) =>
{
    if (!Guid.TryParse(id, out var passwordId))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["id"] = ["The id must be a valid GUID."]
        });
    }

    var value = await passwords.FindAsync(passwordId, cancellationToken);
    return value is null
        ? Results.Problem(statusCode: StatusCodes.Status404NotFound, title: "Password not found.")
        : Results.Ok(value);
});

app.Run();

public partial class Program;
