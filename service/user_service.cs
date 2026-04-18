using model;
using domain;
using repository;

namespace service;

class UserService(UserRepository repository)
{
    public Task<IReadOnlyList<User>> ListAsync() => repository.ListAsync();

    public Task<User?> GetAsync(long id) => repository.GetByIdAsync(id);

    public async Task<User> CreateAsync(CreateUserRequest request)
    {
        var name = NormalizeRequired(request.Name, nameof(request.Name));
        var email = NormalizeRequired(request.Email, nameof(request.Email));
        await EnsureEmailIsUniqueAsync(email);

        return await repository.CreateAsync(name, email);
    }

    public async Task<User?> UpdateAsync(long id, UpdateUserRequest request)
    {
        var name = NormalizeRequired(request.Name, nameof(request.Name));
        var email = NormalizeRequired(request.Email, nameof(request.Email));

        if (await repository.GetByIdAsync(id) is null)
            return null;

        await EnsureEmailIsUniqueAsync(email, id);
        return await repository.UpdateAsync(id, name, email);
    }

    public async Task<User?> PatchAsync(long id, PatchUserRequest request)
    {
        var existing = await repository.GetByIdAsync(id);
        if (existing is null)
            return null;

        var name = request.Name is null
            ? existing.Name
            : NormalizeRequired(request.Name, nameof(request.Name));

        var email = request.Email is null
            ? existing.Email
            : NormalizeRequired(request.Email, nameof(request.Email));

        await EnsureEmailIsUniqueAsync(email, id);
        return await repository.UpdateAsync(id, name, email);
    }

    public Task<bool> DeleteAsync(long id) => repository.DeleteAsync(id);

    private static string NormalizeRequired(string value, string fieldName)
    {
        var normalized = value.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
            throw new ArgumentException($"{fieldName} is required");

        return normalized;
    }

    private async Task EnsureEmailIsUniqueAsync(string email, long? exceptUserId = null)
    {
        if (await repository.EmailExistsAsync(email, exceptUserId))
            throw new InvalidOperationException("email already exists");
    }
}
