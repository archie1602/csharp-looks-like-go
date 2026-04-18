#!/usr/bin/env dotnet
#:sdk Microsoft.NET.Sdk.Web
#:property TargetFramework=net11.0
#:property LangVersion=preview

#:property ExperimentalFileBasedProgramEnableIncludeDirective=true
#:property ExperimentalFileBasedProgramEnableTransitiveDirectives=true

#:include models/todo_item.cs
#:include contracts/todo_contracts.cs
#:include services/todo_service.cs
#:include endpoints/todo_endpoints.cs

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<TodoService>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new { message = "Todo API is running" }));
app.MapTodoEndpoints();

app.Run();