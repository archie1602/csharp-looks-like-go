#!/usr/bin/env dotnet run

#:sdk Microsoft.NET.Sdk.Web
#:package Npgsql.EntityFrameworkCore.PostgreSQL
#:package Microsoft.AspNetCore.OpenApi

#:include config/config.cs
#:include config/json_context.cs
#:include domain/user.cs
#:include model/user_models.cs
#:include db/app_db_context.cs
#:include db/user_config.cs
#:include repository/user_repository.cs
#:include service/user_service.cs
#:include handler/user_handler.cs
#:include handler/exception_handler.cs

using model;
using config;
using db;
using handler;
using Microsoft.EntityFrameworkCore;
using service;
using repository;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddAppSettings();
var connectionString = builder.Configuration.GetPostgresConnectionString();

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));
builder.Services.AddValidation();
builder.Services.ConfigureHttpJsonOptions(opt => opt.SerializerOptions.TypeInfoResolverChain.Add(AppJsonSerializerContext.Default));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserHandler>();

builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();
app.MapOpenApi();

var users = app.MapGroup("/api/users").WithTags("users");

users.MapGet(   "/",          (UserHandler handler)                                      => handler.ListUsersAsync());
users.MapGet(   "/{id:long}", (long id, UserHandler handler)                             => handler.GetUserAsync(id));
users.MapPost(  "/",          (CreateUserRequest request, UserHandler handler)           => handler.CreateUserAsync(request));
users.MapPut(   "/{id:long}", (long id, UpdateUserRequest request, UserHandler handler)  => handler.UpdateUserAsync(id, request));
users.MapPatch( "/{id:long}", (long id, PatchUserRequest request, UserHandler handler)   => handler.PatchUserAsync(id, request));
users.MapDelete("/{id:long}", (long id, UserHandler handler)                             => handler.DeleteUserAsync(id));

await app.RunAsync();
