# Minimal Web API

Minimal file-based ASP.NET Core CRUD API with layered structure and PostgreSQL via EF Core.

## Structure

```text
Client -> main routes -> handler -> service -> repository -> db -> PostgreSQL
```

```text
config/      app settings
model/       request and error DTOs
db/          EF Core DbContext
domain/      entities
handler/     HTTP handlers
repository/  data access
service/     business logic
migrations/  EF Core migrations
```

## Run

```bash
rtk dotnet run main.cs
```

## Endpoints

```http
GET    /api/users/
GET    /api/users/{id}
POST   /api/users/
PUT    /api/users/{id}
PATCH  /api/users/{id}
DELETE /api/users/{id}
```

## Migrations

The migration files are checked in under `migrations/`. EF tooling currently needs an SDK-style `.csproj`; this project keeps `main.cs` as the runtime entrypoint, so create a temporary tooling project if you need to add another migration.
