using domain;
using model;
using service;

namespace handler;

class UserHandler(UserService service)
{
    public Task<IReadOnlyList<User>> ListUsers() => service.List();

    public async Task<IResult> GetUser(long id)
    {
        var user = await service.Get(id);
        return user is null
            ? Results.NotFound()
            : Results.Ok(user);
    }

    public async Task<IResult> CreateUser(CreateUserRequest request)
    {
        var user = await service.Create(request);
        return Results.Created($"/api/users/{user.Id}", user);
    }

    public async Task<IResult> UpdateUser(long id, UpdateUserRequest request)
    {
        var user = await service.Update(id, request);
        return user is null
            ? Results.NotFound()
            : Results.Ok(user);
    }

    public async Task<IResult> PatchUser(long id, PatchUserRequest request)
    {
        var user = await service.Patch(id, request);
        return user is null
            ? Results.NotFound()
            : Results.Ok(user);
    }

    public async Task<IResult> DeleteUser(long id) =>
        await service.Delete(id)
            ? Results.NoContent()
            : Results.NotFound();
}
