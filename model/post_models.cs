using System.ComponentModel.DataAnnotations;

namespace model;

record CreatePostRequest(
    [property: Required, StringLength(200, MinimumLength = 1)]
    string Title,
    [property: Required, MinLength(1)]
    string Body);

record UpdatePostRequest(
    [property: Required, StringLength(200, MinimumLength = 1)]
    string Title,
    [property: Required, MinLength(1)]
    string Body);

record PatchPostRequest(
    [property: StringLength(200, MinimumLength = 1)]
    string? Title,
    [property: MinLength(1)]
    string? Body);
