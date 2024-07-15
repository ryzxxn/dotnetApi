using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using dotnetApi; // Import the namespace where MySqlDbContext is located

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MySqlDbContext>(); // Correct the DbContext type to match your class name

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapPost("/student/create", async (Student student, MySqlDbContext db) =>
{
    db.Student.Add(student);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();
