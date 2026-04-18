using model;
using domain;
using repository;

namespace service;

class UserService(UserRepository repository)
{
    public Task<IReadOnlyList<User>> List() => repository.List();

    public Task<User?> Get(long id) => repository.GetById(id);

    public async Task<User> Create(CreateUserRequest request)
    {
        var name = NormalizeRequired(request.Name, nameof(request.Name));
        var email = NormalizeRequired(request.Email, nameof(request.Email));
        await EnsureEmailIsUnique(email);

        return await repository.Create(name, email);
    }

    public async Task<User?> Update(long id, UpdateUserRequest request)
    {
        var name = NormalizeRequired(request.Name, nameof(request.Name));
        var email = NormalizeRequired(request.Email, nameof(request.Email));

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
            : NormalizeRequired(request.Name, nameof(request.Name));

        var email = request.Email is null
            ? existing.Email
            : NormalizeRequired(request.Email, nameof(request.Email));

        await EnsureEmailIsUnique(email, id);
        return await repository.Update(id, name, email);
    }

    public Task<bool> Delete(long id) => repository.Delete(id);

    private static string NormalizeRequired(string value, string fieldName)
    {
        var normalized = value.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
            throw new ArgumentException($"{fieldName} is required");

        return normalized;
    }

    private async Task EnsureEmailIsUnique(string email, long? exceptUserId = null)
    {
        if (await repository.EmailExists(email, exceptUserId))
            throw new InvalidOperationException("email already exists");
    }
}
