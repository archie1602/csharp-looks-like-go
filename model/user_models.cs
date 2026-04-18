namespace model;

record CreateUserRequest(string Name, string Email);

record UpdateUserRequest(string Name, string Email);

record PatchUserRequest(string? Name, string? Email);

record ErrorResponse(string Error);
