#!/usr/bin/env dotnet
#:sdk Microsoft.NET.Sdk.Web
#:package Humanizer
#:property TargetFramework=net11.0
#:property LangVersion=preview

#:property ExperimentalFileBasedProgramEnableIncludeDirective=true
#:property ExperimentalFileBasedProgramEnableTransitiveDirectives=true

#:include models/todo_item.cs
#:include contracts/todo_contracts.cs
#:include services/todo_service.cs
#:include endpoints/todo_endpoints.cs

using System.Text.Json.Serialization;
using services;
using models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<TodoService>();
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.TypeInfoResolverChain.Add(AppJsonSerializerContext.Default));

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new ApiStatusResponse("Todo API is running")));
app.MapTodoEndpoints();

app.Run();

[JsonSerializable(typeof(ApiStatusResponse))]
[JsonSerializable(typeof(CreateTodoRequest))]
[JsonSerializable(typeof(IReadOnlyList<TodoItem>))]
[JsonSerializable(typeof(List<TodoItem>))]
[JsonSerializable(typeof(TodoItem))]
internal sealed partial class AppJsonSerializerContext : JsonSerializerContext;
