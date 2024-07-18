using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using dotnetApi;
using dotnetApi.Functions; // Adjust the namespace as per your project structure

var builder = WebApplication.CreateBuilder(args);

// Dependency injection
builder.Services.AddDbContext<MySqlDbContext>();
var app = builder.Build();

// GET all student data
app.MapGet("/student", (MySqlDbContext db) =>
{
    return StudentCrud.GetAllStudents(db);
});

// Create a new student row
app.MapPost("/student/create", async ([FromBody] Student student, MySqlDbContext db) =>
{
    return await StudentCrud.CreateStudent(student, db);
});

// Delete student using StudentID
app.MapPost("/student/delete", async ([FromBody] Student student, MySqlDbContext db) =>
{
    return await StudentCrud.DeleteStudent(student, db);
});

// Update Student Data using StudentID
app.MapPost("/student/update", async ([FromBody] Student updatedStudent, MySqlDbContext db) =>
{
    return await StudentCrud.UpdateStudent(updatedStudent, db);
});

app.MapGet("/{id}", async (int id, MySqlDbContext db) =>
{
    try
    {
        var studentDetails = await db.Students
            .Where(s => s.StudentID == id)
            .Select(s => new
            {
                s.StudentID,
                s.FirstName,
                s.LastName,
                Courses = s.StudentCourses?.Select(sc => new
                {
                    sc.Course?.CourseID,
                    sc.Course?.CourseName,
                    Department = new
                    {
                        sc.Course?.Department?.DepartmentID,
                        sc.Course?.Department?.DepartmentName
                    },
                    Instructor = new
                    {
                        sc.Course?.Instructor?.InstructorID,
                        sc.Course?.Instructor?.FirstName,
                        sc.Course?.Instructor?.LastName
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (studentDetails == null)
        {
            return Results.NotFound("Student not found");
        }

        return Results.Ok(studentDetails);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error fetching student details: {ex.Message}");
        return Results.BadRequest("Failed to fetch student details.");
    }
});


app.Run();

