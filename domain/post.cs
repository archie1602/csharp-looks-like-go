namespace domain;

class Post
{
    public long Id { get; set; }

    public required string Title { get; set; }

    public required string Body { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    
    // entity relationship
    public long UserId { get; set; }

    public User User { get; set; } = null!;
}
