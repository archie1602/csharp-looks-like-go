using services;

internal static class TodoEndpoints
{
    public static void MapTodoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/todos");

        group.MapGet("/", (TodoService service) =>
            Results.Ok(service.GetAll()));

        group.MapPost("/", (CreateTodoRequest request, TodoService service) =>
        {
            var todo = service.Create(request.Title);
            return Results.Created($"/api/todos/{todo.Id}", todo);
        });
    }
}
