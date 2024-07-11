using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dotnetApi;

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

app.Run();
