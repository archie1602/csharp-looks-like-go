using db;
using domain;
using Microsoft.EntityFrameworkCore;

namespace repository;

class PostRepository(AppDbContext db)
{
    public async Task<IReadOnlyList<Post>> ListByUser(long userId) =>
        await db.Posts
            .Where(post => post.UserId == userId)
            .OrderByDescending(post => post.CreatedAt)
            .ToListAsync();

    public Task<Post?> Get(long userId, long id) =>
        db.Posts.FirstOrDefaultAsync(post =>
            post.Id == id &&
            post.UserId == userId);

    public async Task<Post> Create(long userId, string title, string body)
    {
        var post = new Post
        {
            UserId = userId,
            Title = title,
            Body = body,
        };
        db.Posts.Add(post);
        await db.SaveChangesAsync();
        return post;
    }

    public async Task<Post?> Update(long userId, long id, string title, string body)
    {
        var post = await Get(userId, id);
        if (post is null)
            return null;

        post.Title = title;
        post.Body = body;
        await db.SaveChangesAsync();
        return post;
    }

    public async Task<bool> Delete(long userId, long id)
    {
        var post = await Get(userId, id);
        if (post is null)
            return false;

        db.Posts.Remove(post);
        await db.SaveChangesAsync();
        return true;
    }
}
