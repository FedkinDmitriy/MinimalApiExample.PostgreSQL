using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiExample.PostgreSQL.Data;
using MinimalApiExample.PostgreSQL.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// ��������� ������� Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyContext>(o => o.UseNpgsql("Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=251187"));

var app = builder.Build();

// ��������� Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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

app.MapGet("/users", async Task<Ok<List<User>>> (MyContext dbContext) =>
{
    return TypedResults.Ok(await dbContext.Users.AsNoTracking().ToListAsync()); // ����� �������� ���� List<User>
});

app.MapGet("/users/{id}", async Task<Results<Ok<User>, NotFound>> (MyContext dbContext, int id) =>
{
    var user = await dbContext.Users.FindAsync(id);
    return user is null ? TypedResults.NotFound() : TypedResults.Ok(user);
});

app.MapPost("/users/add", async Task<Results<Created<User>, BadRequest>> (MyContext dbContext, [FromBody] User user) =>
{
    if (user is not null)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return TypedResults.Created("/users", user);
    }
    return TypedResults.BadRequest();
});

app.MapPut("/users/upd", async Task<Results<NoContent, BadRequest, NotFound>> (MyContext dbContext, [FromBody] User updatedUser) =>
{
    if (updatedUser is null) return TypedResults.BadRequest();

    var existingUser = await dbContext.Users.FindAsync(updatedUser.Id);

    if (existingUser is null) return TypedResults.NotFound();

    dbContext.Entry(existingUser).CurrentValues.SetValues(updatedUser);

    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
});

app.Run();
