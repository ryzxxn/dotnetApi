using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dotnetApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
var connectionString = configuration.GetConnectionString("DefaultConnection");

// Create an instance of DataBaseConnect and pass the connection string to the constructor
var db = new DataBaseConnect(connectionString);

// Create an instance of CRUDStudents and pass the DataBaseConnect instance to the constructor
var crudStudents = new CRUDStudents(db);

var app = WebApplication.Create(args);

app.MapGet("/{name}", (string name) => GreetFunction.greet(name));

app.MapGet("/sum", ([FromQuery] int num1, [FromQuery] int num2) =>
{
    int sum = GreetFunction.sum(num1, num2);
    return sum;
});

app.MapPost("/student", ([FromBody] Person request) =>
{
    return $"name: {request.Name}, age: {request.Age}, phone: {request.PhoneNumber}";
});

app.MapGet("/getStudents", () => crudStudents.GetAllStudents());

app.Run();
