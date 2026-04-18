using domain;
using model;
using System.Text.Json.Serialization;

namespace config;

[JsonSerializable(typeof(CreateUserRequest))]
[JsonSerializable(typeof(UpdateUserRequest))]
[JsonSerializable(typeof(PatchUserRequest))]
[JsonSerializable(typeof(CreatePostRequest))]
[JsonSerializable(typeof(UpdatePostRequest))]
[JsonSerializable(typeof(PatchPostRequest))]
[JsonSerializable(typeof(ErrorResponse))]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(List<User>))]
[JsonSerializable(typeof(IReadOnlyList<User>))]
[JsonSerializable(typeof(Post))]
[JsonSerializable(typeof(List<Post>))]
[JsonSerializable(typeof(IReadOnlyList<Post>))]
partial class AppJsonSerializerContext : JsonSerializerContext;
