using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using dotnetApi;

var builder = WebApplication.CreateBuilder(args);

// Dependecy injection
builder.Services.AddDbContext<MySqlDbContext>();

var app = builder.Build();

// Create a new student in student
app.MapPost("/student/create", async (Student student, MySqlDbContext db) =>
{
    db.Student.Add(student);
    await db.SaveChangesAsync();
    return Results.Ok();
});

// Delete student from student 
app.MapDelete("/student/delete", async (Student student, MySqlDbContext db) =>
{
    var student = await db.Student.FindAsync(student.StudentID); 
    if (student == null)
    {
        return Results.NotFound("Student not found");
    }

    db.Student.Remove(student);
    await db.SaveChangesAsync();
    return Results.Ok();
}); 

app.Run();
