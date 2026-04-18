#:property TargetFramework=net11.0
#:property LangVersion=preview
#:property ExperimentalFileBasedProgramEnableIncludeDirective=true
#:property ExperimentalFileBasedProgramEnableTransitiveDirectives=true

#:include packages.cs
#:include includes.cs

using model;
using config;
using db;
using handler;
using middleware;
using service;
using repository;
using Microsoft.EntityFrameworkCore;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddAppSettings();
        var connectionString = builder.Configuration.GetPostgresConnectionString();

        // dependency injection things
        builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));
        builder.Services.AddValidation();
        builder.Services.ConfigureHttpJsonOptions(opt => opt.SerializerOptions.TypeInfoResolverChain.Add(AppJsonSerializerContext.Default));

        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<UserHandler>();

        builder.Services.AddScoped<PostRepository>();
        builder.Services.AddScoped<PostService>();
        builder.Services.AddScoped<PostHandler>();

        builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // middleware things
        app.UseExceptionHandler();
        app.UseMiddleware<RequestLogger>();
        app.MapOpenApi();

        // endpoint things
        var users = app.MapGroup("/api/users").WithTags("users");

        users.MapGet(   "/",          (UserHandler handler)                                      => handler.ListUsers());
        users.MapGet(   "/{id:long}", (long id, UserHandler handler)                             => handler.GetUser(id));
        users.MapPost(  "/",          (CreateUserRequest request, UserHandler handler)           => handler.CreateUser(request));
        users.MapPut(   "/{id:long}", (long id, UpdateUserRequest request, UserHandler handler)  => handler.UpdateUser(id, request));
        users.MapPatch( "/{id:long}", (long id, PatchUserRequest request, UserHandler handler)   => handler.PatchUser(id, request));
        users.MapDelete("/{id:long}", (long id, UserHandler handler)                             => handler.DeleteUser(id));

        var posts = app.MapGroup("/api/users/{userId:long}/posts").WithTags("posts");

        posts.MapGet(   "/",          (long userId, PostHandler handler)                                       => handler.ListPosts(userId));
        posts.MapGet(   "/{id:long}", (long userId, long id, PostHandler handler)                              => handler.GetPost(userId, id));
        posts.MapPost(  "/",          (long userId, CreatePostRequest request, PostHandler handler)            => handler.CreatePost(userId, request));
        posts.MapPut(   "/{id:long}", (long userId, long id, UpdatePostRequest request, PostHandler handler)   => handler.UpdatePost(userId, id, request));
        posts.MapPatch( "/{id:long}", (long userId, long id, PatchPostRequest request, PostHandler handler)    => handler.PatchPost(userId, id, request));
        posts.MapDelete("/{id:long}", (long userId, long id, PostHandler handler)                              => handler.DeletePost(userId, id));

        // let's start our C#-go-frankenstein :D
        await app.RunAsync();
    }
}
