using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalApiExample.PostgreSQL.Data;
using MinimalApiExample.PostgreSQL.Data.DTOs;
using MinimalApiExample.PostgreSQL.Data.Filters;
using MinimalApiExample.PostgreSQL.Data.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails(); //добавляет реализацию IProblemDetailsService

// Добавляем сервисы Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyContext>(o => o.UseNpgsql("Host=localhost;Port=5432;Database=TestDb;Username=postgres;Password=251187"));

var app = builder.Build(); // автоматически добавляет UseDeveloperExceptionPage() в режиме разработки


// Настройка Swagger middleware
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
app.UseStatusCodePages(); // Api возвращает ответ Problem Details для всех ответов об ошибках ( builder.Services.AddProblemDetails();)





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

app.MapGet("/users", async Task<Results<Ok<List<UserResponseDTO>>, NoContent>> (MyContext dbContext) =>
{
    var response = await dbContext.Users.AsNoTracking().Select(u => new UserResponseDTO() { 
        Id = u.Id,
        FirstName = u.firstName,
        LastName = u.lastName,
        DateOfBirth = u.dateOfBirth,
        HasBlogs = u.Blogs.Any()
    }).ToListAsync();
    return response.Count > 0 ? TypedResults.Ok(response) : TypedResults.NoContent();
});

app.MapGet("/users/{id}", async Task<Results<Ok<UserResponseDTOWithBlogs>, NotFound>> (MyContext dbContext, int id) =>
{
    //без DTO будет цикличная сериализация
    var dto = await dbContext.Users.Where(u => u.Id == id)
    .Select(u => new UserResponseDTOWithBlogs
    {
        Id = u.Id,
        FirstName = u.firstName,
        LastName = u.lastName,
        DateOfBirth = u.dateOfBirth,
        Blogs = u.Blogs
        .Select(b => new BlogResponseDTO
        {
            Id = b.Id,
            Title = b.Title,
            CreatedDate = b.CreatedDate,
            Context = b.Context
        }).ToList()
    })
    .AsNoTracking()
    .FirstOrDefaultAsync();

    return dto is not null ? TypedResults.Ok(dto) : TypedResults.NotFound();



    //вариант с Include (медленнее)

    //var user = await dbContext.Users.AsNoTracking().Include(u => u.Blogs).FirstOrDefaultAsync(u => u.Id == id);
    //if (user is not null)
    //{
    //    UserResponseDTOWithBlogs dto = new() 
    //    {
    //        Id = user.Id,
    //        FirstName = user.firstName,
    //        LastName = user.lastName,
    //        DateOfBirth = user.dateOfBirth,
    //        Blogs = user.Blogs.Select(b => new BlogResponseDTO() { Id = b.Id,
    //            Title = b.Title,
    //            CreatedDate = b.CreatedDate,
    //            Context = b.Context }).ToList()
    //    };
    //    return TypedResults.Ok(dto);
    //}
    //return TypedResults.NotFound();

}).AddEndpointFilter<IdValidationFilter>();

app.MapPost("/users", async Task<Results<Created<UserCreateDTO>, BadRequest<string>>>(MyContext dbContext, UserCreateDTO user) =>
{
    try
    {
        User newUser = new()
        {
            firstName = user.firstName,
            lastName = user.lastName,
            dateOfBirth = user.dateOfBirth
        };

        await dbContext.Users.AddAsync(newUser);
        await dbContext.SaveChangesAsync();
        return TypedResults.Created($"/users/{newUser.Id}", user);
    }
    catch (Exception ex)
    {
        return TypedResults.BadRequest(ex.Message);
    }
})
.AddEndpointFilter<UserValidationFilter>();

app.MapPut("/users/{id}", async Task<Results<NoContent, BadRequest<string>, NotFound>> (MyContext dbContext, [FromBody] UserCreateDTO updatedUser, int id) =>
{
    try
    {
        var existingUser = await dbContext.Users.FindAsync(id);
        if (existingUser is null) return TypedResults.NotFound();

        //dbContext.Entry(existingUser).CurrentValues.SetValues(updatedUser); //может стереть Blogs в модели User

        existingUser.firstName = updatedUser.firstName;
        existingUser.lastName = updatedUser.lastName;
        existingUser.dateOfBirth = updatedUser.dateOfBirth;

        await dbContext.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    catch (Exception ex)
    {
        return TypedResults.BadRequest(ex.Message);
    }
}).AddEndpointFilter<UserValidationFilter>();

app.MapDelete("/users/{id}", async Task<Results<NoContent, NotFound, BadRequest<string>>> (MyContext dbContext, int id) =>
{
    var user = await dbContext.Users.FindAsync(id);

    if (user is not null)
    {
        try
        {
            dbContext.Remove(user);
            await dbContext.SaveChangesAsync();
            return TypedResults.NoContent();
        }
        catch (DbUpdateException ex)
        {
            return TypedResults.BadRequest($"Ошибка базы данных {ex.Message}");
        }
    }
    else
    {
        return TypedResults.NotFound();
    }
}).AddEndpointFilter<IdValidationFilter>();

app.MapGet("/blogs/{id}", async Task<Results<Ok<BlogResponseDTO>, NotFound, BadRequest<string>>> (MyContext dbContext, int id) =>
{
    try
    {
        var blog = await dbContext.Blogs.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        if (blog is not null)
        {
            BlogResponseDTO dto = new()
            {
                Id = blog.Id,
                Title = blog.Title,
                CreatedDate = blog.CreatedDate,
                Context = blog.Context
            };
            return TypedResults.Ok(dto);
        }
        return TypedResults.NotFound();
    }
    catch (Exception ex)
    {
        return TypedResults.BadRequest(ex.Message);
    }
}).AddEndpointFilter<IdValidationFilter>();

app.MapPost("/blogs/{id}", async Task<Results<Created<BlogResponseDTO>, BadRequest<string>>> (MyContext dbContext, int id, BlogCreateDTO blogDTO) =>
{
    if (await dbContext.Users.AnyAsync(u => u.Id == id))
    {
        Blog blog = new()
        {
            Title = blogDTO.Title,
            CreatedDate = blogDTO.CreatedDate,
            Context = blogDTO.Context,
            UserId = id
        };
        try
        {
            await dbContext.Blogs.AddAsync(blog);
            await dbContext.SaveChangesAsync();

            BlogResponseDTO resp = new()
            {
                Id = blog.Id,
                Title = blog.Title,
                CreatedDate = blog.CreatedDate,
                Context = blog.Context
            };

            return TypedResults.Created($"/blogs/{resp.Id}", resp);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
    else
        return TypedResults.BadRequest("Нет такого пользователя");
}).AddEndpointFilter<IdValidationFilter>();

app.MapDelete("/blogs/{id}", async Task<Results<NoContent, NotFound, BadRequest<string>>>(MyContext dbContext, int id) => { 

    var blog = await dbContext.Blogs.FindAsync(id);

    if(blog is not null)
    {
        try
        {
            dbContext.Remove(blog);
            await dbContext.SaveChangesAsync();
            return TypedResults.NoContent();
        }
        catch (DbUpdateException ex)
        {
            return TypedResults.BadRequest($"Ошибка базы данных {ex.Message}");
        }
    }
    else
    {
        return TypedResults.NotFound();
    }
}).AddEndpointFilter<IdValidationFilter>();

app.Run();
