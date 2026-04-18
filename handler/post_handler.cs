using domain;
using model;
using service;

namespace handler;

class PostHandler(PostService service)
{
    public Task<IReadOnlyList<Post>> ListPosts(long userId) => service.List(userId);

    public async Task<IResult> GetPost(long userId, long id)
    {
        var post = await service.Get(userId, id);
        return post is null
            ? Results.NotFound()
            : Results.Ok(post);
    }

    public async Task<IResult> CreatePost(long userId, CreatePostRequest request)
    {
        var post = await service.Create(userId, request);
        return Results.Created($"/api/users/{userId}/posts/{post.Id}", post);
    }

    public async Task<IResult> UpdatePost(long userId, long id, UpdatePostRequest request)
    {
        var post = await service.Update(userId, id, request);
        return post is null
            ? Results.NotFound()
            : Results.Ok(post);
    }

    public async Task<IResult> PatchPost(long userId, long id, PatchPostRequest request)
    {
        var post = await service.Patch(userId, id, request);
        return post is null
            ? Results.NotFound()
            : Results.Ok(post);
    }

    public async Task<IResult> DeletePost(long userId, long id) =>
        await service.Delete(userId, id)
            ? Results.NoContent()
            : Results.NotFound();
}
