namespace domain;

class User
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    // entity relationship
    public ICollection<Post> Posts { get; set; } = [];
}
