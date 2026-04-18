using model;
using domain;
using repository;
using util;

namespace service;

class UserService(UserRepository repository)
{
    public Task<IReadOnlyList<User>> List() => repository.List();

    public Task<User?> Get(long id) => repository.GetById(id);

    public async Task<User> Create(CreateUserRequest request)
    {
        var name = request.Name.RequireNonEmpty(nameof(request.Name));
        var email = request.Email.RequireNonEmpty(nameof(request.Email));
        await EnsureEmailIsUnique(email);

        return await repository.Create(name, email);
    }

    public async Task<User?> Update(long id, UpdateUserRequest request)
    {
        var name = request.Name.RequireNonEmpty(nameof(request.Name));
        var email = request.Email.RequireNonEmpty(nameof(request.Email));

        if (await repository.GetById(id) is null)
            return null;

        await EnsureEmailIsUnique(email, id);
        return await repository.Update(id, name, email);
    }

    public async Task<User?> Patch(long id, PatchUserRequest request)
    {
        var existing = await repository.GetById(id);
        if (existing is null)
            return null;

        var name = request.Name is null
            ? existing.Name
            : request.Name.RequireNonEmpty(nameof(request.Name));

        var email = request.Email is null
            ? existing.Email
            : request.Email.RequireNonEmpty(nameof(request.Email));

        await EnsureEmailIsUnique(email, id);
        return await repository.Update(id, name, email);
    }

    public Task<bool> Delete(long id) => repository.Delete(id);

    private async Task EnsureEmailIsUnique(string email, long? exceptUserId = null)
    {
        if (await repository.EmailExists(email, exceptUserId))
            throw new InvalidOperationException("email already exists");
    }
}
