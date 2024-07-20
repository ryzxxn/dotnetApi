using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using dotnetApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MySqlDbContext>();
var app = builder.Build();

app.MapGet("/api/studentgetall", (MySqlDbContext db ) =>
{
    return StudentCrud.GetAllStudents(db);
});

app.MapPost("/api/studentcreate", async ([FromBody] Student student_details, MySqlDbContext db ) =>
{
    // Console.WriteLine(student_details.FirstName);
    return await StudentCrud.CreateStudent(student_details, db);
});

app.MapPost("/api/studentdelete", async ([FromBody] Student student_details, MySqlDbContext db ) =>
{
    // Console.WriteLine(student_details.FirstName);
    return await StudentCrud.DeleteStudent(student_details, db);
});

app.MapGet("/api/departments", () => {
    using var db = new MySqlDbContext();
    return DepartmentCrud.GetAllDepartments(db);
});

app.MapPost("/api/departmentcreate", async ([FromBody] Department department , MySqlDbContext db ) =>
{
    // Console.WriteLine(student_details.FirstName);
    return await DepartmentCrud.CreateDepartment(department, db);
});

app.MapPost("/api/departmentdelete", async ([FromBody] Department department, MySqlDbContext db ) =>
{
    // Console.WriteLine(student_details.FirstName);
    return await DepartmentCrud.DeleteDepartment(department.DepartmentID, db);
});

app.Run();