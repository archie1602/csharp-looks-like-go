#!/usr/bin/env dotnet
#:sdk Microsoft.NET.Sdk.Web
#:package Npgsql.EntityFrameworkCore.PostgreSQL

#:include config/config.cs
#:include config/json_context.cs
#:include domain/user.cs
#:include model/user_models.cs
#:include db/app_db_context.cs
#:include db/user_config.cs
#:include repository/user_repository.cs
#:include service/user_service.cs
#:include handler/user_handler.cs

using model;
using config;
using db;
using domain;
using handler;
using Microsoft.EntityFrameworkCore;
using service;
using repository;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddAppSettings();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));
builder.Services.AddValidation();
builder.Services.ConfigureHttpJsonOptions(opt => opt.SerializerOptions.TypeInfoResolverChain.Add(AppJsonSerializerContext.Default));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserHandler>();

var app = builder.Build();

var users = app.MapGroup("/api/users");

users.MapGet(   "/",          (UserHandler handler)                                      => handler.ListUsers());
users.MapGet(   "/{id:long}", (long id, UserHandler handler)                             => handler.GetUser(id));
users.MapPost(  "/",          (CreateUserRequest request, UserHandler handler)           => handler.CreateUser(request));
users.MapPut(   "/{id:long}", (long id, UpdateUserRequest request, UserHandler handler)  => handler.UpdateUser(id, request));
users.MapPatch( "/{id:long}", (long id, PatchUserRequest request, UserHandler handler)   => handler.PatchUser(id, request));
users.MapDelete("/{id:long}", (long id, UserHandler handler)                             => handler.DeleteUser(id));

app.Run();
