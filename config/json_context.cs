using domain;
using model;
using System.Text.Json.Serialization;

namespace config;

[JsonSerializable(typeof(CreateUserRequest))]
[JsonSerializable(typeof(UpdateUserRequest))]
[JsonSerializable(typeof(PatchUserRequest))]
[JsonSerializable(typeof(ErrorResponse))]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(List<User>))]
[JsonSerializable(typeof(IReadOnlyList<User>))]
partial class AppJsonSerializerContext : JsonSerializerContext;
