using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using dotnetApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MySqlDbContext>();
var app = builder.Build();

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
app.MapGet("/api/students", ( MySqlDbContext db ) =>
{
    return StudentCrud.GetAllStudents(db);
});

app.MapPost("/api/studentcreate", async ([FromBody] Student student_details, MySqlDbContext db ) =>
{
    return await StudentCrud.CreateStudent(student_details, db);
});

app.MapPut("/api/studentupdate", async ([FromBody] Student updatedStudent, MySqlDbContext db) =>
{
    return await StudentCrud.UpdateStudent(updatedStudent, db);
});

app.MapDelete("/api/studentdelete", async ([FromBody] int studentID, MySqlDbContext db) =>
{
    Console.WriteLine(studentID);
    return await StudentCrud.DeleteStudent(studentID, db);
});
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
app.MapGet("/api/departments", (MySqlDbContext db) => 
{
    return DepartmentCrud.GetAllDepartments(db);
});

app.MapPost("/api/departmentcreate", async ([FromBody] Department department , MySqlDbContext db ) =>
{
    // Console.WriteLine(student_details.FirstName);
    return await DepartmentCrud.CreateDepartment(department, db);
});

app.MapDelete("/api/departmentdelete/{departmentID}", async (int departmentID, MySqlDbContext db) =>
{
    return await DepartmentCrud.DeleteDepartment(departmentID, db);
});

app.MapPost("/api/departmentupdate", async ([FromBody] Department updatedDepartment, MySqlDbContext db) =>
{
    return await DepartmentCrud.UpdateDepartment(updatedDepartment, db);
});
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
app.MapGet("/api/courses", (MySqlDbContext db) =>
{
    return CourseCrud.GetAllCourses(db);
});

app.MapPost("/api/coursecreate", async ([FromBody] Course course, MySqlDbContext db) =>
{
    return await CourseCrud.CreateCourse(course, db);
});

app.MapPut("/api/courseupdate", async ([FromBody] Course updatedCourse, MySqlDbContext db) =>
{
    return await CourseCrud.UpdateCourse(updatedCourse, db);
});

app.MapDelete("/api/coursedelete/{courseID}", async (int courseID, MySqlDbContext db) =>
{
    return await CourseCrud.DeleteCourse(courseID, db);
});
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
app.MapGet("/api/instructors", () =>
{
    using var db = new MySqlDbContext();
    return InstructorCrud.GetAllInstructors(db);
});

app.MapPost("/api/instructorcreate", async ([FromBody] Instructor instructor, MySqlDbContext db) =>
{
    return await InstructorCrud.CreateInstructor(instructor, db);
});

app.MapPut("/api/instructorupdate", async ([FromBody] Instructor updatedInstructor, MySqlDbContext db) =>
{
    return await InstructorCrud.UpdateInstructor(updatedInstructor, db);
});

app.MapDelete("/api/instructordelete/{instructorID}", async (int instructorID, MySqlDbContext db) =>
{
    return await InstructorCrud.DeleteInstructor(instructorID, db);
});

app.MapGet("/api/instructor/{instructorID}", async (int instructorID, MySqlDbContext db) =>
{
    return await InstructorCrud.GetInstructorById(instructorID, db);
});
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
app.Run();