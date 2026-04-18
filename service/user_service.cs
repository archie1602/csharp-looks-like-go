using model;
using domain;
using repository;

namespace service;

class UserService(UserRepository repository)
{
    public IReadOnlyList<User> List() => repository.List();

    public User? Get(long id) => repository.GetById(id);

    public User Create(CreateUserRequest request)
    {
        var name = NormalizeRequired(request.Name, nameof(request.Name));
        var email = NormalizeRequired(request.Email, nameof(request.Email));
        EnsureEmailIsUnique(email);

        return repository.Create(name, email);
    }

    public User? Update(long id, UpdateUserRequest request)
    {
        var name = NormalizeRequired(request.Name, nameof(request.Name));
        var email = NormalizeRequired(request.Email, nameof(request.Email));

        if (repository.GetById(id) is null)
        {
            return null;
        }

        EnsureEmailIsUnique(email, id);
        return repository.Update(id, name, email);
    }

    public User? Patch(long id, PatchUserRequest request)
    {
        var existing = repository.GetById(id);
        if (existing is null)
        {
            return null;
        }

        var name = request.Name is null
            ? existing.Name
            : NormalizeRequired(request.Name, nameof(request.Name));

        var email = request.Email is null
            ? existing.Email
            : NormalizeRequired(request.Email, nameof(request.Email));

        EnsureEmailIsUnique(email, id);
        return repository.Update(id, name, email);
    }

    public bool Delete(long id) => repository.Delete(id);

    private static string NormalizeRequired(string value, string fieldName)
    {
        var normalized = value.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new ArgumentException($"{fieldName} is required");
        }

        return normalized;
    }

    private void EnsureEmailIsUnique(string email, long? exceptUserId = null)
    {
        if (repository.EmailExists(email, exceptUserId))
        {
            throw new InvalidOperationException("email already exists");
        }
    }
}
