using System.ComponentModel.DataAnnotations;

namespace model;

record CreateUserRequest(
    [property: Required, StringLength(100, MinimumLength = 1)]
    string Name,
    [property: Required, EmailAddress, StringLength(320)]
    string Email);

record UpdateUserRequest(
    [property: Required, StringLength(100, MinimumLength = 1)]
    string Name,
    [property: Required, EmailAddress, StringLength(320)]
    string Email);

record PatchUserRequest(
    [property: StringLength(100, MinimumLength = 1)]
    string? Name,
    [property: EmailAddress, StringLength(320)]
    string? Email);

record ErrorResponse(string Error);
