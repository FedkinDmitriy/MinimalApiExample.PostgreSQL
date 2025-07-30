using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiExample.PostgreSQL.Data;
using MinimalApiExample.PostgreSQL.Data.Filters;
using MinimalApiExample.PostgreSQL.Data.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails(); //добавл€ет реализацию IProblemDetailsService

// ƒобавл€ем сервисы Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyContext>(o => o.UseNpgsql("Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=251187"));

var app = builder.Build(); // автоматически добавл€ет UseDeveloperExceptionPage() в режиме разработки


// Ќастройка Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error"); // обработка ошибок в промокружении без передачи данных о коде
}

app.MapGet("/error", () => "Sorry, an error occurred.");
app.UseStatusCodePages(); // Api возвращает ответ Problem Details дл€ всех ответов об ошибках ( builder.Services.AddProblemDetails();)





app.MapGet("/", () => "Hello World!");

app.MapGet("/check", async (MyContext db) =>
{
    try
    {
        var exists = await db.Database.CanConnectAsync();
        return exists ? "DB connected" : "DB not connected";
    }
    catch (Exception ex)
    {
        return $"Error: {ex.Message}";
    }
});

app.MapGet("/users", async Task<Results<Ok<List<User>>, NoContent>> (MyContext dbContext) =>
{
    var list = await dbContext.Users.AsNoTracking().ToListAsync();
    return list.Count > 0 ? TypedResults.Ok(list) : TypedResults.NoContent();
});

app.MapGet("/users/{id}", async Task<Results<Ok<User>, NotFound>> (MyContext dbContext, int id) =>
{
    var user = await dbContext.Users.FindAsync(id);
    return user is null ? TypedResults.NotFound() : TypedResults.Ok(user);
});

app.MapPost("/users", async (MyContext dbContext, User user) =>
{
    await dbContext.Users.AddAsync(user);
    await dbContext.SaveChangesAsync();
    return TypedResults.Created("/users", user);
})
.AddEndpointFilter<UserValidationFilter>();

app.MapPut("/users", async Task<Results<NoContent, BadRequest, NotFound>> (MyContext dbContext, [FromBody] User updatedUser) =>
{
    if (updatedUser is null) return TypedResults.BadRequest();

    var existingUser = await dbContext.Users.FindAsync(updatedUser.Id);

    if (existingUser is null) return TypedResults.NotFound();

    dbContext.Entry(existingUser).CurrentValues.SetValues(updatedUser);

    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
});

app.MapDelete("/users/{id}", async Task<Results<NoContent,NotFound>> (MyContext dbContext, int id) =>
{
    var user = await dbContext.Users.FindAsync(id);

    if(user is not null)
    {
        dbContext.Remove(user);
        await dbContext.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    else
        return TypedResults.NotFound();
} );

app.Run();
