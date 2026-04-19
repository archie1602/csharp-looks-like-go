<p align="center">
  <img src="docs/banner.png" alt="csharp-looks-like-go" width="640">
</p>

# Minimal Web API

A small ASP.NET Core CRUD service built as a **file-based app** (no `.csproj`, no `.sln`), targeting .NET 11. The goal is to show how far modern C# can go in looking and feeling like a Go or Python web service, while still giving you EF Core, DI, and the full ASP.NET Core pipeline.

## Idea

File-based apps (new in .NET 10 / refined in .NET 11 preview 3) let you ship a real web app as a handful of `.cs` files with directives at the top:

```csharp
#:sdk Microsoft.NET.Sdk.Web
#:package Npgsql.EntityFrameworkCore.PostgreSQL@10.0.1
#:include domain/user.cs
```

No XML, no project metadata files, no ceremony. You can run it with `dotnet run main.cs`, publish it with `dotnet publish main.cs`, and `chmod +x main.cs` to execute it like a script. This repo is a proof-of-concept that a layered CRUD service (entities, DTOs, EF Core, migrations, OpenAPI, middleware) fits comfortably inside that model.

## Stack

- .NET 11 (preview 3): `#:sdk`, `#:package`, `#:include`, `#:property` directives
- ASP.NET Core Minimal APIs
- EF Core 10 + Npgsql (PostgreSQL)
- Built-in OpenAPI (`Microsoft.AspNetCore.OpenApi`)
- JetBrains.Annotations for DI-flow hints

## Structure

```text
Client -> route -> handler -> service -> repository -> db -> PostgreSQL
```

```text
config/       app settings + JSON source-gen context
domain/       entities (User, Post)
model/        request and error DTOs
db/           EF Core DbContext and entity configurations
repository/   data access (async EF calls)
service/      business logic and validation
handler/      HTTP handlers and exception handler
middleware/   request pipeline middleware
migrations/   EF Core migrations
main.cs       entry point (MSBuild properties, includes, route table)
packages.cs   NuGet pins and global usings
includes.cs   central file list (transitive from main.cs)
globals.cs    global using directives
```

## Conventions

The codebase intentionally leans toward Go/Python aesthetics:

- **snake_case file names** (`user_handler.cs`, `post_repository.cs`): matches Go/Python, easy to grep.
- **lowercase, singular folder names** (`handler`, not `Handlers`): Go stdlib style (`net/http`, `encoding/json`).
- **Lowercase, singular namespaces** (`handler`, `service`, `repository`): follows the folder convention; namespaces read as Go packages (`handler.UserHandler`).
- **No `Async` suffix on methods**. Every method here is async, so the suffix carries no information. The Framework Design Guidelines still recommend the suffix for *library* APIs; this is an app, so it's dropped for readability.
- **Primary constructors everywhere**: `class UserService(UserRepository repo)` replaces the old Java-style ceremony.
- **Layered architecture, not vertical slices**. Each concern (handler/service/repository) has its own folder. Simple enough to navigate, teaches the pipeline explicitly.
- **`required` properties on entities**: compile-time enforcement replaces `= null!;` hacks. Nav props stay as `= [];` / `= null!;` per EF convention.
- **Central JSON source-gen context** (`config/json_context.cs`): AOT-friendly, one place to register all serialized types.
- **Central exception handling** via `IExceptionHandler` + `ProblemDetails`. Handlers don't try/catch; `ArgumentException` / `InvalidOperationException` from services become `400` automatically.
- **Flat `#:include` manifest**. All file includes live in `includes.cs`, referenced once from `main.cs`. `main.cs` stays a short preamble (properties, packages, includes, entry point).

## Endpoints

```http
# users
GET    /api/users/
GET    /api/users/{id}
POST   /api/users/
PUT    /api/users/{id}
PATCH  /api/users/{id}
DELETE /api/users/{id}

# posts (nested under user)
GET    /api/users/{userId}/posts/
GET    /api/users/{userId}/posts/{id}
POST   /api/users/{userId}/posts/
PUT    /api/users/{userId}/posts/{id}
PATCH  /api/users/{userId}/posts/{id}
DELETE /api/users/{userId}/posts/{id}

# openapi
GET    /openapi/v1.json
```

## Run

```bash
dotnet run main.cs
```

Or, with the shebang and execute permission:

```bash
chmod +x main.cs
./main.cs
```

## Publish as a single native binary

**File-based apps publish with Native AOT by default.** One command, one native executable. Same distribution story Go gives you with `go build`.

```bash
dotnet publish main.cs -o bin
```

Output: a **single native binary** at `bin/main`, around 33 MB out of the box.

For a leaner binary, pass a few size-oriented flags:

```bash
dotnet publish main.cs -o bin \
  -p:OptimizationPreference=Size \
  -p:InvariantGlobalization=true \
  -p:DebuggerSupport=false \
  -p:EventSourceSupport=false \
  -p:HttpActivityPropagationSupport=false \
  -p:StripSymbols=true
```

### The result: **~30 MB. One file. Zero dependencies.**

| Out of the box | Size-optimized |
| -------------- | -------------- |
| **~33 MB**     | **~30 MB**     |

That's a full ASP.NET Core web service with EF Core, PostgreSQL driver, validation, OpenAPI, and a PATCH API, in **one file** weighing roughly the same as a modern Electron app's splash screen.

### What you **don't** need on the target machine

- No .NET SDK
- No .NET runtime
- No ASP.NET Core shared framework
- No `dotnet` CLI
- No container, no `glibc` shim, no sidecar

Just the binary. `scp bin/main user@server:/opt/app/main`, `./main`, done. Exactly like shipping a Go binary, except you're writing C#.

```bash
./bin/main
```

## Migrations

Migrations are checked in under `migrations/` (snake_case filenames matching the file-naming convention).

As of .NET 11 preview 3, `dotnet ef` still expects an SDK-style `.csproj` and can't target a file-based app directly. To add a migration:

1. Create a temporary `.tooling/tooling.csproj` that globs the entity + DbContext + migration files from the repo via `<Compile Include="..\domain\*.cs" />` etc., and references `Microsoft.EntityFrameworkCore.Design` + the Postgres provider.
2. Add a small `IDesignTimeDbContextFactory<AppDbContext>` in the tooling folder.
3. Run `dotnet ef migrations add <name> --output-dir ../migrations --namespace minimal_web_api.migrations` from inside `.tooling/`.
4. Rename the generated `<name>.Designer.cs` to `<name>.designer.cs` to match the file-naming convention.
5. Delete `.tooling/`.

This is the one remaining rough edge of the file-based-app model and is tracked upstream.
