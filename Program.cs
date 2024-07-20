using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;

var app = WebApplication.Create(args);

app.MapGet("/", () => "Hello World!");

app.Run();