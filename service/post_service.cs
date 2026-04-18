using domain;
using model;
using repository;
using util;

namespace service;

class PostService(PostRepository posts, UserRepository users)
{
    public Task<IReadOnlyList<Post>> List(long userId) => posts.ListByUser(userId);

    public Task<Post?> Get(long userId, long id) => posts.Get(userId, id);

    public async Task<Post> Create(long userId, CreatePostRequest request)
    {
        await EnsureUserExists(userId);
        var title = request.Title.RequireNonEmpty(nameof(request.Title));
        var body = request.Body.RequireNonEmpty(nameof(request.Body));

        return await posts.Create(userId, title, body);
    }

    public async Task<Post?> Update(long userId, long id, UpdatePostRequest request)
    {
        await EnsureUserExists(userId);
        var title = request.Title.RequireNonEmpty(nameof(request.Title));
        var body = request.Body.RequireNonEmpty(nameof(request.Body));

        return await posts.Update(userId, id, title, body);
    }

    public async Task<Post?> Patch(long userId, long id, PatchPostRequest request)
    {
        await EnsureUserExists(userId);
        var existing = await posts.Get(userId, id);
        if (existing is null)
            return null;

        var title = request.Title is null
            ? existing.Title
            : request.Title.RequireNonEmpty(nameof(request.Title));

        var body = request.Body is null
            ? existing.Body
            : request.Body.RequireNonEmpty(nameof(request.Body));

        return await posts.Update(userId, id, title, body);
    }

    public Task<bool> Delete(long userId, long id) => posts.Delete(userId, id);

    private async Task EnsureUserExists(long userId)
    {
        if (await users.GetById(userId) is null)
            throw new InvalidOperationException($"user {userId} not found");
    }
}
