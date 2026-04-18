using model;
using service;

namespace handler;

class UserHandler(UserService service)
{
    public IResult ListUsers() => Results.Ok(service.List());

    public IResult GetUser(long id)
    {
        var user = service.Get(id);
        return user is null
            ? Results.NotFound()
            : Results.Ok(user);
    }

    public IResult CreateUser(CreateUserRequest request)
    {
        try
        {
            var user = service.Create(request);
            return Results.Created($"/api/users/{user.Id}", user);
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            return Results.BadRequest(new ErrorResponse(ex.Message));
        }
    }

    public IResult UpdateUser(long id, UpdateUserRequest request)
    {
        try
        {
            var user = service.Update(id, request);
            return user is null
                ? Results.NotFound()
                : Results.Ok(user);
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            return Results.BadRequest(new ErrorResponse(ex.Message));
        }
    }

    public IResult PatchUser(long id, PatchUserRequest request)
    {
        try
        {
            var user = service.Patch(id, request);
            return user is null
                ? Results.NotFound()
                : Results.Ok(user);
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            return Results.BadRequest(new ErrorResponse(ex.Message));
        }
    }

    public IResult DeleteUser(long id) =>
        service.Delete(id)
            ? Results.NoContent()
            : Results.NotFound();
}
