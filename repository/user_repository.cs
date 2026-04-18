using db;
using domain;
using Microsoft.EntityFrameworkCore;

namespace repository;

class UserRepository(AppDbContext db)
{
    public async Task<IReadOnlyList<User>> ListAsync() =>
        await db.Users.OrderBy(user => user.Id).ToListAsync();

    public async Task<User?> GetByIdAsync(long id) =>
        await db.Users.FindAsync(id).AsTask();

    public Task<bool> EmailExistsAsync(string email, long? exceptUserId = null)
    {
        var normalizedEmail = email.ToLower();
        return db.Users.AnyAsync(user =>
            user.Email.ToLower() == normalizedEmail &&
            user.Id != exceptUserId);
    }

    public async Task<User> CreateAsync(string name, string email)
    {
        var user = new User { Name = name, Email = email };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateAsync(long id, string name, string email)
    {
        var user = await GetByIdAsync(id);
        if (user is null)
            return null;

        user.Name = name;
        user.Email = email;
        await db.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var user = await GetByIdAsync(id);
        if (user is null)
            return false;

        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return true;
    }
}
