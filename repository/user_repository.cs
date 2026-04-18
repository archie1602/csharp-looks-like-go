using db;
using domain;

namespace repository;

class UserRepository(AppDbContext db)
{
    public IReadOnlyList<User> List() => db.Users.OrderBy(user => user.Id).ToList();

    public User? GetById(long id) => db.Users.Find(id);

    public bool EmailExists(string email, long? exceptUserId = null)
    {
        var normalizedEmail = email.ToLower();
        return db.Users.Any(user =>
            user.Email.ToLower() == normalizedEmail &&
            user.Id != exceptUserId);
    }

    public User Create(string name, string email)
    {
        var user = new User { Name = name, Email = email };
        db.Users.Add(user);
        db.SaveChanges();
        return user;
    }

    public User? Update(long id, string name, string email)
    {
        var user = GetById(id);
        if (user is null)
            return null;

        user.Name = name;
        user.Email = email;
        db.SaveChanges();
        return user;
    }

    public bool Delete(long id)
    {
        var user = GetById(id);
        if (user is null)
            return false;

        db.Users.Remove(user);
        db.SaveChanges();
        return true;
    }
}
