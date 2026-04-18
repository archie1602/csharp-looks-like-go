using model;
using service;

namespace handler;

class UserHandler(UserService service)
{
    public Task<IReadOnlyList<domain.User>> ListUsersAsync() => service.ListAsync();

    public async Task<IResult> GetUserAsync(long id)
    {
        var user = await service.GetAsync(id);
        return user is null
            ? Results.NotFound()
            : Results.Ok(user);
    }

    public async Task<IResult> CreateUserAsync(CreateUserRequest request)
    {
        var user = await service.CreateAsync(request);
        return Results.Created($"/api/users/{user.Id}", user);
    }

    public async Task<IResult> UpdateUserAsync(long id, UpdateUserRequest request)
    {
        var user = await service.UpdateAsync(id, request);
        return user is null
            ? Results.NotFound()
            : Results.Ok(user);
    }

    public async Task<IResult> PatchUserAsync(long id, PatchUserRequest request)
    {
        var user = await service.PatchAsync(id, request);
        return user is null
            ? Results.NotFound()
            : Results.Ok(user);
    }

    public async Task<IResult> DeleteUserAsync(long id) =>
        await service.DeleteAsync(id)
            ? Results.NoContent()
            : Results.NotFound();
}
